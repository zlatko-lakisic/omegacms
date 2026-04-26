(function () {
    'use strict';

    angular
        .module('app.taxonomy.forms', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.taxonomy_forms', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/taxonomy/forms/:currentView/:path/:action?/:id',
            params: { taxonomyPath: {} },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/taxonomy/forms/taxonomy-form.html',
                    controller: 'TaxonomyFormController as vm'
                }
            },
            //params: {
            //    taxonomyPath: '',
            //    taxonomyId: 0
            //},
            bodyClass: 'forms',
            resolve: {
                taxonomy: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var taxonomyId = $stateParams.id || 0;
                    if ($stateParams.action !== 'add') {
                        (new mdBusinessLogic.dataAccess.controllers.taxonomyController()).getById(
                        taxonomyId,
                        function (data) {
                            (new mdBusinessLogic.dataAccess.controllers.contentController()).taxonomyContentGetContentByTaxonomy(
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
                    } else {
                        var taxonomy = new mdBusinessLogic.dataAccess.entities.taxonomy();
                        taxonomy.ParentId = taxonomyId;
                        defer.resolve(taxonomy);
                    }
                    return defer.promise;
                }]/*,
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
                }]*/
            }
        });
    }

})();
