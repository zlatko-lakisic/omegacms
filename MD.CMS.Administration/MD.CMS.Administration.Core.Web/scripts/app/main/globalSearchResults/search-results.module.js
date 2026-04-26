(function () {
    'use strict';

    angular
        .module('app.searchResults.module', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider) {
        // State
        $stateProvider.state('app.search_results', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/searchResults/:searchTerm/',

            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/globalSearchResults/global-search-results.html',
                    controller: 'SearchResultsController as vm'
                }
            },           
            bodyClass: 'file-manager',
            params: {
                searchTerm: ''
            },
            resolve: {
                searchResults: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    if ($stateParams.searchTerm != '') {
                        (new mdBusinessLogic.dataAccess.controllers.searchController()).fullTextSearch($stateParams.searchTerm, function (data) {
                            defer.resolve(data);
                        }, function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    } else {
                        defer.resolve({});
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();