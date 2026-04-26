/// <reference path="./enums/contentEnum.ts" />

namespace mdBusinessLogic.dataAccess.controllers.options.v2 {
    export interface iSortableRequestOptions<T> {
        SortField?: T;
        SortDirection?: sortDirection;
    }

    export enum sortDirection {
        Ascending,
        Descending
    }
}
