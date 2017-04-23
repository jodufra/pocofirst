using Application.Configuration;
using Application.Migration.Entities;
using Application.Migration.Models;
using Application.Migration.Repositories;
using Application.Migration.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Application.Migration
{
    public class MigratorOptions
    {
        public List<Type> EntitiesExcluded { get; set; }
        public string EntitiesNamespace { get; set; }
        public string ScriptOutputPath { get; set; }
        public MigratorOptions()
        {
            EntitiesExcluded = new List<Type>();
            EntitiesNamespace = "Application.Entities";
            ScriptOutputPath = "../../Scripts/";
        }
    }
    public interface IDataMigrator
    {
        void Migrate(IDatabaseConnection dbConnection);
    }
    public class Migrator
    {
        private IDatabaseConnection DbConnection;
        private ISchemaRepository SchemaRepository;
        private List<Type> Entities;
        private IList<Schema> Schemas;
        private Dictionary<string, Table> Tables;

        private MigratorOptions Options;

        public Migrator(IDatabaseConnection dbConnection) : this(dbConnection, new MigratorOptions()) { }
        public Migrator(IDatabaseConnection dbConnection, MigratorOptions options)
        {
            DbConnection = dbConnection;
            SchemaRepository = new SchemaRepository(dbConnection);
            Options = options;
        }

        public string GetDatabaseUpdateScript()
        {
            LoadSchemasAndEntities();
            var script = ComposeScript();
            if (!string.IsNullOrEmpty(script))
                SaveScript(script);
            return script;
        }

        public void RunDbUpdate(string script)
        {
            SchemaRepository.ExecuteComposition(script);
        }

        public void RunDbUpdate(IDataMigrator rawDataMigrator)
        {
            rawDataMigrator.Migrate(DbConnection);
        }

        public void RunDbUpdate(string script, IDataMigrator rawDataMigrator)
        {
            RunDbUpdate(script);
            RunDbUpdate(rawDataMigrator);
        }

        private void LoadSchemasAndEntities()
        {
            Schemas = SchemaRepository.GetSchemas();
            Entities = TypesLookupService.DataContractsInNamespace(Options.EntitiesNamespace, Options.EntitiesExcluded).ToList();
            Tables = new Dictionary<string, Table>();

            Table table; Type type; PropertyInfo property;
            BindingFlags instancePublic = BindingFlags.Instance | BindingFlags.Public;

            foreach (var schema in Schemas)
            {
                if (!Tables.ContainsKey(schema.TableName))
                {
                    type = Entities.Where(e => e.Name == schema.TableName.ToPascalCase('_')).FirstOrDefault();
                    table = type != null ? new Table(schema, type) : new Table(schema);
                    Tables.Add(schema.TableName, table);
                }
                else
                {
                    table = Tables[schema.TableName];
                }

                property = null;

                if (table.Entity != null)
                {
                    property = table.Entity.GetProperties(instancePublic).Where(x => Attribute.IsDefined(x, typeof(DataMemberAttribute))).FirstOrDefault();
                }

                table[schema.ColumnName] = property != null ? new Column(property, schema) : new Column(schema);
            }

            foreach (var entity in Entities)
            {
                string tableName = entity.Name.ToSnakeCase('_');

                if (!Tables.ContainsKey(tableName))
                {
                    Tables.Add(tableName, new Table(entity));
                }

                table = Tables[tableName];

                if (table.Entity == null)
                {
                    table.Entity = entity;
                }

                var fields = table.Entity.GetProperties(instancePublic).Where(x => Attribute.IsDefined(x, typeof(DataMemberAttribute)));

                foreach (var field in fields)
                {
                    string columnName = field.Name;
                    if (!table.HasColumn(columnName))
                    {
                        table[columnName] = new Column(field);
                    }
                }
            }
        }

        private string ComposeScript()
        {
            if (Tables.Values.Where(tv => tv.Operation != Activity.Ignore).Count() == 0)
                return null;

            // drop columns
            var tablesDropCols = Tables.Values.Where(tv => tv.Operation == Activity.Update).Select(tv => tv.GetSchema(Activity.Drop)).ToList();
            if (tablesDropCols.Any())
            {
                tablesDropCols.ForEach(t => t.Columns.ForEach(c => Tables[t.Name].RemoveColumn(c.Name)));
            }

            // drop tables
            var tablesDrop = Tables.Values.Where(tv => tv.Operation == Activity.Drop).Select(tv => tv.GetFullSchema()).ToList();
            if (tablesDrop.Any())
            {
                tablesDrop.ForEach(t => Tables.Remove(t.Name));
            }

            // create tables
            var tablesCreate = Tables.Values.Where(tv => tv.Operation == Activity.Create).Select(t => t.GetFullSchema()).ToList();
            if (tablesCreate.Any())
            {
                tablesCreate.ForEach(t => Tables.Remove(t.Name));
            }

            // create columns
            var tablesCreateCols = Tables.Values.Select(t => t.GetSchema(Activity.Create)).ToList();
            if (tablesCreateCols.Any())
            {
                tablesCreateCols.ForEach(t => Tables.Remove(t.Name));
            }

            // update columns
            var tablesUpdate = Tables.Values.Select(t => t.GetSchema(Activity.Update)).ToList();
            if (tablesUpdate.Any())
            {
                tablesUpdate.ForEach(t => Tables.Remove(t.Name));
            }

            return SchemaRepository.ComposeScript(tablesDrop, tablesDropCols, tablesCreate, tablesCreateCols, tablesUpdate);
        }

        private void SaveScript(string script)
        {
            var now = DateTime.Now;
            var filename = "migrate_" + now.Year.ToString("0000") + now.Month.ToString("00") + now.Day.ToString("00") + now.Hour.ToString("00") + now.Minute.ToString("00") + ".sql";
            //File.WriteAllText(Options.ScriptOutputPath + filename, script);
        }

    }
}
