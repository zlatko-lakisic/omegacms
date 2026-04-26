(function () {
    'use strict';

    angular
        .module('app.mediacontent.form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.mediacontent_form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/mediacontent/form/:currentView/:path/:action?/:folderId/:id?/:fileType',
            params: { folderPath: {} },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/mediacontent/form/mediacontent-form.html',
                    controller: 'MediaContentFormController as vm'
                }
            },
            bodyClass: 'forms',
            resolve: {
                mediaContent: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var contentId = $stateParams.id || 0;
                    var folderId = $stateParams.folderId || 0;
                    var lcid = mdBusinessLogic.settings.lcid || 2057;
                    if (contentId) {
                        (new mdBusinessLogic.dataAccess.controllers.mediaContentController()).getByIdWithMetaData(
                        contentId, lcid,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    } else {
                        var mediaContent = new mdBusinessLogic.dataAccess.entities.mediaContent();
                        mediaContent.FolderId = folderId;
                        defer.resolve(mediaContent);
                    }
                    return defer.promise;
                }],
                metaDataFields: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var folderId = $stateParams.folderId || 0;
                    var isNew = $stateParams.action != 'edit';
                    (new mdBusinessLogic.dataAccess.controllers.metaDataFieldController()).metadatagetByFolder(
                        folderId,
                        function (data) {
                            defer.resolve(data.data);
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
