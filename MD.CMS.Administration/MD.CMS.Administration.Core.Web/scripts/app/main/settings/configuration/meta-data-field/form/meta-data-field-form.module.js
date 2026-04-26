(function ()
{
    'use strict';

    angular
        .module('app.settings.configuration.meta-data-field-form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider)
    {
        $stateProvider.state('app.meta-data-field-form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/meta-data-field-form/:id?',
            views    : {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/meta-data-field/form/meta-data-field-form.html',
                    controller : 'MetaDataFieldFormController as vm'
                }
            },
            params: {
                id: {
                    type: 'string',
                    value: ''
                }
            },
            bodyClass: 'forms',
            resolve: {
                metaDataField: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var metaDataFieldId = $stateParams.id;
                    if (metaDataFieldId) {
                        (new mdBusinessLogic.dataAccess.controllers.metaDataFieldController()).getById(
                        metaDataFieldId,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    } else {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.metaDataField());
                    }
                    return defer.promise;
                }],
                attributeTypes: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    (new mdBusinessLogic.dataAccess.controllers.attributeTypeDefinitionController()).getAll(
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