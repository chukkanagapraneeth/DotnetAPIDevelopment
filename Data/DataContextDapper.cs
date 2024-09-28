using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        public IEnumerable<T> LoadData<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            return dbConnection.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            return dbConnection.QuerySingle<T>(sql);
        }

        // METHOD OVERLOADING
        public bool ExecuteSql(string sql, out int rowsAffected)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            rowsAffected = dbConnection.Execute(sql);
            return rowsAffected > 0;
        }

        public bool ExecuteSql(string sql)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );
            int rowsAffected;
            return ExecuteSql(sql, out rowsAffected);
        }

        public bool ExecuteSqlWithParameters(string sql, List<SqlParameter> parameters)
        {
            IDbConnection dbConnection = new SqlConnection(
                _config.GetConnectionString("DefaultConnection")
            );

            SqlCommand command = new SqlCommand(sql);
            foreach (SqlParameter sqlParameter in parameters)
            {
                command.Parameters.Add(sqlParameter);
            }

            dbConnection.Open();
            command.Connection = (SqlConnection)dbConnection;

            int rowsAffected = command.ExecuteNonQuery();
            dbConnection.Close();
            return rowsAffected > 0;
        }
    }
}
