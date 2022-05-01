using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.Journal.Database
{
    public class DatabaseHelpers
    {
        public static bool ExecutVerifyOwnership(DbContext context,string email,  int[] treeIds)
        {
            string connectionstring = context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionstring))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "jl.VerifyOwnership";

                SqlParameter emailparameter = command.Parameters.Add("@email", SqlDbType.Text);
                emailparameter.Direction = ParameterDirection.Input;
                emailparameter.Value = email;

                DataTable table = new DataTable();
                table.Columns.Add("TreeId", typeof(int));
                foreach (var treeId in treeIds)
                {
                    table.Rows.Add(treeId);
                }
                SqlParameter treeeIds = command.Parameters.Add("@TreeIds", SqlDbType.Structured);
                treeeIds.Direction = ParameterDirection.Input;
                treeeIds.Value = table;


                SqlParameter returnValue = command.Parameters.Add("@HasPermission", SqlDbType.Bit);
                returnValue.Direction = ParameterDirection.Output;

                connection.Open();
                
                var r = command.ExecuteNonQuery();
                return (bool)returnValue.Value;
            }
        }
    }
}
