(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsAutocomplete', ['$q', 'mdAutocompleteService', '$mdDialog', mdCmsAutocomplete]);
    /**
    * @name md-cms-autocomplete
    * @directive md-cms-autocomplete
    * @example <md-cms-autocomplete md-type="vm.type" md-input-name="my_test_input" md-floating-label="My Test Input" />
    * @param {reference} md-type
    * @param {literal} md-input-name
    * @param {literal} md-floating-label
    * @param {reference} [md-selected-item]
    * @param {reference} [md-selected-entity]
    * @param {literal} placeholder
    * @param {reference} ng-disabled
    * @param {reference} [ng-pattern]
    */
    function mdCmsAutocomplete($q, mdAutocompleteService, $mdDialog) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-autocomplete/md-cms-autocomplete.template.html',
            transclude: true,
            scope: {
                mdType: "=",
                mdInputName: "@",
                mdFloatingLabel: "@",
                mdSelectedItem: "=?",
                mdSelectedEntity: "=?",
                placeholder: "@",
                ngDisabled: "=",
                ngPattern: "=?",
                mdChange: "&?",
                mdOnSave: "&?"
            },
            link: function (scope, element, attrs) {
                //Directive variables
                if (scope.mdSelectedItem === undefined || scope.mdSelectedItem == null) {
                    scope.mdSelectedItem = { value: null };
                }
                if (scope.mdSelectedEntity === undefined || scope.mdSelectedEntity == null) {
                    scope.mdSelectedEntity = {};
                }
                if (scope.ngPattern === undefined || scope.ngPattern == null) {
                    scope.ngPattern = '';
                }
                scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;

                if (scope.ngPattern === undefined || scope.ngPattern == null) {
                    scope.ngPattern = '';
                }


                if (scope.mdMultiple === undefined || scope.mdMultiple == null) {
                    scope.mdMultiple = false;
                }

                scope.entity = null;
                scope.typeString = '';

                //Directive methods
                scope.querySearch = mdAutocompleteService.setup(scope).querySearch;
                scope.selectedItemChange = selectedItemChange;
                scope.getInnerHtml = getInnerHtml;
                scope.openMoreinfoDialog = openMoreinfoDialog;
                scope.openNewElementDialog = openNewElementDialog;

                //Autocomplete query search
                function onSave() {
                }
                function selectedItemChange(entity) {
                    scope.mdSelectedItem.value = mdAutocompleteService.setup(scope).selectedItemChange(entity);
                    if (scope.mdChange) {
                        scope.mdChange()(entity);
                    }
                }
                function getInnerHtml() {
                    return element.find('md-inner-html').html() || '';
                }
                function openMoreinfoDialog(ev) {
                    $mdDialog.show({
                        contentElement: '#' + scope.mdInputName + '_moreInfoDialog',
                        parent: angular.element(document.body),
                        targetEvent: ev,
                        clickOutsideToClose: true,
                        multiple: true
                    });
                }
                

                function openNewElementDialog(ev) {

                    $mdDialog.show({
                        parent: angular.element(document.body),
                        targetEvent: ev,
                        templateUrl: 'scripts/app/core/directives/md-cms-content-form/dialog/md-cms-content-form-dialog.template.html',
                        locals: {
                            data: {
                                constraints: scope.mdSelectedItem.jsonField.getRelevantConstraint(),
                                uniqueId: scope.mdInputName
                            }
                        },
                        clickOutsideToClose: true,
                        controller: 'mdCmsContentFormDialogController as vm',
                        multiple: true
                    }).then(function (data) {
                        scope.mdSelectedItem.value = data.UniqueId;
                        scope.promise = mdAutocompleteService.setup(scope).init();
                    }, function (error) {
                    });
                }

                if (scope.mdOnSave !== undefined && scope.mdOnSave != null) {
                    scope.mdOnSave()(onSave);
                }

                scope.promise = mdAutocompleteService.setup(scope).init();
            }
        };
    }
})();
