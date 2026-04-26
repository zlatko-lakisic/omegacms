(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions.designer', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.report_definitions_designer', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/reporting/designer/:action/:id?',
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/reporting/report-definitions/designer/report-definitions-designer.html',
                    controller: 'ReportinDefinitionsDesignerController as vm'
                }
            },
            params: {
                id: {
                    type: 'string',
                    value: ''
                },
                action: {
                    type: 'string',
                    value: ''
                }
            },
            bodyClass: 'forms',
            resolve: {
                allEntities: ['$q', function ($q) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.reportDefinitionController()).getEntities(function (data) {
                        for (var i = 0; i < data.length; i++) {
                            if (data[i].Icon == null || data[i].Icon == '') {
                                switch (data[i].Type) {
                                    case 2:
                                        data[i].Icon = 'account';
                                        break;
                                    case 3:
                                        data[i].Icon = 'checkbox-marked';
                                        break;
                                    case 4:
                                        data[i].Icon = 'folder-image';
                                        break;
                                    case 5:
                                        data[i].Icon = 'folder';
                                        break;
                                    default:
                                        data[i].Icon = 'file';
                                        break;
                                }
                            }
                        }
                        defer.resolve(data);
                    }, function (error) { });
                    return defer.promise;
                }],
                reportToEdit: ['$q', '$stateParams', function ($q, $stateParams) {
                    var defer = $q.defer();
                    if ($stateParams.action == 'add') {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.reportDefinition());
                    } else {
                        (new mdBusinessLogic.dataAccess.controllers.reportDefinitionController()).getById($stateParams.id, function (data) {
                            defer.resolve(data);
                        }, function (error) { });
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();