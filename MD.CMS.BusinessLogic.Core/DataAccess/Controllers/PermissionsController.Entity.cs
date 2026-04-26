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
        private class Entity : BaseController<Entity>
        {
            public List<EntityPermission> Create(Tools.BaseDataAccess.Plugins.Core.Mapping.Entities entity, DataTable table)
            {
                List<EntityPermission> permissions = new List<EntityPermission>();

                DataTable grouped = table.AsEnumerable().GroupBy(row => row.GetValue<int>(PermissionsEnum.Object.GetStringValue())).Select(group => group.First()).CopyToDataTable();
                foreach (DataRow goupRow in grouped.Rows)
                {
                    EntityPermission obj = new EntityPermission();

                    if (obj != null)
                    {
                        obj.Object = entity;
                        obj.Entity = (Tools.BaseDataAccess.Plugins.Core.Mapping.Entities)goupRow.GetValue<int>(PermissionsEnum.Object.GetStringValue());
                        obj.AccessTypes = new HashSet<PermissionAccessTypeEnum>();

                        DataTable accessTypesTable = table.AsEnumerable().Where(row => row.GetValue<int>(PermissionsEnum.Entity.GetStringValue()) == obj.Object.GetIntValue()).CopyToDataTable();

                        foreach(DataRow accessTypeRow in accessTypesTable.Rows)
                        {
                            obj.AccessTypes.Add((PermissionAccessTypeEnum)accessTypeRow.GetValue<int>(PermissionsEnum.AccessType.GetStringValue()));
                        }

                        permissions.Add(obj);
                    }
                }

                return permissions;
            }
        }
    }
}
