using System;

namespace DataBase
{
    public static class ReturnCodeDescriptions
    {
        public static string GetDescription(this ReturnCode code)
        {
            switch (code)
            {
                case ReturnCode.Success:
                    return "Successful operation";
                case ReturnCode.IllegalEmail:
                    return "Email is not correct";
                case ReturnCode.IllegalPassword:
                    return "Password is not correct";
                case ReturnCode.IllegalConfirmCode:
                    return "Confirmation code is not correct";
                case ReturnCode.UserAlreadyExists:
                    return "User with given e-mail already exists";
                case ReturnCode.UserDoesntExist:
                    return "User with given e-mail and password doesn't exists";
                case ReturnCode.ConfirmCodeExpired:
                    return "Confirmation code expired";
                case ReturnCode.WrongConfirmCode:
                    return "Wrong confirmation code";
                case ReturnCode.NoEmailToConfirm:
                    return "User to confirm not found";
                case ReturnCode.TokenDoesntExist:
                    return "Token doesn't exist";
                case ReturnCode.TokenExpired:
                    return "Token expired";
                case ReturnCode.TooManyRecoverRequests:
                    return "Too many recover requests";
                case ReturnCode.DataAlreadyExists:
                    return "Data with given name already exists";
                case ReturnCode.DataDoesntExist:
                    return "Data with given name doesn't exist";
                case ReturnCode.StorageLimitAchieved:
                    return "Storage limit achieved";
                case ReturnCode.NoConnection:
                    return "No connection to server";
                case ReturnCode.Busy:
                    return "Process is busy";
                default:
                    throw new Exception();
            }
        }
    }
}
