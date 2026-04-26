(function () {
    'use strict';

    angular
        .module('app.content.list', [])
        .config(['$stateProvider', 'mdPermissionAuthenticateProvider', config]);

    /** @ngInject */
    function config($stateProvider, mdPermissionAuthenticateProvider) {
        // State
        $stateProvider.state('app.content_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/content/list/*folderPath',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/content/list/content-list.html',
                    controller: 'ContentListController as vm'
                }
            },
            params: {
                folderPath:"Root",
                currentView: 'grid'               
            },
            bodyClass: 'contnet-list',
            resolve: {
                folder: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var lcid = mdBusinessLogic.settings.lcid || 2057;
                    var folderConfig = {
                        Paths: [$stateParams.folderPath || 'Root'],
                        FillParent: true,
                        FillContentTypeDefinitions: true,
                        FillContents: true,
                        FillChildren: true,
                        CurrentPageIndex: 0,
                        MaxNumberOfRows: 1,
                        Lcid: lcid,
                        ChildFolderRequestOptions: {
                            FillParent: false,
                            FillContentTypeDefinitions: false,
                            FillContents: false,
                            FillChildren: false,
                            CurrentPageIndex: 0,
                            MaxNumberOfRows: 10,
                            Lcid: lcid,
                        },
                        ParentFolderRequestOptions: {
                            FillParent: false,
                            FillContentTypeDefinitions: false,
                            FillContents: false,
                            FillChildren: false,
                            CurrentPageIndex: 0,
                            MaxNumberOfRows: 1,
                            Lcid: lcid,
                        },
                        ContentRequestOptions: {
                            LoadAuthor: true,
                            Lcid: lcid,
                            FillFields: true
                        }
                    };
                    (new mdBusinessLogic.dataAccess.controllers.folderController()).get(folderConfig, function (data) {
                        defer.resolve(data.Items[0]);
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });

                    return defer.promise;
                }],
                onEnter: mdPermissionAuthenticateProvider.onStateEnter(['Content'])
            }
        });
    }

})();
