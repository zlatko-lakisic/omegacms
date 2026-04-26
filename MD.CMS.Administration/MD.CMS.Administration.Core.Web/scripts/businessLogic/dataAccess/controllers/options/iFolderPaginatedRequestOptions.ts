namespace mdBusinessLogic.dataAccess.controllers.options {
    export interface iFolderPaginatedRequestOptions extends iPathPaginatedRequestOptions {
        fillContents?: boolean,
        fillMediaContents?: boolean,
        parentId?: number
    }
}
