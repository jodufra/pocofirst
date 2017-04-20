using Application.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Application.Migration
{
    public class Program
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;

        static void Main(string[] args)
        {
            Console.WriteLine("[Log] App init");

            if (!DatabaseMigrator.Run())
                Console.WriteLine("[Log] Nothing to migrate");

            Console.WriteLine();
            Console.WriteLine("[Log] Terminated");
        }
    }

    public static class DatabaseMigrator
    {
        public static bool Run()
        {
            var dbConnection = new DatabaseConnection(new MySqlConnection(Program.ConnectionString));

            var options = new MigratorOptions()
            {
                EntitiesExcluded = new List<Type>() { { typeof(Application.Entities.BaseEntity) }, { typeof(Application.Entities.CustomFieldEntity) } }
            };

            var migrator = new Migrator(dbConnection, options);

            var script = migrator.GetDatabaseUpdateScript();

            if (!string.IsNullOrEmpty(script))
            {
                migrator.RunDbUpdate(script);
                return true;
            }
            return false;
        }
    }


}
