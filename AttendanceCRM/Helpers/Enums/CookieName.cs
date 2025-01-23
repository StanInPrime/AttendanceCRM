using System.ComponentModel;

namespace AttendanceCRM.Helpers.Enums
{
    public enum CookieName
    {
        [Description("AEIIsRemember")]
        IsRemember,

        [Description("AEIUserName")]
        UserName,

        [Description("AEIPassword")]
        Password
    }
}
