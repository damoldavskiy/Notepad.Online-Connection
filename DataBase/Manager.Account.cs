using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataBase
{
    public partial class Manager
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
            if (Status != ManagerStatus.Ready)
                throw new Exception();
            
            if (!IsPasswordValid(newpassword))
                return ReturnCode.IllegalPassword;

            var result = Instance.ChangePassword(Email, Password, newpassword);

            if (result == ReturnCode.Success)
            {
                Password = newpassword;
            }

            return result;
        }

        public static async Task<ReturnCode> ChangePasswordAsync(string newpassword)
        {
            if (Status != ManagerStatus.Ready)
                throw new Exception();

            if (!IsPasswordValid(newpassword))
                return ReturnCode.IllegalPassword;

            var result = await Instance.ChangePasswordAsync(Email, Password, newpassword);

            if (result == ReturnCode.Success)
            {
                Password = newpassword;
            }

            return result;
        }

        public static void Logout()
        {
            if (Status == ManagerStatus.Idle)
                throw new Exception();

            Email = null;
            Password = null;
            Token = null;
            Status = ManagerStatus.Idle;
        }
    }
}
