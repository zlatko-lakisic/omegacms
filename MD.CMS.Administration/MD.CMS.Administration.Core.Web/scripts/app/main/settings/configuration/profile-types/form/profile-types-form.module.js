/// <reference path="../../user/list/user-list.module.js" />
(function () {
    'use strict';

    angular
        .module('app.settings.configuration.profile-types-form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.profile-types-form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/settings/profile-types/:currentView/:id?',
            params: {
                id: {
                    type: 'string',
                    value: ''
                }
            },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/profile-types/form/profile-types-form.html',
                    controller: 'ProfileTypesFormController as vm'
                }
            },
            bodyClass: 'forms',
            resolve: {
                profileType: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var profileTypeId = $stateParams.id;
                    if (profileTypeId) {
                        (new mdBusinessLogic.dataAccess.controllers.profileTypeController()).getById(
                        profileTypeId,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    } else {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.profileType());
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();
