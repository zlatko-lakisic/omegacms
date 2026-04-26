(function () {
    'use strict';

    angular
        .module('app.personal.mailbox', [])
        .config(['msNavigationServiceProvider', '$stateProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider, $stateProvider, msApiProvider) {

        // State
        $stateProvider.state('app.mailbox', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/personal/mailbox',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/personal/mailbox/mailbox.html',
                    controller: 'MailboxController as vm'
                }
            },
            resolve: {
                Icons: ["msApi", function (msApi) {
                    return msApi.resolve('icons@get');
                }],
                systemFolders: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.messageFolderController()).getAllSystemFolders(
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    return defer.promise;
                }],
                userFolders: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.messageFolderController()).getByAuthorId(
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    return defer.promise;
                }]
            },
            bodyClass: 'file-manager'
        });
        // Api for all Icons from fuse
        msApiProvider.register('icons', ['assets/icons/selection.json']);
    }
})();
