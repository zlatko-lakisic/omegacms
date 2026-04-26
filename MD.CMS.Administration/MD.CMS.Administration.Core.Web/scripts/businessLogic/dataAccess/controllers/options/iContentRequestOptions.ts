namespace mdBusinessLogic.dataAccess.controllers.options {
    export interface iContentRequestOptions {
        ContentIds?: Array<string>;
        LoadAuthor?: boolean;
        LoadFields?: boolean;
        LoadMetaData?: boolean;
        LCID?: number;
        FolderId?: number;
        OnlyPublished?: boolean;
        SearchTerm?: string;
        PageIndex?: number,
        PageSize?: number,
        DataBound?: boolean,
    }
}
