(function () {
    'use strict';

    angular
        .module('app.settings.configuration.user.list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State       
        $stateProvider.state('app.user_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/user/:currentView/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/user/list/user-list.html',
                    controller: 'UserListController as vm'
                }
            },
            params: {
                currentView: 'list'
            },
            bodyClass: 'file-manager',
            resolve: {
                users: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var userConfig = {
                        sort: "",
                        currentPageIndex: 0,
                        maxNumberOfRows: 10,
                        searchTerm: ""
                    };
                    (new mdBusinessLogic.dataAccess.controllers.userController()).paginationGetAll(
                        userConfig,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    return defer.promise;
                }],
                profileTypes: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.profileTypeController()).getAll(
                        "",
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    return defer.promise;
                }]
            }
        });
    }

})();
