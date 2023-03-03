using MiniTwit.Core.Responses;
using static MiniTwit.Core.Error.ErrorType;

namespace MiniTwit.Core.Error;

public enum ErrorType
{
    INVALID_USER_ID,
    INVALID_USERNAME,
    INVALID_PASSWORD,
    USERNAME_TAKEN
}

public static class ErrorTypeExtensions
{
    public static string ErrorMsg(this ErrorType? errorType) => errorType switch
    {
        INVALID_USER_ID => "Invalid user id",
        INVALID_USERNAME => "Invalid username",
        INVALID_PASSWORD => "Invalid password",
        USERNAME_TAKEN => "Username is already taken",
        _ => "Unknown error"
    };

    public static APIError ToAPIError(this ErrorType? errorType, HTTPResponse HTTPResponse)
    {
        return new APIError
        {
            Status = (int) HTTPResponse,
            ErrorMsg = errorType.ErrorMsg()
        };
    }
}
