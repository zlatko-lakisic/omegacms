/// <reference path="./base/baseController.ts" />
/// <reference path="./base/baseController.options.ts" />
/// <reference path="../entities/contentAlias.ts" />
/// <reference path="../entities/content.ts" />
/// <reference path="../../helpers/mdException.ts" />
/// <reference path="../../helpers/encoder.ts" />

namespace mdBusinessLogic.dataAccess.controllers {
    export class searchController extends base.BaseController<searchController, entities.search.searchResults> {

        constructor() {
            super('Search/');
        }

        public fullTextSearch(searchTerm: string, onSuccess: (obj: entities.search.searchResults) => void, onError: (error: helpers.mdException) => void): void {
            let options: base.AjaxMethodOptions<searchController, entities.search.searchResults> = new base.AjaxMethodOptions<searchController, entities.search.searchResults>();
            options.includeAuthHeader = true;
            options.address = this.getAddress('FullText', [searchTerm]);
            options.responseData = new entities.search.searchResults();
            options.onSuccess = (options: base.AjaxMethodOptions<searchController, entities.search.searchResults>): void => {
                onSuccess(options.responseData);
            }
            options.onError = (options: base.AjaxMethodOptions<searchController, entities.search.searchResults>): void => {
                onError(options.exception);
            }
            this._get(options);
        }
    }
}
