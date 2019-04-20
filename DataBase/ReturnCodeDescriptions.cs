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
                    return Resource.Success;
                case ReturnCode.IllegalEmail:
                    return Resource.IllegalEmail;
                case ReturnCode.IllegalPassword:
                    return Resource.IllegalPassword;
                case ReturnCode.IllegalConfirmCode:
                    return Resource.IllegalConfirmCode;
                case ReturnCode.UserAlreadyExists:
                    return Resource.UserAlreadyExists;
                case ReturnCode.UserDoesntExist:
                    return Resource.UserDoesntExist;
                case ReturnCode.ConfirmCodeExpired:
                    return Resource.ConfirmCodeExpired;
                case ReturnCode.WrongConfirmCode:
                    return Resource.WrongConfirmCode;
                case ReturnCode.NoEmailToConfirm:
                    return Resource.NoEmailToConfirm;
                case ReturnCode.TokenDoesntExist:
                    return Resource.TokenDoesntExist;
                case ReturnCode.TokenExpired:
                    return Resource.TokenExpired;
                case ReturnCode.TooManyRecoverRequests:
                    return Resource.TooManyRecoverRequests;
                case ReturnCode.DataAlreadyExists:
                    return Resource.DataAlreadyExists;
                case ReturnCode.DataDoesntExist:
                    return Resource.DataDoesntExist;
                case ReturnCode.StorageLimitAchieved:
                    return Resource.StorageLimitAchieved;
                case ReturnCode.NoConnection:
                    return Resource.NoConnection;
                case ReturnCode.Busy:
                    return Resource.Busy;
                default:
                    throw new Exception();
            }
        }
    }
}
