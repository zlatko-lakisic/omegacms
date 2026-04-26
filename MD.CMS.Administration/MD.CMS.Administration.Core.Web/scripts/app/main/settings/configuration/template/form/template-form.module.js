(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template-form', [])
        .config(['$stateProvider', config]);

    /** @ngInject */
    function config($stateProvider) {
        $stateProvider.state('app.template-form', {
            url: '/' + mdBusinessLogic.globals.selectedLanguage + '/template/:id?',
            params: {
                id: {
                    type: 'string',
                    value: ''
                }
            },
            views: {
                'content@app': {
                    templateUrl: 'scripts/app/main/settings/configuration/template/form/template-form.html',
                    controller: 'TemplateFormController as vm'
                }
            },
            bodyClass: 'forms',
            resolve: {
                template: ['$q', '$stateParams', 'mdFeedbackService', function ($q, $stateParams, $mdFeedbackService) {
                    var defer = $q.defer();
                    var templateId = $stateParams.id;
                    if(templateId){
                        (new mdBusinessLogic.dataAccess.controllers.templateController()).getById(
                        templateId,
                        function (data) {
                            defer.resolve(data);
                        },
                        function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    } else {
                        defer.resolve(new mdBusinessLogic.dataAccess.entities.template());
                    }
                    return defer.promise;
                }]
            }
        });
    }

})();