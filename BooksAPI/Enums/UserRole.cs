using System.ComponentModel;

public enum UserRole
{
    [Description("Regular user")]
    User,

    [Description("System administrator")]
    Administrator,

    [Description("Content moderator")]
    Moderator
}