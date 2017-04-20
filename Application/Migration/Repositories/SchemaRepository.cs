using Application.Configuration;
using Application.Migration.Entities;
using Application.Migration.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Application.Migration.Repositories
{
    public interface ISchemaRepository
    {
        bool TableExists(string tableName);
        IList<Schema> GetSchemas();
        IList<Schema> GetTableSchema(string tableName);
        string ComposeScript(List<TableSchema> tablesDrop, List<TableSchema> tablesDropCols, List<TableSchema> tablesCreate, List<TableSchema> tablesCreateCols, List<TableSchema> tablesUpdate);
        void ExecuteComposition(string script);
    }
    public class SchemaRepository : ISchemaRepository
    {
        internal DatabaseConnection Connection { get; set; }
        internal StringBuilder Script { get; set; }

        public SchemaRepository(IDatabaseConnection conn)
        {
            this.Connection = (DatabaseConnection)conn;
            this.Script = new StringBuilder();
        }

        public bool TableExists(string tableName)
        {
            try
            {
                Connection.Execute("SELECT COUNT(*) FROM `" + tableName + "`;", null);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public IList<Schema> GetSchemas()
        {
            var query = "SELECT TABLE_NAME AS 'TableName', COLUMN_NAME AS 'ColumnName', DATA_TYPE AS 'DataType'," +
                "(CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END ) AS 'IsNullable'" +
                "FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + this.Connection.Connection.Database + "';";
            return Connection.ExecuteList<Schema>(query, null);
        }

        public IList<Schema> GetTableSchema(string tableName)
        {
            var query = "SELECT TABLE_NAME AS 'TableName', COLUMN_NAME AS 'ColumnName', DATA_TYPE AS 'DataType'," +
                "(CASE WHEN IS_NULLABLE = 'YES' THEN 1 ELSE 0 END ) AS 'IsNullable'" +
                "FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + this.Connection.Connection.Database + "' AND TABLE_NAME = '" + tableName + "';";
            return Connection.ExecuteList<Schema>(query, null);
        }

        public string ComposeScript(List<TableSchema> tablesDrop, List<TableSchema> tablesDropCols, List<TableSchema> tablesCreate, List<TableSchema> tablesCreateCols, List<TableSchema> tablesUpdate)
        {
            var Script = new StringBuilder();
            Script.Append(SCRIPT_HEADER.Replace("@DatabaseName@", Connection.Connection.Database));
            Script.Append(ComposeDropTables(tablesDrop));
            Script.Append(ComposeDropColumns(tablesDropCols));
            Script.Append(ComposeCreateTables(tablesCreate));
            Script.Append(ComposeCreateColumns(tablesCreateCols));
            Script.Append(ComposeUpdateColumns(tablesUpdate));
            Script.Append(SCRIPT_FOOTER);
            return Script.ToString();
        }

        public string ComposeDropTables(List<TableSchema> tables)
        {
            var query = Environment.NewLine;
            foreach (var table in tables)
            {
                query += "DROP TABLE `" + table.Name + "`;" + Environment.NewLine;
            }
            return query;
        }

        public string ComposeDropColumns(List<TableSchema> tables)
        {
            var query = Environment.NewLine;
            var dropStatements = new List<string>();
            foreach (var table in tables)
            {
                if (!table.Columns.Any()) continue;
                dropStatements.Clear();
                query += "ALTER TABLE `" + table.Name + "`" + Environment.NewLine;
                foreach (var col in table.Columns)
                {
                    if (col.IsForeignKey())
                    {
                        dropStatements.Add("  DROP CONSTRAINT `fk_" + table.Name + "_" + col.ForeignKeyReference() + "`");
                        dropStatements.Add("  DROP INDEX `idx_" + table.Name + "_" + col.Name + "`");
                    }
                    dropStatements.Add("  DROP COLUMN `" + col.Name + "`");
                }
                query += String.Join("," + Environment.NewLine, dropStatements) + ";" + Environment.NewLine;
            }
            return query;
        }

        public string ComposeCreateTables(List<TableSchema> tables)
        {
            var query = Environment.NewLine;
            foreach (var table in tables)
            {
                query += "CREATE TABLE `" + table.Name + "` (" + Environment.NewLine;
                foreach (var col in table.Columns.OrderBy(c => c.Order))
                {
                    query += "  `" + col.Name + "` " + col.Type + " " + (col.Nullable ? "DEFAULT" : "NOT") + " NULL" + (col.IsPrimaryKey() && col.AutoIncrementable() ? " AUTO_INCREMENT" : "") + "," + Environment.NewLine;
                }
                var pks = table.GetPrimaryKeys();
                if (pks.Any())
                {
                    query += "  PRIMARY KEY (";
                    foreach (var pk in pks)
                    {
                        query += "`" + pk.Name + "`,";
                    }
                    query = query.Remove(query.Length - 1);
                    query += ")," + Environment.NewLine;
                }
                var fks = table.GetForeignKeys();
                if (fks.Any())
                {
                    foreach (var fk in fks)
                    {
                        query += "  KEY `idx_" + table.Name + "_" + fk.Name + "` (`" + fk.Name + "`)," + Environment.NewLine;
                    }
                    foreach (var fk in fks)
                    {
                        query += "  CONSTRAINT `fk_" + table.Name + "_" + fk.ForeignKeyReference() + "` FOREIGN KEY (`" + fk.Name + "`) REFERENCES `" + fk.ForeignKeyReference() + "` (`Id`) ON UPDATE CASCADE ON DELETE " + (fk.Nullable ? "SET NULL" : "CASCADE") + "," + Environment.NewLine;
                    }
                }

                query = query.Remove(query.Length - Environment.NewLine.Length - 1);
                query += Environment.NewLine + ") ENGINE=InnoDB DEFAULT CHARSET=utf8;" + Environment.NewLine + Environment.NewLine;
            }

            return query;
        }

        public string ComposeCreateColumns(List<TableSchema> tables)
        {
            var query = Environment.NewLine;
            foreach (var table in tables)
            {
                if (!table.Columns.Any()) continue;

                query += "ALTER TABLE `" + table.Name + "` " + Environment.NewLine;
                foreach (var col in table.Columns.OrderBy(c => c.Order))
                {
                    query += "  ADD `" + col.Name + "` " + col.Type + " " + (col.Nullable ? "DEFAULT" : "NOT") + " NULL," + Environment.NewLine;
                }
                var fks = table.GetForeignKeys();
                if (fks.Any())
                {
                    foreach (var fk in fks)
                    {
                        query += "  ADD KEY `idx_" + table.Name + "_" + fk.Name + "` (`" + fk.Name + "`)," + Environment.NewLine;
                    }
                    foreach (var fk in fks)
                    {
                        query += "  ADD CONSTRAINT `fk_" + table.Name + "_" + fk.ForeignKeyReference() + "` FOREIGN KEY (`" + fk.Name + "`) REFERENCES `" + fk.ForeignKeyReference() + "` (`Id`) ON UPDATE CASCADE ON DELETE " + (fk.Nullable ? "SET NULL" : "CASCADE") + "," + Environment.NewLine;
                    }
                }

                query = query.Remove(query.Length - Environment.NewLine.Length - 1);
                query += ";" + Environment.NewLine + Environment.NewLine;
            }
            return query;
        }

        public string ComposeUpdateColumns(List<TableSchema> tables)
        {
            var query = Environment.NewLine;
            foreach (var table in tables)
            {
                if (!table.Columns.Any()) continue;

                query += "ALTER TABLE `" + table.Name + "` " + Environment.NewLine;
                foreach (var col in table.Columns.OrderBy(c => c.Order))
                {
                    query += "  MODIFY `" + col.Name + "` " + col.Type + " " + (col.Nullable ? "DEFAULT" : "NOT") + " NULL," + Environment.NewLine;
                }
                query = query.Remove(query.Length - Environment.NewLine.Length - 1);
                query += ";" + Environment.NewLine + Environment.NewLine;
            }
            return query;
        }

        public void ExecuteComposition(string script)
        {
            Connection.Execute(script, null);
        }

        private const string SCRIPT_HEADER = @"
/*!40101 SET NAMES utf8 */;
/*!40101 SET SQL_MODE=''*/;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

CREATE DATABASE /*!32312 IF NOT EXISTS*/`@DatabaseName@` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `@DatabaseName@`;
";

        private const string SCRIPT_FOOTER = @"
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
";
    }

}
