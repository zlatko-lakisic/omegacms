(function () {
    'use strict';

    angular
        .module('app.menu.list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State       
        $stateProvider.state('app.menu_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/menu/list/*menuPath/:currentView',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/menu/list/menu-list.html',
                    controller: 'MenuListController as vm'
                }
            },
            params: {
                menuPath:"Root",
                currentView: ''
            },
            bodyClass: 'file-manager',
            resolve: {
                menu: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var menuConfig = {
                        path: $stateParams.menuPath || 'Root',
                        pageIndex: 0,
                        pageSize: 10,
                        searchTerm: ""
                    };
                    (new mdBusinessLogic.dataAccess.controllers.menuController()).paginationGetMenuByPath(menuConfig, function (data) {
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
