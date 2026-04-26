(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template-form')
        .controller('TemplateFormController', ['$state', '$rootScope', '$scope', '$mdSidenav', '$mdDialog', '$mdMedia', 'TemplateService', 'template', 'mdFeedbackService', TemplateFormController]);

    /** @ngInject */
    function TemplateFormController($state, $rootScope, $scope, $mdSidenav, $mdDialog, $mdMedia, TemplateService, template, $mdFeedbackService) {
        var vm = this;

        // Controllers
        var templateController = new mdBusinessLogic.dataAccess.controllers.templateController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);
        vm.service = TemplateService;

        // Variables
        vm.templateData = {};
        vm.template = template;
        vm.id = $state.params.id;
        vm.action = vm.id ? 'edit' : 'create'
        vm.formTitle = vm.id ? $rootScope.globals.resources.Titles.EditTemplate : $rootScope.globals.resources.Titles.AddTemplate;
        var dialogTextInfo = vm.action === 'edit' ? $rootScope.globals.resources.Labels.EditedText : $rootScope.globals.resources.Labels.AddedText;

        // Methods
        vm.sendForm = sendForm;
        vm.chooseTemplate = chooseTemplate;

        function sendForm() {
            templateController.save(vm.template,
                function (data) {
                    $mdFeedbackService.reportInfo('save');
                    $state.go('app.template-list');
                }, function (error) {
                    $mdFeedbackService.reportError('save', error);
                });
        }

        function chooseTemplate(event) {
            vm.service.setTemplate(vm.template);
            $mdDialog.show({
                controller: 'BrowseDialogController',
                templateUrl: 'scripts/app/main/settings/configuration/template/browse-dialog/browse-dialog.template.html',
                parent: angular.element(document.body),
                targetEvent: event,
                fullscreen: $mdMedia('sm') || $mdMedia('xs'),
                clickOutsideToClose: true
            })
                .then(function (data) {
                    vm.template.TemplateUrl = data;
                }, function () {
                });
        }

        var updateTemplate = function () {
            vm.template = vm.service.template;
        };

        //executing
        if (vm.id) {
            vm.service.setTemplate(vm.template);
        }

        vm.service.registerObserverCallback(updateTemplate);
    }
})();
