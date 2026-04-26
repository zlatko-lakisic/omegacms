(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template-form')
        .controller('BrowseDialogController', ['$state', '$scope', '$mdSidenav', '$mdDialog', 'TemplateService', TemplateDirectoryController]);

    /** @ngInject */
    function TemplateDirectoryController($state, $scope, $mdSidenav, $mdDialog, TemplateService) {
        var vm = this;

        //variables
        vm.enableBack = false;
        vm.filterText = {};

        //helpers
        vm.service = TemplateService;
        var templateDirectoryController = new mdBusinessLogic.dataAccess.controllers.templateDirectoryController();
        vm.template = new mdBusinessLogic.dataAccess.entities.template();

        //methods
        vm.chooseFolder = chooseFolder;
        vm.chooseFile = chooseFile;
        vm.goBack = goBack;
        vm.cancelDialog = cancelDialog;
        vm.filterFiles = filterFiles;

        function getTemplateDirectoryByPath() {
            templateDirectoryController.getTemplateDirectoryByPath(vm.template, function (data) {
                $scope.$apply(function () {
                    vm.templateData = data;
                });
            }, function (error) { })
        }

        function goBack() {
            vm.template = vm.service.getTemplate();
            vm.template.TemplateUrl = vm.template.TemplateUrl.substring(0, vm.template.TemplateUrl.lastIndexOf("\\"));
            getTemplateDirectoryByPath();
            shouldEnableBack();
        }

        function shouldEnableBack() {
            vm.enableBack = vm.template.TemplateUrl.length > 0;
        }

        function cancelDialog() {
            $mdDialog.cancel();
        }

        function findTemplateUrl(templateData, folderName) {
            for (var i in templateData.Children) {
                if (templateData.Children[i].Name == folderName) {
                    return templateData.Children[i].Path;
                }
            }
        }

        function chooseFolder(folderName) {
            vm.template = vm.service.getTemplate();
            vm.template.TemplateUrl = findTemplateUrl(vm.templateData, folderName);
            getTemplateDirectoryByPath();
            shouldEnableBack();
        }

        function chooseFile(file) {
            vm.template = vm.service.getTemplate();
            vm.template.TemplateUrl +=  "\\" + file;
            vm.service.setTemplate(vm.template);
            vm.service.setTemplateUrl(vm.template.TemplateUrl);
            $mdDialog.hide(vm.template.TemplateUrl);
        }

        function filterFiles() {
            vm.filterText = { Name: vm.searchText };
        }

        //executing
        getTemplateDirectoryByPath();
    }
})();