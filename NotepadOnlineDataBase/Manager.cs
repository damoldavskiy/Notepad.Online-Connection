using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBase
{
    public class Manager
    {
        public static bool Authorized { get; private set; }
        public static bool WaitingForConfirm { get; private set; }
        public static string Login { get; private set; }
        public static string Password { get; private set; }
        public static string Token { get; private set; }

        static bool IsLoginValid(string login)
        {
            return login.Length >= 4 && login.Length <= 40 && Regex.IsMatch(login, @"\S+@\S+\.\S\S+", RegexOptions.IgnoreCase);
        }

        static bool IsPasswordValid(string password)
        {
            return password.Length >= 4 && password.Length <= 40;
        }

        static bool IsConfirmCodeValid(string code)
        {
            return code.Length == 4 && int.TryParse(code, out int num);
        }

        public static ReturnCode Register(string login, string password)
        {
            if (!IsLoginValid(login))
                return ReturnCode.IllegalLogin;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = Instance.Register(login, password);

            if (result == ReturnCode.Success)
            {
                WaitingForConfirm = true;
                Login = login;
                Password = password;
            }

            return result;
        }

        public static async Task<ReturnCode> RegisterAsync(string login, string password)
        {
            if (!IsLoginValid(login))
                return ReturnCode.IllegalLogin;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = await Instance.RegisterAsync(login, password);

            if (result == ReturnCode.Success)
            {
                WaitingForConfirm = true;
                Login = login;
                Password = password;
            }

            return result;
        }

        public static ReturnCode Confirm(string code)
        {
            if (!WaitingForConfirm)
                throw new Exception();

            if (!IsConfirmCodeValid(code))
                return ReturnCode.IllegalConfirmCode;

            var result = Instance.Confirm(Login, code);

            if (result == ReturnCode.Success)
            {
                WaitingForConfirm = false;
                Authorized = true;
                Authorize(Login, Password);
            }

            return result;
        }

        public static async Task<ReturnCode> ConfirmAsync(string code)
        {
            if (!WaitingForConfirm)
                throw new Exception();

            if (!IsConfirmCodeValid(code))
                return ReturnCode.IllegalConfirmCode;

            var result = await Instance.ConfirmAsync(Login, code);

            if (result == ReturnCode.Success)
            {
                WaitingForConfirm = false;
                Authorized = true;
                await AuthorizeAsync(Login, Password);
            }

            return result;
        }

        public static ReturnCode Authorize(string login, string password)
        {
            if (!IsLoginValid(login))
                return ReturnCode.IllegalLogin;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = Instance.Authorize(login, password);

            if (result.Item1 == ReturnCode.Success)
            {
                WaitingForConfirm = false;
                Authorized = true;
                Login = login;
                Password = password;
                Token = result.Item2;
            }

            return result.Item1;
        }

        public static async Task<ReturnCode> AuthorizeAsync(string login, string password)
        {
            if (!IsLoginValid(login))
                return ReturnCode.IllegalLogin;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = await Instance.AuthorizeAsync(login, password);

            if (result.Item1 == ReturnCode.Success)
            {
                WaitingForConfirm = false;
                Authorized = true;
                Login = login;
                Password = password;
                Token = result.Item2;
            }

            return result.Item1;
        }

        public static ReturnCode Authorize(string login, string password, string token)
        {
            if (!IsLoginValid(login))
                return ReturnCode.IllegalLogin;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = Instance.GetNames(token);

            if (result.Item1 == ReturnCode.Success)
            {
                WaitingForConfirm = false;
                Authorized = true;
                Login = login;
                Password = password;
                Token = token;
            }
            else if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
                return Authorize(login, password);

            return result.Item1;
        }

        public static async Task<ReturnCode> AuthorizeAsync(string login, string password, string token)
        {
            if (!IsLoginValid(login))
                return ReturnCode.IllegalLogin;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = await Instance.GetNamesAsync(token);

            if (result.Item1 == ReturnCode.Success)
            {
                WaitingForConfirm = false;
                Authorized = true;
                Login = login;
                Password = password;
                Token = token;
            }
            else if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
                return await AuthorizeAsync(login, password);

            return result.Item1;
        }

        public static Tuple<ReturnCode, string[]> GetNames()
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.GetNames(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.GetNames(Token);
            }

            return result;
        }

        public static async Task<Tuple<ReturnCode, string[]>> GetNamesAsync()
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.GetNamesAsync(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.GetNamesAsync(Token);
            }

            return result;
        }

        public static Tuple<ReturnCode, string, string> GetData(string name)
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.GetData(Token, name);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.GetData(Token, name);
            }

            return result;
        }

        public static async Task<Tuple<ReturnCode, string, string>> GetDataAsync(string name)
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.GetDataAsync(Token, name);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.GetDataAsync(Token, name);
            }

            return result;
        }

        public static ReturnCode AddData(string name, string description, string text)
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.AddData(Token, name, description, text);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.AddData(Token, name, description, text);
            }

            return result;
        }

        public static async Task<ReturnCode> AddDataAsync(string name, string description, string text)
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.AddDataAsync(Token, name, description, text);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.AddDataAsync(Token, name, description, text);
            }

            return result;
        }

        public static ReturnCode DelData(string name)
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.DelData(Token, name);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.DelData(Token, name);
            }

            return result;
        }

        public static async Task<ReturnCode> DelDataAsync(string name)
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.DelDataAsync(Token, name);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.DelDataAsync(Token, name);
            }

            return result;
        }

        public static ReturnCode EditName(string name, string newname)
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.EditName(Token, name, newname);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.EditName(Token, name, newname);
            }

            return result;
        }

        public static async Task<ReturnCode> EditNameAsync(string name, string newname)
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.EditNameAsync(Token, name, newname);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.EditNameAsync(Token, name, newname);
            }

            return result;
        }

        public static ReturnCode EditDescription(string name, string newdescription)
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.EditDescription(Token, name, newdescription);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.EditDescription(Token, name, newdescription);
            }

            return result;
        }

        public static async Task<ReturnCode> EditDescriptionAsync(string name, string newdescription)
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.EditDescriptionAsync(Token, name, newdescription);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.EditDescriptionAsync(Token, name, newdescription);
            }

            return result;
        }

        public static ReturnCode EditText(string name, string newtext)
        {
            if (!Authorized)
                throw new Exception();

            var result = Instance.EditText(Token, name, newtext);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Authorize(Login, Password).Item2;
                result = Instance.EditText(Token, name, newtext);
            }

            return result;
        }

        public static async Task<ReturnCode> EditTextAsync(string name, string newtext)
        {
            if (!Authorized)
                throw new Exception();

            var result = await Instance.EditTextAsync(Token, name, newtext);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.AuthorizeAsync(Login, Password)).Item2;
                result = await Instance.EditTextAsync(Token, name, newtext);
            }

            return result;
        }
    }
}
