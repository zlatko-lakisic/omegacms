using System.Collections.Generic;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Providers.Authentication
{
    public interface IUser
    {
        string ReferenceId { get; }

        string Username { get; }

        IEnumerable<AuthUserField> MetaDataFieldValues { get; set; }

        IEnumerable<MemberOf> MemberOf { get; set; }
    }
}
