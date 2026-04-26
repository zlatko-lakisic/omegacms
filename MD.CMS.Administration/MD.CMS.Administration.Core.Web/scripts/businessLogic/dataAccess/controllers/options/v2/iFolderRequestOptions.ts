/// <reference path="./enums/folderEnum.ts" />

namespace mdBusinessLogic.dataAccess.controllers.options.v2 {
    export interface iFolderRequestOptions extends iSortableRequestOptions<enums.folderEnum>, iPageableRequestOptions, iSearchableRequestOptions {
        FolderIds?: Array<number>;
        Paths?: Array<string>;
        FillParent?: boolean;
        FillAllParents?: boolean;
        FillContentTypeDefinitions?: boolean;
        Depth?: number;
        FillContents?: boolean;
        FillChildren?: boolean;
        ChildFolderRequestOptions?: iFolderRequestOptions;
        ParentFolderRequestOptions?: iFolderRequestOptions;
        ContentRequestOptions?: iContentRequestOptions;
        ParentId?: number;
        OnlyPublished?: boolean;
        Lcid?: number;
        FillTemplates?: boolean;
    }
}
