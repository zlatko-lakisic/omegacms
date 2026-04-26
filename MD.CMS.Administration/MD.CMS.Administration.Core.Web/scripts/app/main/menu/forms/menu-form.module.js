(function () {
    'use strict';

    angular
        .module('app.menu.forms', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.menu_forms', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/menu/forms/:currentView/:path/:action?/:id/:menuId?',
            params: { menuPath: {} },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/menu/forms/menu-form.html',
                    controller: 'MenuFormController as vm'
                }
            },
            params: {
                menuId: {
                    type: 'string',
                    value: ''
                }
            },
            bodyClass: 'forms',
            resolve: {
                menu: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var menuId = $stateParams.menuId || 0;
                    if (menuId == 0) {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.menu());
                    } else {
                        (new mdBusinessLogic.dataAccess.controllers.menuController()).getById(
                            menuId,
                            function (data) {
                                (new mdBusinessLogic.dataAccess.controllers.contentController()).menuContentGetContentByMenu(
                                    data.Id,
                                    function (data2) {
                                        data.Contents = data2;
                                        defer.resolve(data);
                                    }, function (error) {
                                        $mdFeedbackService.reportError('load', error);
                                    });
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    }
                    return defer.promise;
                }],
                contents: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.contentController()).getAll(
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