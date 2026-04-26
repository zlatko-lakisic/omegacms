(function () {
    'use strict';

    angular
        .module('app.settings.configuration.user.form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.user_form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/user/form/:currentView/:action/:id?',
            params: {
                currentView: {
                    type: 'string',
                    value: 'list'
                },
                action: {
                    type: 'string',
                    value: ''
                },
                id: {
                    type: 'string',
                    value: ''
                }
            },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/user/form/user-form.html',
                    controller: 'UserFormController as vm'
                }
            },
            bodyClass: 'forms',
            resolve: {
                user: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var id = $stateParams.id;
                    var isNew = $stateParams.action == 'add';
                    if (!isNew) {
                        (new mdBusinessLogic.dataAccess.controllers.userController()).getById(
                            id,
                            function (data) {
                                defer.resolve(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    } else {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.user());
                    }
                    return defer.promise;
                }],
                allProfileTypes: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var id = $stateParams.id;
                    var isNew = $stateParams.action == 'add';
                    if (!isNew) {
                        (new mdBusinessLogic.dataAccess.controllers.profileTypeController()).getAll('',
                            function (data) {
                                defer.resolve(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    } else {
                        defer.resolve([]);
                    }
                    return defer.promise;
                }],
                selectedProfileType: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var id = $stateParams.id;
                    var isNew = $stateParams.action == 'add';
                    if (isNew) {
                        (new mdBusinessLogic.dataAccess.controllers.profileTypeController()).getById(id,
                            function (data) {
                                defer.resolve(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    } else {
                        defer.resolve(null);
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();
