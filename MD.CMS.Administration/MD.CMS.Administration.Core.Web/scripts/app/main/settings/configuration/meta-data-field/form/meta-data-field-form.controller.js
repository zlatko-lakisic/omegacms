(function () {
    'use strict';

    angular
        .module('app.settings.configuration.meta-data-field-form')
        .controller('MetaDataFieldFormController', ['$state', '$rootScope', '$scope', '$mdSidenav', '$mdDialog', 'metaDataField', 'attributeTypes', 'mdFeedbackService', MetaDataFieldFormController]);

    /** @ngInject */
    function MetaDataFieldFormController($state, $rootScope, $scope, $mdSidenav, $mdDialog, metaDataField, attributeTypes, $mdFeedbackService) {
        var vm = this;

        // Controllers
        var metaDataFieldController = new mdBusinessLogic.dataAccess.controllers.metaDataFieldController();
        var attributeTypeDefinitionController = new mdBusinessLogic.dataAccess.controllers.attributeTypeDefinitionController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        // Variables
        vm.metaDataField = metaDataField;
        vm.attributeTypes = attributeTypes;
        vm.ListValueList = vm.metaDataField.ListValue.split(vm.metaDataField.Delimiter);
        vm.oldDelimiter = vm.metaDataField.Delimiter;
        vm.isNew = $state.params.id ? false : true
        vm.addOrEdit = $state.params.id ? 'edit' : 'create';
        vm.formTitle = vm.addOrEdit === 'edit' ? $rootScope.globals.resources.Titles.AddMetaDataField : $rootScope.globals.resources.Titles.EditMetaDataField;
        var dialogTextInfo = vm.addOrEdit === 'edit' ? $rootScope.globals.resources.Labels.EditedText : $rootScope.globals.resources.Labels.AddedText;

        // Methods
        vm.sendForm = sendForm;
        vm.onListModify = onListModify;
        vm.onDelimiterChange = onDelimiterChange;

        function onListModify($chip, $index) {
            var newMultiItemsList = vm.ListValueList.filter(function (item) { return item.indexOf(vm.metaDataField.Delimiter) >= 0; });
            for (var i in newMultiItemsList) {
                if (newMultiItemsList[i].indexOf(vm.metaDataField.Delimiter) >= 0) {
                    var newItemsLit = newMultiItemsList[i].split(vm.metaDataField.Delimiter);
                    for (var j in newItemsLit) {
                        vm.ListValueList.push(newItemsLit[j]);
                    }

                    for (var x = vm.ListValueList.length - 1; x >= 0; x--) {
                        if (vm.ListValueList[x] == newMultiItemsList[i]) {
                            vm.ListValueList.splice(x, 1);
                        }
                    }
                }
            }
            vm.metaDataField.ListValue = vm.ListValueList.join(vm.metaDataField.Delimiter);
        }

        function onDelimiterChange() {
            if (vm.metaDataField.Delimiter !== undefined && vm.metaDataField.Delimiter != '') {
                vm.ListValueList = vm.metaDataField.ListValue.split(vm.oldDelimiter);
                vm.metaDataField.ListValue = vm.ListValueList.join(vm.metaDataField.Delimiter);
                vm.oldDelimiter = vm.metaDataField.Delimiter;
            }
        }

        function sendForm() {
            metaDataFieldController.save(vm.metaDataField, function (data) {
                $mdFeedbackService.reportInfo('save');
                $state.go('app.meta-data-field-list');
            }, function (error) {
                $mdFeedbackService.retportError('save', error);
            });
        } 
    }
})();
