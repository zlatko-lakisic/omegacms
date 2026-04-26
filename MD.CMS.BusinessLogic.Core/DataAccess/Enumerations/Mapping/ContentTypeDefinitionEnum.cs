using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentTypeDefinitionFolderDataBoundSyncEnum
    {
        [StringValue("_FolderId")]
        FolderId,
        [StringValue("_ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("_StartDate")]
        StartDate,
        [StringValue("_EndDate")]
        EndDate,
        [StringValue("_Frequency")]
        Frequency,
        [StringValue("_Enabled")]
        Enabled,
        [StringValue("_SyncType")]
        SyncType,
        [StringValue("_DeltaFieldId")]
        DeltaFieldId
    }
    internal enum ContentTypeDefinitionFolderDataBoundSyncParamatersEnum
    {
        [StringValue("FolderId")]
        FolderId,
        [StringValue("ContentTypeDefinitionId")]
        ContentTypeDefinitionId,
        [StringValue("StartDate")]
        StartDate,
        [StringValue("EndDate")]
        EndDate,
        [StringValue("Frequency")]
        Frequency,
        [StringValue("Enabled")]
        Enabled,
        [StringValue("SyncType")]
        SyncType,
        [StringValue("DeltaFieldId")]
        DeltaFieldId
    }
}