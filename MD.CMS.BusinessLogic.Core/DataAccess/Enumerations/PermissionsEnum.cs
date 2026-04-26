//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations
{
    public enum PermissionsEnum
    {
        #region ENUM

        Public = 0,

        AttributeTypeDefinitionControllerGetById = 1,
        AttributeTypeDefinitionControllerGetAll = 2,
        ContentControllerGetById = 3,
        ContentControllerGetAll = 4,
        ContentControllerSelectAllCount = 5,
        ContentControllerGetByContentId = 6,
        ContentControllerGetByFolderId = 7,
        ContentControllerGetBySearchTerm = 8,
        ContentControllerSave = 9,
        ContentControllerDelete = 10,
        ContentControllerTranslate = 11,


        ContentTypeDefinitionControllerGetById = 12,
        ContentTypeDefinitionControllerGetAll = 13,
        ContentTypeDefinitionControllerGetByFolder = 14,
        ContentTypeDefinitionControllerSave = 15,
		ContentTypeDefinitionControllerDelete = 16,
		ContentTypeDefinitionFieldControllerGetById = 17,
        ContentTypeDefinitionFieldControllerGetByContentTypeDefinition = 18,

        ContentTypeDefinitionFieldControllerSave = 19,
        ContentTypeDefinitionFieldControllerDelete = 20,
        ContentTypeDefinitionFieldValueControllerGetByContentId = 21,
        ContentTypeDefinitionFieldValueControllerGetByContent = 22,
        ContentTypeDefinitionFieldValueControllerSave = 23,
        ContentTypeDefinitionFolderControllerSave = 24,
        ContentTypeDefinitionFolderControllerDelete = 25,

        CultureControllerGetByLCID = 26,
        CultureControllerGetByCode = 27,


        CultureControllerGetAll = 28,
        CultureControllerGetApproved = 29,
        CultureControllerGetAllForContentId = 30,
        CultureControllerDelete = 31,
        CultureControllerSave = 32,

        FolderControllerGetById = 33,
        FolderControllerGetByParentId = 34,
        FolderControllerGetHierarchyByParentId = 35,
        FolderControllerGetFolderBypath = 36,
        FolderControllerGeUsedFolderMetaDaraField = 37,
        FolderControllerSave = 38,
        FolderControllerDelete = 39,
        FolderControllerGetUsedFolderMediaContentMetaDataField = 40,

        FolderMediaContentMetaDataFieldControllerGetByIds = 41,

        FolderMediaContentMetaDataFieldControllerGetAll = 42,
        FolderMetaDataFieldControllerGetByIds = 43,
        FolderMetaDataFieldControllerGetAll = 44,
        GalleriesControllerGetById = 45,
        GalleriesControllerGetAll = 46,
        GalleriesControllerSave = 47,
        GoogleControllerTranslate = 48,

        LCIDControllerGetById = 49,
        LCIDControllerGetAll = 50,

        MediaContentControllerGetById = 51,
        MediaContentControllerGetAll = 52,
        MediaContentControllerSelectAllCount = 53,
        MediaContentControllerGetByFolderId = 54,
        MediaContentControllerPost = 55,
        MediaContentControllerDelete = 56,
        MediaContentControllerGetByFileType = 159,
        MediaContentControllerSearch = 160,
        MediaContentMetaDataFieldValuesControllerGetByMediaContent = 57,
        MenuContentControllerGetByMenuId = 58,
        MenuContentControllerDelete = 59,
        MenuContentControllerDeletemenu = 60,
        MenuContentControllerPost = 61,
        MenuControllerGetById = 62,
        MenuControllerGetByParentId = 63,
        MenuControllerGetByContent = 64,
        MenuControllerGetAll = 65,
        MenuControllerGetHierarchyByParentidId = 66,
        MenuControllerPost = 67,
        MenuControllerDelete = 68,
        MenuControllerAssignContentToMenu = 69,
        MenuControllerGetMenuByPath = 70,
        MenuControllerMenuSearchByname = 71,

        MetaDataFieldControllerGetAll = 72,
        MetaDataFieldControllerGetById = 73,
        MetaDataFieldControllerGetByFolderid = 74,
        MetaDataFieldControllerGetbyFolder = 75,
        MetaDataFieldControllerPost = 76,
        MetaDataFieldControllerDelete = 77,
        MetaDataFieldValueControllerGetByContentId = 78,
        ProfileControllerAssignProfileTypeToUser = 79,
        ProfileTypeControllergetById = 80,
        ProfileTypeControllerGetAll = 81,
        ProfileTypeControllerGetByUser = 82,
        ProfileTypeControllerPost = 83,

        ProfileTypeControllerDelete = 84,

        ProfileTypeFieldControllerGetByid = 85,
        ProfileTypeFieldControllergetByProfileType = 86,
        ProfileTypeFieldControllerPost = 87,
        ProfileTypeFieldControllerDelete = 88,

        ProfileTypeFieldValueControllerGetByUser = 89,
        ProfileTypeFieldValueControllerPost = 90,
        TaxonomyContentControllerGetBytaxonomyid = 91,
        TaxonomyContentControllerDelete = 92,
        TaxonomyContentControllerDeleteTaxonomy = 93,
        TaxonomyContentControllerPost = 94,
        TaxonomyControllergetById = 95,
        TaxonomyControllerGetByparentId = 96,
        TaxonomyControllerGetByContent = 97,
        TaxonomyControllerGetAll = 98,
        TaxonomyControllerGetHierarchyByParentid = 99,
        TaxonomyControllerTaxonomySerachByName = 100,
        TaxonomyControllerPost = 101,
        TaxonomyControllerAssignContentToTaxonomy = 102,
        TaxonomyControllerGetTaxonomyByPath = 103,
        TaxonomyControllerDelete = 104,
        UploadControllerUploadFile = 105,
        UserControllergetById = 106,
        UserControllerGetAll = 107,
        UserControllerGetByToken = 108,
        UserControllerLogout = 109,
        UserControllerLogin = 110,
        UserControllerDelete = 111,
        UserControllerPost = 112,

        PermissionsControllerGetById = 113,
        PermissionsControllerGetAll = 114,
        PermissionsControllerGetByProfileType = 115,
        PermissionsControllerGetByMethod = 116,
        PermissionsControllerAsignPermission = 117,

        ContentAliasControllerGetAll = 118,
        ContentAliasControllerGetByContent = 119,
        ContentAliasControllerPost = 120,
        ContentAliasControllerGetById = 121,
        ContentAliasControllerDelete = 122,

        ContentControllerSelectByContentTypeDefinitionCount = 123,
        ProfileTypeControllerPostFieldValues = 124,
        ContentControllerPaginationGetByFolderId = 125,
        ContentControllerGetByFolderIdCount = 126,

        FolderControllerGetFolderWithPaginationByPath = 127,
        FolderControllerGetByParentIdCount = 128,

        ProfileTypeControllerGetNotBelonging = 129,
        TaxonomyContentControllerSaveTaxonomyContent = 130,

        UserControllerPaginationGetAll = 131,
        UserControllerGetAllCount = 132,

        TaxonomyControllerGetByParentIdCount = 133,
        TaxonomyControllerGetTaxonomyWithPaginationByPath = 134,
        TaxonomyContentControllerGetByTaxonomyIdCount = 135,
        TaxonomyContentControllerPaginationGetByTaxonomyId = 136,

        MenuControllerGetByParentIdCount = 137,
        MenuControllerPaginationGetMenuByPath = 138,
        ContentTypeDefinitionControlleGetAll = 139,
        ContentTypeDefinitionControlleGetAllCount = 140,
        ContentTypeDefinitionsByFolder = 141,
        ContentTypeDefinitionFolderControllerGetByFolder = 142,
        MetaDataFieldControllerPaginationGetAll = 143,
        MetaDataFieldControllerGetAllCount = 144,
        FolderMediaContentMetaDataFieldControllerGetByFolderid=145,

        TemplateControllerGetById = 146,
        TemplateControllerGetAll = 147,
        TemplateControllerGetByContent = 148,
        TemplateControllerGetByFolder = 149,
        TemplateControllerPost = 150,
        TemplateControllerDelete = 151,




        ContentControllerGetByAll = 152,

        ContentControllerDeleteByAll = 153,
        ContentControllerGetAllVersion = 154,
        ContetAliasControllerGetAllAliasesByContent = 155,
        MetaDataFieldValueControllerGetByContent = 156,

        SystemInfoPerformance = 157,
        SystemInfoGetAllJobs = 159,
        SystemInfoRemoveJob = 160,

        PermissionsControllerGetAllForUser = 158,

        TaxonomyUpdateChildren = 161,
        FolderMediaContentMetaDataFieldControllerGetUsedFolderMediaContentMetaDataField = 162,
        FolderMetaDataFieldControllerGeUsedFolderMetaDaraField = 163,
		ContentTypeDefinitionControlleGetDataStructure = 164
		#endregion
	}
}
