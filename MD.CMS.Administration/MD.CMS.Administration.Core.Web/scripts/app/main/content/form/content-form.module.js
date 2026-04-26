(function ()
{
    'use strict';

    angular
        .module('app.content.form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider)
    {
        $stateProvider.state('app.content_form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/content/form/:currentView/:action/:path/:folderId/?lcid&id&isDataBound&contentTypeId',
            params: {
                action: {
                    type: 'string',
                    value: ''
                },
                id: {
                    type: 'string',
                    value: ''
                },
                lcid: {
                    type: 'int',
                    value: mdBusinessLogic.settings.lcid,
                    squash: true
                },
                isDataBound: {
                    type: 'boolean',
                    value: ''
                },
                path: {
                    type: 'string',
                    value: ''
                },
                folderId: {
                    type: 'string',
                    value: ''
                },
                currentView: {
                    type: 'string',
                    value: ''
                }
            },
            views    : {
                'content@app': {
                    templateUrl: 'scripts/app/main/content/form/content-form.html',
                    controller : 'ContentFormController as vm'
                }
            },
            params: {
                folderPath: '',
                currentView: '',
                isDataBound: 'false'
            },
            bodyClass: 'forms',
            resolve: {
                folder: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var lcid = mdBusinessLogic.settings.lcid || 2057;
                    var folderId = $stateParams.folderId;
                        (new mdBusinessLogic.dataAccess.controllers.folderController()).get({
                            FolderIds: [folderId],
                            CurrentPageIndex: 0,
                            MaxNumberOfRows: 1,
                            Lcid: lcid,
                            FillTemplates: true
                        }, function (data) {
                            defer.resolve(data.Items[0]);
                        }, function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                        return defer.promise;
                    }],
                content: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var contentId = $stateParams.id;
                    var contentTypeId = $stateParams.contentTypeId ? $stateParams.contentTypeId : 0;
                    var isDataBound = $stateParams.isDataBound ? $stateParams.isDataBound : false;
                    var lcid = mdBusinessLogic.settings.lcid || 2057;
                    var folderId = $stateParams.folderId;
                    if ($stateParams.action == "add") {
                        var newContent = new mdBusinessLogic.dataAccess.entities.content();
                        newContent.FolderId = folderId;
                        newContent.ContentTypeDefinitionId = contentTypeId;
                        defer.resolve(newContent);
                    } else {
                        if (contentId) {
                            (new mdBusinessLogic.dataAccess.controllers.contentController()).get({
                                ContentIds: [contentId],
                                LoadAuthor: false,
                                Lcid: lcid,
                                FillFields: true,
                                DataBound: isDataBound,
                                ContentTypeId: contentTypeId
                            },
                            function (data) {
                                defer.resolve(data.Items[0]);
                            },
                            function (error) {
                                $mdFeedbackService.reportError('load', error);
                            });
                        } else {
                            var newContent = new mdBusinessLogic.dataAccess.entities.content();
                            newContent.FolderId = folderId;
                            defer.resolve(newContent);
                        }
                    }
                    return defer.promise;
                }],
                taxonomies: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var contentId = $stateParams.id;
                    var contentTypeId = $stateParams.contentTypeId ? $stateParams.contentTypeId : 0;
                    var isDataBound = $stateParams.isDataBound ? $stateParams.isDataBound : false;
                    var lcid = mdBusinessLogic.settings.lcid || 2057;
                    if ($stateParams.action == "add") {
                        defer.resolve([]);
                    } else {
                        if (contentId) {
                            (new mdBusinessLogic.dataAccess.controllers.taxonomyController()).getByContent(
                                contentId,
                                function (data) {
                                    defer.resolve(data);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                        } else {
                            defer.resolve([]);
                        }
                    }
                    return defer.promise;
                }],
                contentTypeDefinition: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var contentId = $stateParams.id;
                    var contentTypeId = $stateParams.contentTypeId ? $stateParams.contentTypeId : 0;
                    var isDataBound = $stateParams.isDataBound ? $stateParams.isDataBound : false;
                    var lcid = mdBusinessLogic.settings.lcid || 2057;
                    if ($stateParams.action == "add") {
                        if (contentId === undefined && contentTypeId) {
                            (new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController()).getById(
                                contentTypeId,
                                function (data) {
                                    defer.resolve(data);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                        } else {
                            defer.resolve(null);
                        }
                    } else {
                        defer.resolve(null);
                    }
                    return defer.promise;
                }],
                folderMetaData: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var contentId = $stateParams.id;
                    var folderId = $stateParams.folderId;
                    if ($stateParams.action == "add") {
                        if (contentId === undefined && folderId) {
                            (new mdBusinessLogic.dataAccess.controllers.metaDataFieldController()).getByFolderId(
                                folderId,
                                function (data) {
                                    defer.resolve(data);
                                },
                                function (error) {
                                    $mdFeedbackService.reportError('load', error);
                                });
                        } else {
                            defer.resolve([]);
                        }
                    } else {
                        defer.resolve([]);
                    }
                    return defer.promise;
                }]
            }
        });
    }
})();
