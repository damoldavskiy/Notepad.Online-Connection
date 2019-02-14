using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBase
{
    public class Manager
    {
        public static string Email { get; private set; }
        public static string Password { get; private set; }
        public static string Token { get; private set; }
        public static ManagerStatus Status { get; private set; }

        static bool IsEmailValid(string email)
        {
            return Regex.IsMatch(email, @"\S+@\S+\.\S\S+", RegexOptions.IgnoreCase) && email.Length <= 40;
        }

        static bool IsPasswordValid(string password)
        {
            return password.Length >= 4 && password.Length <= 40;
        }

        static bool IsConfirmCodeValid(string code)
        {
            return code.Length == 4 && int.TryParse(code, out int num);
        }

        public static ReturnCode Register(string email, string password)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = Instance.Register(email, password);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.RegistrationConfirmation;
                Email = email;
                Password = password;
            }

            return result;
        }

        public static async Task<ReturnCode> RegisterAsync(string email, string password)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = await Instance.RegisterAsync(email, password);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.RegistrationConfirmation;
                Email = email;
                Password = password;
            }

            return result;
        }

        public static ReturnCode ConfirmRegistration(string code)
        {
            if (Status != ManagerStatus.RegistrationConfirmation)
                throw new Exception();

            if (!IsConfirmCodeValid(code))
                return ReturnCode.IllegalConfirmCode;

            var result = Instance.ConfirmRegistration(Email, code);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.Idle;
                Login(Email, Password);
            }

            return result;
        }

        public static async Task<ReturnCode> ConfirmRegistrationAsync(string code)
        {
            if (Status != ManagerStatus.RegistrationConfirmation)
                throw new Exception();

            if (!IsConfirmCodeValid(code))
                return ReturnCode.IllegalConfirmCode;

            var result = await Instance.ConfirmRegistrationAsync(Email, code);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.Idle;
                await LoginAsync(Email, Password);
            }

            return result;
        }

        public static ReturnCode Recovery(string email, string newpassword)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(newpassword))
                return ReturnCode.IllegalPassword;

            var result = Instance.Recovery(email);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.RecoveryConfirmation;
                Email = email;
                Password = newpassword;
            }

            return result;
        }

        public static async Task<ReturnCode> RecoveryAsync(string email, string newpassword)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(newpassword))
                return ReturnCode.IllegalPassword;

            var result = await Instance.RecoveryAsync(email);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.RecoveryConfirmation;
                Email = email;
                Password = newpassword;
            }

            return result;
        }

        public static ReturnCode ConfirmRecovery(string code)
        {
            if (Status != ManagerStatus.RecoveryConfirmation)
                throw new Exception();

            if (!IsConfirmCodeValid(code))
                return ReturnCode.IllegalConfirmCode;

            var result = Instance.ConfirmRecovery(Email, Password, code);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.Idle;
                Login(Email, Password);
            }

            return result;
        }

        public static async Task<ReturnCode> ConfirmRecoveryAsync(string code)
        {
            if (Status != ManagerStatus.RecoveryConfirmation)
                throw new Exception();

            if (!IsConfirmCodeValid(code))
                return ReturnCode.IllegalConfirmCode;

            var result = await Instance.ConfirmRecoveryAsync(Email, Password, code);

            if (result == ReturnCode.Success)
            {
                Status = ManagerStatus.Idle;
                await LoginAsync(Email, Password);
            }

            return result;
        }

        public static ReturnCode Login(string email, string password)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = Instance.Login(email, password);

            if (result.Item1 == ReturnCode.Success)
            {
                Status = ManagerStatus.Ready;
                Email = email;
                Password = password;
                Token = result.Item2;
            }

            return result.Item1;
        }

        public static async Task<ReturnCode> LoginAsync(string email, string password)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = await Instance.LoginAsync(email, password);

            if (result.Item1 == ReturnCode.Success)
            {
                Status = ManagerStatus.Ready;
                Email = email;
                Password = password;
                Token = result.Item2;
            }

            return result.Item1;
        }

        public static ReturnCode Login(string email, string password, string token)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = Instance.GetNames(token);

            if (result.Item1 == ReturnCode.Success)
            {
                Status = ManagerStatus.Ready;
                Email = email;
                Password = password;
                Token = token;
            }
            else if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
                return Login(email, password);

            return result.Item1;
        }

        public static async Task<ReturnCode> LoginAsync(string email, string password, string token)
        {
            if (!IsEmailValid(email))
                return ReturnCode.IllegalEmail;
            if (!IsPasswordValid(password))
                return ReturnCode.IllegalPassword;

            var result = await Instance.GetNamesAsync(token);

            if (result.Item1 == ReturnCode.Success)
            {
                Status = ManagerStatus.Ready;
                Email = email;
                Password = password;
                Token = token;
            }
            else if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
                return await LoginAsync(email, password);

            return result.Item1;
        }

        public static ReturnCode ChangePassword(string newpassword)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            return Instance.ChangePassword(Email, Password, newpassword);
        }

        public static async Task<ReturnCode> ChangePasswordAsync(string newpassword)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            return await Instance.ChangePasswordAsync(Email, Password, newpassword);
        }

        public static Tuple<ReturnCode, string[]> GetNames()
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.GetNames(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.GetNames(Token);
            }

            return result;
        }

        public static async Task<Tuple<ReturnCode, string[]>> GetNamesAsync()
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.GetNamesAsync(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.GetNamesAsync(Token);
            }

            return result;
        }

        public static Tuple<ReturnCode, string, string> GetData(string name)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.GetData(Token, name);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.GetData(Token, name);
            }

            return result;
        }

        public static async Task<Tuple<ReturnCode, string, string>> GetDataAsync(string name)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.GetDataAsync(Token, name);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.GetDataAsync(Token, name);
            }

            return result;
        }

        public static ReturnCode AddData(string name, string description, string text)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.AddData(Token, name, description, text);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.AddData(Token, name, description, text);
            }

            return result;
        }

        public static async Task<ReturnCode> AddDataAsync(string name, string description, string text)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.AddDataAsync(Token, name, description, text);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.AddDataAsync(Token, name, description, text);
            }

            return result;
        }

        public static ReturnCode DelData(string name)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.DelData(Token, name);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.DelData(Token, name);
            }

            return result;
        }

        public static async Task<ReturnCode> DelDataAsync(string name)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.DelDataAsync(Token, name);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.DelDataAsync(Token, name);
            }

            return result;
        }

        public static ReturnCode EditName(string name, string newname)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.EditName(Token, name, newname);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.EditName(Token, name, newname);
            }

            return result;
        }

        public static async Task<ReturnCode> EditNameAsync(string name, string newname)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.EditNameAsync(Token, name, newname);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.EditNameAsync(Token, name, newname);
            }

            return result;
        }

        public static ReturnCode EditDescription(string name, string newdescription)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.EditDescription(Token, name, newdescription);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.EditDescription(Token, name, newdescription);
            }

            return result;
        }

        public static async Task<ReturnCode> EditDescriptionAsync(string name, string newdescription)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.EditDescriptionAsync(Token, name, newdescription);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.EditDescriptionAsync(Token, name, newdescription);
            }

            return result;
        }

        public static ReturnCode EditText(string name, string newtext)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = Instance.EditText(Token, name, newtext);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.EditText(Token, name, newtext);
            }

            return result;
        }

        public static async Task<ReturnCode> EditTextAsync(string name, string newtext)
        {
            if (!(Status == ManagerStatus.Ready))
                throw new Exception();

            var result = await Instance.EditTextAsync(Token, name, newtext);
            if (result == ReturnCode.TokenExpired || result == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.EditTextAsync(Token, name, newtext);
            }

            return result;
        }
    }
}
