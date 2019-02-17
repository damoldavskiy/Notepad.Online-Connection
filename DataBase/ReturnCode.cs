namespace DataBase
{
    public enum ReturnCode
    {
        Success = 0,
        IllegalEmail = 100,
        IllegalPassword = 101,
        IllegalConfirmCode = 102,
        UserAlreadyExists = 200,
        UserDoesntExist = 201,
        ConfirmCodeExpired = 202,
        WrongConfirmCode = 203,
        NoEmailToConfirm = 204,
        TokenDoesntExist = 205,
        TokenExpired = 206,
        TooManyRecoverRequests = 207,
        DataAlreadyExists = 300,
        DataDoesntExist = 301,
        NoConnection = 400,
        Busy = 401
    }
}
