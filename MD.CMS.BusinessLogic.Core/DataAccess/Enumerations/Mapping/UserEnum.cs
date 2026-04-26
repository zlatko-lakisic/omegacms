using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum UserEnum
    {
        [StringValue("UserId")]
        UserId,
        [StringValue("Username")]
        Username,
        [StringValue("Password")]
        Password

    }

    internal enum UserParametersEnum
    {
        [StringValue("_UserId")]
        UserId,
        [StringValue("_Username")]
        Username,
        [StringValue("_Password")]
        Password

    }


    internal enum UserSPEnum
    {
        [StringValue("Users_Select")]
        Select,
        [StringValue("Users_Delete")]
        Delete,
        [StringValue("Users_Insert")]
        Insert,
        [StringValue("Users_Update")]
        Update,
        [StringValue("Users_SelectByUsernameAndPassword")]
        SelectByUsernameAndPassword
    }
}