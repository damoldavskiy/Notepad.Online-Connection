using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataBase
{
    public class Instance
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

        public static ReturnCode Register(string login, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@login", login);
            parameters.Add("@password", password);
            return Execute("Register", parameters);
        }

        public static async Task<ReturnCode> RegisterAsync(string login, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@login", login);
            parameters.Add("@password", password);
            return await ExecuteAsync("Register", parameters);
        }

        public static ReturnCode Confirm(string login, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@login", login);
            parameters.Add("@code", code);
            return Execute("Confirm", parameters);
        }

        public static async Task<ReturnCode> ConfirmAsync(string login, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@login", login);
            parameters.Add("@code", code);
            return await ExecuteAsync("Confirm", parameters);
        }

        public static Tuple<ReturnCode, string> Authorize(string login, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@login", login);
            parameters.Add("@password", password);
            var result = ExecuteGet("Authorize", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string>(result.Item1, null);
            return new Tuple<ReturnCode, string>(result.Item1, result.Item2.Count > 0 ? result.Item2[0] : null);
        }

        public static async Task<Tuple<ReturnCode, string>> AuthorizeAsync(string login, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@login", login);
            parameters.Add("@password", password);
            var result = await ExecuteGetAsync("Authorize", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string>(result.Item1, null);
            return new Tuple<ReturnCode, string>(result.Item1, result.Item2.Count > 0 ? result.Item2[0] : null);
        }

        public static Tuple<ReturnCode, string[]> GetNames(string token)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            var result = ExecuteGet("GetNames", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string[]>(result.Item1, null);
            return new Tuple<ReturnCode, string[]>(result.Item1, result.Item2.ToArray());
        }

        public static async Task<Tuple<ReturnCode, string[]>> GetNamesAsync(string token)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            var result = await ExecuteGetAsync("GetNames", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string[]>(result.Item1, null);
            return new Tuple<ReturnCode, string[]>(result.Item1, result.Item2.ToArray());
        }

        public static Tuple<ReturnCode, string, string> GetData(string token, string name)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            var result = ExecuteGet("GetData", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string, string>(result.Item1, null, null);
            return new Tuple<ReturnCode, string, string>(result.Item1, result.Item2[0], result.Item2[1]);
        }

        public static async Task<Tuple<ReturnCode, string, string>> GetDataAsync(string token, string name)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            var result = await ExecuteGetAsync("GetData", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string, string>(result.Item1, null, null);
            return new Tuple<ReturnCode, string, string>(result.Item1, result.Item2[0], result.Item2[1]);
        }

        public static ReturnCode AddData(string token, string name, string description, string text)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@description", description);
            parameters.Add("@text", text);
            return Execute("AddData", parameters);
        }

        public static async Task<ReturnCode> AddDataAsync(string token, string name, string description, string text)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@description", description);
            parameters.Add("@text", text);
            return await ExecuteAsync("AddData", parameters);
        }

        public static ReturnCode DelData(string token, string name)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            return Execute("DelData", parameters);
        }

        public static async Task<ReturnCode> DelDataAsync(string token, string name)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            return await ExecuteAsync("DelData", parameters);
        }

        public static ReturnCode EditName(string token, string name, string newname)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@newname", newname);
            return Execute("EditName", parameters);
        }

        public static async Task<ReturnCode> EditNameAsync(string token, string name, string newname)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@newname", newname);
            return await ExecuteAsync("EditName", parameters);
        }

        public static ReturnCode EditDescription(string token, string name, string newdescription)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@newdescription", newdescription);
            return Execute("EditDescription", parameters);
        }

        public static async Task<ReturnCode> EditDescriptionAsync(string token, string name, string newdescription)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@newdescription", newdescription);
            return await ExecuteAsync("EditDescription", parameters);
        }

        public static ReturnCode EditText(string token, string name, string newtext)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@newtext", newtext);
            return Execute("EditText", parameters);
        }

        public static async Task<ReturnCode> EditTextAsync(string token, string name, string newtext)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@token", token);
            parameters.Add("@name", name);
            parameters.Add("@newtext", newtext);
            return await ExecuteAsync("EditText", parameters);
        }
    }
}
