using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Application.Configuration
{
    public interface IDatabaseConnection
    {
        IDbConnection Connection { get; set; }
    }

    public class DatabaseConnection : IDatabaseConnection
    {
        public IDbConnection Connection { get; set; }

        public DatabaseConnection()
        {
            this.Connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        }

        public DatabaseConnection(MySqlConnection connection)
        {
            this.Connection = connection;
        }

        public DatabaseConnection(string connectionString)
        {
            this.Connection = new MySqlConnection(connectionString);
        }

        public bool IsConnectionOpen()
        {
            return Connection.State == ConnectionState.Open;
        }

        public IDbConnection GetConnection()
        {
            return this.Connection;
        }

        public void SetOpened()
        {
            if (!IsConnectionOpen())
                this.Connection.Open();
        }

        public void SetClosed()
        {
            if (IsConnectionOpen())
                this.Connection.Close();
        }

        public void Close()
        {
            if (this.Connection != null)
            {
                this.SetClosed();
                this.Connection.Dispose();
            }
        }

        public void Execute(string sql, object param)
        {
            using (IDbConnection cn = this.Connection)
            {
                this.SetOpened();
                cn.Execute(sql, param);
                this.SetClosed();
            }
        }

        public T ExecuteSingle<T>(string sql, object param)
        {
            var result = default(T);
            using (IDbConnection cn = this.Connection)
            {
                this.SetOpened();
                result = cn.Query<T>(sql, param).FirstOrDefault();
                this.SetClosed();
            }
            return result;
        }

        public IList<T> ExecuteList<T>(string sql, object param)
        {
            IList<T> result = null;
            using (IDbConnection cn = this.Connection)
            {
                this.SetOpened();
                result = cn.Query<T>(sql, param).ToList();
                this.SetClosed();
            }
            return result;
        }
    }
}
