using MiniTwit.Core.Responses;
using static MiniTwit.Core.Error.ErrorType;

namespace MiniTwit.Core.Error;

public enum ErrorType
{
    INVALID_USER_ID,
    INVALID_USERNAME,
    INVALID_PASSWORD,
    USERNAME_TAKEN,
    USERNAME_MISSING,
    PASSWORD_MISSING,
    EMAIL_MISSING_OR_INVALID,
    UNAUTHORIZED_CREDENTIALS, 
    FOLLOWALREADYEXISTS
}

public static class ErrorTypeExtensions
{
    public static string ErrorMsg(this ErrorType errorType) => errorType switch
    {
        INVALID_USER_ID => "Invalid user id",
        INVALID_USERNAME => "Invalid username",
        INVALID_PASSWORD => "Invalid password",
        USERNAME_TAKEN => "The username is already taken",
        USERNAME_MISSING => "You have to enter a username",
        PASSWORD_MISSING => "You have to enter a password",
        EMAIL_MISSING_OR_INVALID => "You have to enter a valid email address",
        UNAUTHORIZED_CREDENTIALS => "You are not authorized to use this resource!",
        FOLLOWALREADYEXISTS => "You are already following this user",
        _ => "Unknown error"
    };

    public static string ErrorMsg(this ErrorType? errorType) => ErrorMsg((ErrorType) errorType!);

    public static APIError ToAPIError(this ErrorType errorType, HTTPResponse HTTPResponse)
    {
        return new APIError
        {
            Status = (int) HTTPResponse,
            ErrorMsg = errorType.ErrorMsg()
        };
    }

    public static APIError ToAPIError(this ErrorType? errorType, HTTPResponse HTTPResponse) => ToAPIError((ErrorType) errorType!, HTTPResponse);
}
