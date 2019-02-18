using System;
using System.Threading.Tasks;

namespace DataBase
{
    public partial class Manager
    {
        public static Tuple<ReturnCode, string[]> GetNames()
        {
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
                throw new Exception();

            var result = await Instance.GetNamesAsync(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.GetNamesAsync(Token);
            }

            return result;
        }

        public static Tuple<ReturnCode, string[]> GetDescriptions()
        {
            if (Status != ManagerStatus.Ready)
                throw new Exception();

            var result = Instance.GetDescriptions(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.GetDescriptions(Token);
            }

            return result;
        }

        public static async Task<Tuple<ReturnCode, string[]>> GetDescriptionsAsync()
        {
            if (Status != ManagerStatus.Ready)
                throw new Exception();

            var result = await Instance.GetDescriptionsAsync(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.GetDescriptionsAsync(Token);
            }

            return result;
        }

        public static Tuple<ReturnCode, string[]> GetText()
        {
            if (Status != ManagerStatus.Ready)
                throw new Exception();

            var result = Instance.GetText(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = Instance.Login(Email, Password).Item2;
                result = Instance.GetText(Token);
            }

            return result;
        }

        public static async Task<Tuple<ReturnCode, string[]>> GetTextAsync()
        {
            if (Status != ManagerStatus.Ready)
                throw new Exception();

            var result = await Instance.GetTextAsync(Token);
            if (result.Item1 == ReturnCode.TokenExpired || result.Item1 == ReturnCode.TokenDoesntExist)
            {
                Token = (await Instance.LoginAsync(Email, Password)).Item2;
                result = await Instance.GetTextAsync(Token);
            }

            return result;
        }

        public static Tuple<ReturnCode, string, string> GetData(string name)
        {
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
            if (Status != ManagerStatus.Ready)
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
