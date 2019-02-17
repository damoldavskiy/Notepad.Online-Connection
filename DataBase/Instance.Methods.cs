using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase
{
    public partial class Instance
    {
        public static ReturnCode Register(string email, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);
            return Execute("Register", parameters);
        }

        public static async Task<ReturnCode> RegisterAsync(string email, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);
            return await ExecuteAsync("Register", parameters);
        }

        public static ReturnCode ConfirmRegistration(string email, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@code", code);
            return Execute("ConfirmRegistration", parameters);
        }

        public static async Task<ReturnCode> ConfirmRegistrationAsync(string email, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@code", code);
            return await ExecuteAsync("ConfirmRegistration", parameters);
        }

        public static ReturnCode Recovery(string email)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            return Execute("Recovery", parameters);
        }

        public static async Task<ReturnCode> RecoveryAsync(string email)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            return await ExecuteAsync("Recovery", parameters);
        }

        public static ReturnCode ConfirmRecovery(string email, string newpassword, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@newpassword", newpassword);
            parameters.Add("@code", code);
            return Execute("ConfirmRecovery", parameters);
        }

        public static async Task<ReturnCode> ConfirmRecoveryAsync(string email, string newpassword, string code)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@newpassword", newpassword);
            parameters.Add("@code", code);
            return await ExecuteAsync("ConfirmRecovery", parameters);
        }

        public static Tuple<ReturnCode, string> Login(string email, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);
            var result = ExecuteGet("Login", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string>(result.Item1, null);
            return new Tuple<ReturnCode, string>(result.Item1, result.Item2.Count > 0 ? result.Item2[0] : null);
        }

        public static async Task<Tuple<ReturnCode, string>> LoginAsync(string email, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);
            var result = await ExecuteGetAsync("Login", parameters);

            if (result.Item1 != ReturnCode.Success)
                return new Tuple<ReturnCode, string>(result.Item1, null);
            return new Tuple<ReturnCode, string>(result.Item1, result.Item2.Count > 0 ? result.Item2[0] : null);
        }

        public static ReturnCode ChangePassword(string email, string password, string newpassword)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);
            parameters.Add("@newpassword", newpassword);
            return Execute("ChangePassword", parameters);
        }

        public static async Task<ReturnCode> ChangePasswordAsync(string email, string password, string newpassword)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("@email", email);
            parameters.Add("@password", password);
            parameters.Add("@newpassword", newpassword);
            return await ExecuteAsync("ChangePassword", parameters);
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
