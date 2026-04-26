(function () {
    'use strict';

    angular
        .module('app.folder.forms', ['app.approval.chain'])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.folder_forms', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/folder/forms/:currentView/:path/:action?/:id/:folderId?',
            params: {
                folderId: {
                    type: 'string',
                    value: ''
                }
            },
            params: { folderPath: {} },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/content/folderForm/folder-form.html',
                    controller: 'FolderFormController as vm'
                }
            },
            bodyClass: 'forms',
            resolve: {
                folder: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var folderId = $stateParams.folderId || 0;
                    var parentId = $stateParams.id || 0;
                    if (folderId) {
                        (new mdBusinessLogic.dataAccess.controllers.folderController()).getById(
                            folderId,
                            function (data) {
                                defer.resolve(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    } else {
                        (new mdBusinessLogic.dataAccess.controllers.folderController()).getById(
                            parentId,
                            function (data) {
                                defer.resolve(data);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();
