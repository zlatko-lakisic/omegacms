namespace mdBusinessLogic.dataAccess.entities {
    export var isFunction = (functionToCheck: any): boolean => {
        var getType = {};
        return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
    }
    export var isArray = (obj: any): boolean => {
        return (!!obj) && (obj.constructor === Array);
    }
    export var isObject = (obj: any): boolean => {
        return (!!obj) && (obj.constructor === Object);
    }

    export enum entitiesEnum {
        Content = 1,
        AttributeTypeDefinition = 2,
        ContentTypeDefinition = 3,
        ContentTypeDefinitionField = 4,
        ContentTypeDefinitionFieldValue = 5,
        ContentTypeDefinitionFolder = 6,
        Folder = 7,
        FolderMediaContentMetaDataField = 8,
        FolderMetaDataField = 9,
        MediaContentMetaDataFieldValues = 10,
        MediaContent = 11,
        LCID = 12,
        Culture = 13,
        MenuContent = 14,
        ContentAlias = 15,
        Menu = 16,
        MetaDataField = 17,
        MetaDataFieldValue = 18,
        Permissions = 19,
        Profile = 20,
        ProfileType = 21,
        ProfileTypeField = 22,
        ProfileTypeFieldValue = 23,
        Session = 24,
        TaxonomyContent = 25,
        Taxonomy = 26,
        Template = 27,
        User = 28,
        RWDPermission = 29,
        Report = 30,
        ReportDefinition = 31,
        ReportData = 32,
        ReportScheduler = 33,
        ReportSchedulerAction = 34,
        ApprovalChain = 35,
        Step = 36,
        StepAction = 37,
        StepUser = 38,
        MessageFolder = 39,
        Message = 40,
        ApprovalChainApproval = 41,
        ContentTypeDefinitionDataSource = 42,
        ContentTypeDefinitionDataSourceJoin = 44,
        ContentTypeDefinitionFolderDataBoundCondition = 45,
        ContentTypeDefinitionFolderDataBoundSync = 46
    }
}
