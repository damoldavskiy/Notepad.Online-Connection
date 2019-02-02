namespace DataBase
{
    public enum ReturnCode
    {
        Success = 0,
        IllegalLogin = 100,
        IllegalPassword = 101,
        IllegalConfirmCode = 102,
        IllegalDataName = 103,
        IllegalDataDescription = 104,
        UserAlreadyExists = 200,
        UserDoesntExist = 201,
        ConfirmCodeExpired = 202,
        WrongConfirmCode = 203,
        NoLoginInConfirmList = 204,
        TokenDoesntExist = 205,
        TokenExpired = 206,
        DataAlreadyExists = 300,
        DataDoesntExist = 301,
        NoConnection = 400
    }
}
