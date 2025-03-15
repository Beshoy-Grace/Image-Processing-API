namespace Image.API.DTOs
{
    public enum StatusCodes
    {
        Success = 200,
        BadRequest = 400, // General validation error
        Unauthorized = 401, // Invalid credentials
        Forbidden = 403, // User not allowed to perform operation
        NotFound = 404, // User not found
        InternalServerError = 500, // Server error
        UsernameAlreadyExists = 409, // Conflict - Username already exists
        EmailAlreadyExists = 409, // Conflict - Email already exists
        InvalidUsernameOrPassword = 410, // Conflict - InvalidUsernameOrPassword 
        InvalidTenant = 411,// Invalid Tenant Name
        NoTenantFound = 412,
        YoumustaddEmailorUserName = 413,
        Invalidloginattempt = 414,
        FailedToUpdateUserData = 415,
        FailedToUpdatePassword = 416,
        FailedToSendEmail = 417,
        EmailNotRegistered = 418,
        InvalidMobileFormat = 422, // Unprocessable Entity - Invalid mobile number format
        InvalidEmailFormat = 423, // Unprocessable Entity - Invalid email format
        AccountExpired = 440, // Account expired
        InvalidToken = 498, // Invalid or expired token
        ResourceGone = 410, // Resource requested is no longer available
        RateLimitExceeded = 429, // Rate limit exceeded for the user's IP
        YouDontHavePermission = 430, //you dont have permission
        PasswordMustHaveUppercase = 419, // Passwords must have at least one uppercase
        PasswordMustHaveDigit = 420,// Passwords must have at least one digit
        InvalidUserData = 421,//
        Passwordsmustbeatleast6characters = 424,
        Passwordsmusthaveatleastonenonalphanumericcharacter = 425,
        PasswordMustHaveLowercase = 432,
        PasswordMustHaveNonAlphanumeric = 433,
        PasswordTooShort = 434,
        UserAlreadyHasPassword = 435,
        UserAlreadyInRole = 436,
        UserNotInRole = 437,
        UsernameCannotBeNullOrEmpty = 438,
        EmailCannotBeNullOrEmpty = 439,
        LockoutNotEnabled = 441,
        PhoneNumberAlreadyExists = 442,
        NoDataFound = 443,
        PinCodeExpired = 444,
        Lastupdatedtimeisnull = 445,
        UserIsNotverified = 446,
        AccountLocked = 447,
        FailedToResetPassword = 448,
        PasswordMismatch = 449,
        SamePassword = 450,
        TenantNotActive = 451,
        UserNotActive = 452,
        InvalidFileFormat = 453,
        InvalidFileExceed = 454,
        InvalidSize = 455,

    }
}
