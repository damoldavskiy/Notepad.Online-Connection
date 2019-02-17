using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataBase
{
    public partial class Instance
    {
        static SqlConnection connection;

        static Instance()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = "notepadonline.database.windows.net",
                InitialCatalog = "NotepadOnlineDatabase",
                Password = "comm0np@ss",
                UserID = "common"
            };

            connection = new SqlConnection(builder.ConnectionString);
        }

        static ReturnCode Execute(string procedure, Dictionary<string, string> parameters)
        {
            if (connection.State != ConnectionState.Closed)
                return ReturnCode.Busy;

            var command = new SqlCommand()
            {
                CommandText = procedure,
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            foreach (var pair in parameters)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            var result = command.Parameters.Add("@result", SqlDbType.Int);
            result.Direction = ParameterDirection.ReturnValue;

            try
            {
				connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return ReturnCode.NoConnection;
            }
            finally
            {
                connection.Close();
            }

            return (ReturnCode)result.Value;
        }

        static async Task<ReturnCode> ExecuteAsync(string procedure, Dictionary<string, string> parameters)
        {
            if (connection.State != ConnectionState.Closed)
                return ReturnCode.Busy;

            var command = new SqlCommand()
            {
                CommandText = procedure,
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            foreach (var pair in parameters)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            var result = command.Parameters.Add("@result", SqlDbType.Int);
            result.Direction = ParameterDirection.ReturnValue;

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                return ReturnCode.NoConnection;
            }
            finally
            {
                connection.Close();
            }

            return (ReturnCode)result.Value;
        }

        static Tuple<ReturnCode, List<string>> ExecuteGet(string procedure, Dictionary<string, string> parameters)
        {
            if (connection.State != ConnectionState.Closed)
                return new Tuple<ReturnCode, List<string>>(ReturnCode.Busy, null);

            var command = new SqlCommand()
            {
                CommandText = procedure,
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            foreach (var pair in parameters)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            var result = command.Parameters.Add("@result", SqlDbType.Int);
            result.Direction = ParameterDirection.ReturnValue;

            List<string> results;
            try
            {
                connection.Open();

                var reader = command.ExecuteReader();
                results = new List<string>();

                while (reader.Read())
                    for (int i = 0; i < reader.FieldCount; i++)
                        results.Add(reader.GetValue(i).ToString());
            }
            catch (SqlException)
            {
                return new Tuple<ReturnCode, List<string>>(ReturnCode.NoConnection, null);
            }
            finally
            {
                connection.Close();
            }

            return new Tuple<ReturnCode, List<string>>((ReturnCode)result.Value, results);
        }

        static async Task<Tuple<ReturnCode, List<string>>> ExecuteGetAsync(string procedure, Dictionary<string, string> parameters)
        {
            if (connection.State != ConnectionState.Closed)
                return new Tuple<ReturnCode, List<string>>(ReturnCode.Busy, null);

            var command = new SqlCommand()
            {
                CommandText = procedure,
                CommandType = CommandType.StoredProcedure,
                Connection = connection
            };

            foreach (var pair in parameters)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            var result = command.Parameters.Add("@result", SqlDbType.Int);
            result.Direction = ParameterDirection.ReturnValue;

            List<string> results;
            try
            {
                await connection.OpenAsync();

                var reader = await command.ExecuteReaderAsync();
                results = new List<string>();

                while (await reader.ReadAsync())
                    for (int i = 0; i < reader.FieldCount; i++)
                        results.Add(reader.GetValue(i).ToString());
            }
            catch (SqlException)
            {
                return new Tuple<ReturnCode, List<string>>(ReturnCode.NoConnection, null);
            }
            finally
            {
                connection.Close();
            }

            return new Tuple<ReturnCode, List<string>>((ReturnCode)result.Value, results);
        }
    }
}
