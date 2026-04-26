using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping
{
    public enum Entities : int
    {
        [StringValue("Content")]
        Content = 1,
        [StringValue("Attribute Type Definition")]
        AttributeTypeDefinition = 2, 
        [StringValue("Content Type Definition")]
        ContentTypeDefinition = 3, 
        [StringValue("Content Type Definition Field")]
        ContentTypeDefinitionField = 4, 
        [StringValue("Content Type Definition Field Value")]
        ContentTypeDefinitionFieldValue = 5, 
        [StringValue("Content Type Definition Folder")]
        ContentTypeDefinitionFolder = 6, 
        [StringValue("Folder")]
        Folder = 7, 
        [StringValue("Folder Media Content Meta Data Field")]
        FolderMediaContentMetaDataField = 8, 
        [StringValue("Folder Meta Data Field")]
        FolderMetaDataField = 9, 
        [StringValue("Media Content Meta Data Field Values")]
        MediaContentMetaDataFieldValues = 10, 
        [StringValue("Media Content")]
        MediaContent = 11, 
        [StringValue("LCID")]
        LCID = 12, 
        [StringValue("Culture")]
        Culture = 13, 
        [StringValue("Menu Content")]
        MenuContent = 14, 
        [StringValue("Content Alias")]
        ContentAlias = 15, 
        [StringValue("Menu")]
        Menu = 16, 
        [StringValue("Meta Data Field")]
        MetaDataField = 17, 
        [StringValue("Meta Data Field Value")]
        MetaDataFieldValue = 18, 
        [StringValue("Permissions")]
        Permissions = 19, 
        [StringValue("Profile")]
        Profile = 20, 
        [StringValue("Profile Type")]
        ProfileType = 21, 
        [StringValue("Profile Type Field")]
        ProfileTypeField = 22, 
        [StringValue("Profile Type Field Value")]
        ProfileTypeFieldValue = 23, 
        [StringValue("Session")]
        Session = 24, 
        [StringValue("Taxonomy Content")]
        TaxonomyContent = 25, 
        [StringValue("Taxonomy")]
        Taxonomy = 26, 
        [StringValue("Template")]
        Template = 27, 
        [StringValue("User")]
        User = 28, 
        [StringValue("RWD Permission")]
        RWDPermission = 29, 
        [StringValue("Report")]
        Report = 30, 
        [StringValue("Report Definition")]
        ReportDefinition = 31, 
        [StringValue("Report Data")]
        ReportData = 32, 
        [StringValue("Report Scheduler")]
        ReportScheduler = 33, 
        [StringValue("Report Scheduler Action")]
        ReportSchedulerAction = 34, 
        [StringValue("Approval Chain")]
        ApprovalChain = 35, 
        [StringValue("Step")]
        Step = 36, 
        [StringValue("Step Action")]
        StepAction = 37, 
        [StringValue("Step User")]
        StepUser = 38, 
        [StringValue("Message Folder")]
        MessageFolder = 39, 
        [StringValue("Message")]
        Message = 40, 
        [StringValue("Approval Chain Approval")]
        ApprovalChainApproval = 41,
		[StringValue("Content Type Data Source")]
		ContentTypeDefinitionDataSource = 42,
		[StringValue("Content Type Data Source Join")]
		ContentTypeDefinitionDataSourceJoin = 44,
        [StringValue("Content Type Definition Folder Data Bound Condition")]
        ContentTypeDefinitionFolderDataBoundCondition = 45,
        [StringValue("Content Type Definition Folder Data Bound Sync")]
        ContentTypeDefinitionFolderDataBoundSync = 46,
        [StringValue("System Info")]
        SystemInfo = 47
    }
}