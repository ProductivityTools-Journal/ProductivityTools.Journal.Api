using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace ProductivityTools.Journal.Database
{
    public class DatabaseHelpers
    {
        public static bool ExecutVerifyOwnership(DbContext context, string email, int[] treeIds)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            if (treeIds == null || treeIds.Length == 0)
            {
                return true;
            }

            var connection = context.Database.GetDbConnection() as SqlConnection;
            bool shouldDisposeConnection = false;
            if (connection == null)
            {
                string connectionString = context.Database.GetConnectionString();
                connection = new SqlConnection(connectionString);
                shouldDisposeConnection = true;
            }

            bool shouldCloseConnection = false;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                shouldCloseConnection = true;
            }

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "j.VerifyOwnership";

                    var emailParameter = command.Parameters.Add("@email", SqlDbType.VarChar, 100);
                    emailParameter.Direction = ParameterDirection.Input;
                    emailParameter.Value = email;

                    DataTable table = new DataTable();
                    table.Columns.Add("TreeId", typeof(int));
                    foreach (var treeId in treeIds)
                    {
                        table.Rows.Add(treeId);
                    }

                    var treeIdsParameter = command.Parameters.Add("@TreeIds", SqlDbType.Structured);
                    treeIdsParameter.Direction = ParameterDirection.Input;
                    treeIdsParameter.TypeName = "j.TreeArray";
                    treeIdsParameter.Value = table;

                    var returnValue = command.Parameters.Add("@HasPermission", SqlDbType.Bit);
                    returnValue.Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    return returnValue.Value != null && returnValue.Value != DBNull.Value && (bool)returnValue.Value;
                }
            }
            finally
            {
                if (shouldCloseConnection)
                {
                    connection.Close();
                }
                if (shouldDisposeConnection)
                {
                    connection.Dispose();
                }
            }
        }
    }
}
