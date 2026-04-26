(function () {
    'use strict';

    angular
        .module('app.taxonomy.list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State       
        $stateProvider.state('app.taxonomy_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/taxonomy/list/*taxonomyPath/:currentView',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/taxonomy/list/taxonomy-list.html',
                    controller: 'TaxonomyListController as vm'
                }
            },
            params: {
                taxonomyPath:"Root",
                currentView: 'grid'
            },
            bodyClass: 'file-manager',
            resolve: {
                taxonomy: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var taxonomyConfig = {
                        path: $stateParams.taxonomyPath || 'Root',
                        pageIndex: 0,
                        pageSize: 10,
                        searchTerm: ""
                    };
                    (new mdBusinessLogic.dataAccess.controllers.taxonomyController()).paginationGetTaxonomyByPath(taxonomyConfig, function (data) {
                      defer.resolve(data);
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });
                    return defer.promise;
                }]
            }
        });
    }

})();
