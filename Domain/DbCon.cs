using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Domain
{
    public class DbCon : IDbCon
    {
        private readonly string ConnectionString;
        public DbCon(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("Daya");
        }

        public T Query<T>(string sql)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.QueryFirstOrDefault<T>(sql);
                return result;
            }
        }

        public List<T> QueryList<T>(string sql)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(sql).ToList();
                return result;
            }
        }

        public int Execute<T>(string sql,T dt)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var rowsEffected = con.Execute(sql, dt);
                return rowsEffected;
            }
        }

        public int Execute(string sql)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                var rowsEffected = con.Execute(sql);
                return rowsEffected;
            }
        }

        public List<T> CallStoredProsedure<T>(string name, Dictionary<string, dynamic> parameters)
        {
            DynamicParameters dps = new DynamicParameters();

            foreach (var item in parameters)
                dps.Add(item.Key, item.Value);

            using (var con = new SqlConnection(ConnectionString))
            {
                var result = con.Query<T>(name, dps, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }
    }
}
