using MD.Tools.Helpers.Core.TypeAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum PermissionsEnum
    {
        [StringValue("Type")]
        Type,
        [StringValue("Entity")]
        Entity,
        [StringValue("EntityId")]
        EntityId,
        [StringValue("Object")]
        Object,
        [StringValue("ObjectId")]
        ObjectId,
        [StringValue("AccessType")]
        AccessType
    }

    internal enum PermissionsParameterEnum
    {
        [StringValue("_permissionId")]
        PermissionId,
        [StringValue("_userId")]
        UserId,
        [StringValue("_profileTypeId")]
        ProfileTypeId,
        [StringValue("_controller")]
        Controller,
        [StringValue("_method")]
        Method,
        [StringValue("_function")]
        Function,
        [StringValue("id")]
        Id
    }

    internal enum PermissionsSPEnum
    {

        [StringValue("Permissions_Delete")]
        Delete,
        [StringValue("Permissions_Insert")]
        Insert,
        [StringValue("Permissions_GetById")]
        GetById,
        [StringValue("Permissions_GetByMethod")]
        GetByMethod,
        [StringValue("Permissions_GetByProfileType")]
        GetByProfileType,
        [StringValue("Permissions_SelectAll")]
        SelectAll,
        [StringValue("UserPermissions_Insert")]
        UserPermissionsInsert,
        [StringValue("UserPermissions_Delete")]
        UserPermissionsDelete,
        [StringValue("UserPermissions_GetAll")]
        UserPermissionsGetAll,
        [StringValue("ProfileTypePermissions_Insert")]
        ProfileTypePermissionsInsert,
        [StringValue("ProfileTypePermissions_Delete")]
        ProfileTypePermissionsDelete,
        [StringValue("UserPermissions_GetAssigned")]
        UserPermissionsGetAssigned
    }
}