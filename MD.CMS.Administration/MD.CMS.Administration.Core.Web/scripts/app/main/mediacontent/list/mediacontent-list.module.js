(function () {
    'use strict';

    angular
        .module('app.mediacontent.list', [])
        .config(['$stateProvider', '$translatePartialLoaderProvider', 'msApiProvider', 'msNavigationServiceProvider', config]);

    /** @ngInject */
    function config($stateProvider, $translatePartialLoaderProvider, msApiProvider, msNavigationServiceProvider) {
        // State
        $stateProvider.state('app.mediacontent_list', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/mediacontent/list/*folderPath/:currentView/',
          
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/mediacontent/list/mediacontent-list.html',
                    controller: 'MediaContentListController as vm'
                }
            },
            params: {
                folderPath:'Root',
                currentView: 'grid'
            },
            bodyClass: 'file-manager',
            resolve: {
                folder: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var folderConfig = {
                        path: $stateParams.folderPath || 'Root',
                        pageIndex: 0,
                        pageSize: 10,
                        fillContents: false,
                        fillMediaContents: true,
                        searchTerm: ""
                    };
                    var mediaContentConfig = {
                        folderId: 0,
                        lcid: mdBusinessLogic.settings.lcid || 2057,
                        pageIndex: 0,
                        pageSize: 10,
                        sort: "",
                        searchTerm: ""
                    };
                    (new mdBusinessLogic.dataAccess.controllers.folderController()).paginationGetByFolderPath(folderConfig, function (data) {
                      defer.resolve(data);
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });
                    return defer.promise;
                }]
            }
        });
    }
})();
