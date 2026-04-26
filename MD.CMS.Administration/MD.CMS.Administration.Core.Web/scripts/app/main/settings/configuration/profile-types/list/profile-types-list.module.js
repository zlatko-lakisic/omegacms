/// <reference path="../../user/list/user-list.module.js" />
(function () {
    'use strict';

    angular
        .module('app.settings.configuration.profile-types-list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider.state('app.profile-types-list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/profile-types/',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/profile-types/list/profile-types-list.html',
                    controller: 'ProfileTypesListController as vm'
                }
            },
            params: {
                currentView: ''
            },
            resolve: {
                profileTypes: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var profileTypesConfig = {
                        pageIndex: 0,
                        pageSize: 10,
                        sort: "",
                        searchTerm: ""
                    };
                    (new mdBusinessLogic.dataAccess.controllers.profileTypeController()).getAllWithPagination(
                        profileTypesConfig,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    return defer.promise;
                }],
                profileTypesCount: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var profileTypesCountConfig = {
                        searchTerm: ""
                    };
                    (new mdBusinessLogic.dataAccess.controllers.profileTypeController()).getAllCount(
                        profileTypesCountConfig,
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
