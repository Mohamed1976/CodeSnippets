using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataLibraryDapper
{
    public class DataAccess : IDataAccess
    {
        public DataAccess(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private string ConnectionString { get; set; }

        private SqlConnection sqlConnection = null;
        private bool disposedValue;

        private SqlConnection GetConnection()
        {
            if(sqlConnection == null)
                sqlConnection = new SqlConnection(ConnectionString);

            return sqlConnection;
        }

        public Task<IEnumerable<U>> LoadDataAsync<T, U>(string sql, T parameters)
        {
            return GetConnection().QueryAsync<U>(sql, parameters);
            //IDbConnection connection = GetConnection();
            //return connection.QueryAsync<U>(sql, parameters);
            //Task<IEnumerable<U>> rows = connection.QueryAsync<U>(sql, parameters);
            //return rows;
        }

        public IEnumerable<U> LoadData<T, U>(string sql, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<U>(sql, parameters);
            }
        }

        //Returns the number of rows affected
        public Task<int> SaveDataAsync<T>(string sql, T parameters)
        {
            return GetConnection().ExecuteAsync(sql, parameters);
        }

        //Returns the number of rows affected
        public int SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                return connection.Execute(sql, parameters);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(sqlConnection != null)
                    {
                        sqlConnection.Close();
                        sqlConnection.Dispose();
                        sqlConnection = null;
                    }

                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DataAccess()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
