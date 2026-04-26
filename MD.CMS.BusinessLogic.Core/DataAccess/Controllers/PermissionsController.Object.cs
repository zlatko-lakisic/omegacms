using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MD.Tools.Helpers.Core.Extensions.EnumExt;
using MD.Tools.Helpers.Core.Extensions.DataRow;
using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping;
using MD.CMS.BusinessLogic.Core.DataAccess.Entities.Permissions;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class PermissionsController : BaseController<PermissionsController>
    {
        private class Object : BaseController<Object>
        {
            public List<ObjectPermission> Create(DataTable table)
            {
                List<ObjectPermission> permissions = new List<ObjectPermission>();

                DataTable grouped = table.AsEnumerable().GroupBy(row => string.Format("{0}_{1}", row.GetValue<int>(PermissionsEnum.Object.GetStringValue()), row.GetValue<string>(PermissionsEnum.ObjectId.GetStringValue()))).Select(group => group.First()).CopyToDataTable();
                foreach (DataRow goupRow in grouped.Rows)
                {
                    ObjectPermission obj = new ObjectPermission();

                    if (obj != null)
                    {
                        obj.Object = (Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)goupRow.GetValue<int>(PermissionsEnum.Object.GetStringValue());
                        obj.ObjectId = goupRow.GetValue<string>(PermissionsEnum.ObjectId.GetStringValue(), string.Empty);
                        obj.AccessTypes = new List<PermissionAccessTypeEnum>();

                        foreach (DataRow row in table.AsEnumerable().Where(row =>
                                row.GetValue<int>(PermissionsEnum.Object.GetStringValue()).Equals(obj.Object.GetIntValue()) &&
                                string.Compare(row.GetValue<string>(PermissionsEnum.ObjectId.GetStringValue(), string.Empty), obj.ObjectId, true).Equals(0)
                        ))
                        {
                            obj.AccessTypes.Add((PermissionAccessTypeEnum)row.GetValue<int>(PermissionsEnum.AccessType.GetStringValue()));
                        }

                        permissions.Add(obj);
                    }
                }

                return permissions;
            }
        }
    }
}
