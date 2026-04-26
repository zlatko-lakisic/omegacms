using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations
{
    public enum RWDPermissionTarget : int
    {
        None = 0,
        Folder = 1,
        Content = 2,
        MediaContent = 3
    }
}
