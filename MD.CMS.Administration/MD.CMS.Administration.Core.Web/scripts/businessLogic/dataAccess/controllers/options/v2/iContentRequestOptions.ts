/// <reference path="./enums/contentEnum.ts" />

namespace mdBusinessLogic.dataAccess.controllers.options.v2 {
    export interface iContentRequestOptions extends iSortableRequestOptions<enums.contentEnum>, iPageableRequestOptions, iSearchableRequestOptions {
        ContentIds?: Array<string>;
        OnlyPublished?: boolean;
        LoadAuthor?: boolean;
        FillFields?: boolean;
        FillMetaData?: boolean;
        Lcid?: number;
        FolderId?: number;
        TaxonomyId?: number;
        MenuId?: number;
        Alias?: string;
        DataBound?: boolean;
        ContentTypeId?: number;
    }
}
