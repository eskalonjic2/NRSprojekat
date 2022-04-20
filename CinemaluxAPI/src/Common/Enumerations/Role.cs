using System.ComponentModel;

namespace CinemaluxAPI.Common.Enumerations
{
    public enum Role : byte
    {
      [Description("Administrator, has identity and employee insert, doesen't have a manager, may or may not have a salary")]
      Administrator = 4,
      [Description("Manager, has identity and employee insert, may have manager, has salary")]
      Manager = 3,
      [Description("Employee, has identity and employee insert, may have a manager, has salary")]
      Employee = 2,
      [Description("Volunteer, has identity and employee insert, may have a manager, has salary as null")]
      Volunteer = 1,
      [Description("User, has identity and user insert, restricted to webiste only ")]
      User = 0
    }
}