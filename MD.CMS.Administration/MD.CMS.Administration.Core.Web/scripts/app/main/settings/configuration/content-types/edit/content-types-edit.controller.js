(function () {
    'use strict';

    angular
        .module('app.settings.configuration.content-types-edit')
        .controller('ContentTypeEditController', ['$scope', '$mdDialog', '$state', 'contentTypeDefinition', '$timeout', ContentTypePreviewController]);

    /** @ngInject */
    function ContentTypePreviewController($scope, $mdDialog, $state, contentTypeDefinition, $timeout) {
        var vm = this;

        //Private Attributes
        var contentTypeDefinitionsController = new mdBusinessLogic.dataAccess.controllers.contentTypeDefinitionController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        //Public Attributes
        vm.isEditMode = false;
        vm.contentTypeDefinition = angular.copy(contentTypeDefinition);
        vm.isNew = !($state.params.id !== undefined && $state.params.id != '' && vm.contentTypeDefinition.Id != 0);

        //Public Methods
        vm.openEditor = openEditor;
        vm.closeEditor = closeEditor;
        vm.save = save
        vm.registerEditEvent = registerEditEvent;


        //Private Methods
        var toggleEditMode = function () { }
        function registerEditEvent(event) {
            toggleEditMode = event;
        }
        function toggleMsNavigationFolded() {
            $scope.$root.navigationFolded = !$scope.$root.navigationFolded;
        }
        function openEditor() {
            toggleEditMode();
            vm.isEditMode = true;
            toggleMsNavigationFolded();
            angular.element(document.querySelector('#content-types-preview')).addClass('full-screen');
        }
        function closeEditor(reinit) {
            if (reinit === undefined) {
                reinit = true;
            }

            toggleEditMode(reinit);
            vm.contentTypeDefinition = angular.copy(contentTypeDefinition);
            vm.isEditMode = false;
            toggleMsNavigationFolded();
            $state.go('app.content-types-list');
        }
        function save() {
            for (var i = 0; i < vm.contentTypeDefinition.Fields.length; i++) {
                vm.contentTypeDefinition.Fields[i].setOptions(vm.contentTypeDefinition.Fields[i].JsonField);
                if (vm.contentTypeDefinition.Fields[i].ListValue !== undefined &&
                    vm.contentTypeDefinition.Fields[i].ListValue.length !== undefined &&
                    typeof vm.contentTypeDefinition.Fields[i].ListValue !== 'string' &&
                    !(vm.contentTypeDefinition.Fields[i].ListValue instanceof String)) {
                    vm.contentTypeDefinition.Fields[i].ListValue = vm.contentTypeDefinition.Fields[i].ListValue.join(vm.contentTypeDefinition.Fields[i].Delimiter);
                }
            }
            contentTypeDefinitionsController.save(vm.contentTypeDefinition, function (data) {
                $scope.$apply(function () {
                    vm.closeEditor(false);
                });
            }, function (error) {
            });
        }
        function init() {
            $timeout(function () {
                if (vm.isNew) {
                    openEditor();
                }
            }, 1500);

            $scope.$on('$destroy', function () {
                $scope.$root.navigationFolded = false;
            })
        }

        init();
    }
})();
