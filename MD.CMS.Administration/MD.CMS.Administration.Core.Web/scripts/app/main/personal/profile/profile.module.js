(function () {
    'use strict';

    angular
        .module('app.personal.profile', [])
        .config(['msNavigationServiceProvider', '$stateProvider', 'msApiProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider, $stateProvider, msApiProvider) {

        // State
        $stateProvider.state('app.profile', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/personal/profile',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/personal/profile/profile.html',
                    controller: 'ProfileController as vm'
                }
            },
            resolve: {
                user: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var id = $stateParams.id;
                    (new mdBusinessLogic.dataAccess.controllers.userController()).getById(
                        mdBusinessLogic.globals.loggedOnUser.Id,
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
