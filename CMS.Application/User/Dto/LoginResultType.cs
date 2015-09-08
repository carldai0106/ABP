namespace CMS.Application.User.Dto
{
    public enum LoginResultType
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,
        InvalidPassword,
        UserIsNotActive,

        InvalidTenancyName,
        TenantIsNotActive,
        UserEmailIsNotConfirmed
    }
}