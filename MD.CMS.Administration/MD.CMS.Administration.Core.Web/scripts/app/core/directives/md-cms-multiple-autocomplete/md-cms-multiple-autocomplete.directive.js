(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsMultipleAutocomplete', ['$q', 'mdAutocompleteService', '$mdDialog', mdCmsMultipleAutocomplete]);
    /** @ngInject */
    function mdCmsMultipleAutocomplete($q, mdAutocompleteService, $mdDialog) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-multiple-autocomplete/md-cms-multiple-autocomplete.template.html',
            transclude: true,
            scope: {
                mdType: "=",
                mdInputName: "@",
                mdSelectedItem: "=?",
                placeholder: "@",
                ngDisabled: "=",
                ngPattern: "=?"
            },
            link: function (scope, element, attrs) {
                //Directive variables
                scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                scope.items = [];
                scope.entity = null;
                scope.selectedEntity = null;
                scope.requireMatch = true;

                //Directive methods
                scope.querySearch = mdAutocompleteService.setup(scope).querySearch;
                scope.selectedItemChange = selectedItemChange;
                scope.transformItemToChip = mdAutocompleteService.setup(scope).transformItemToChip;
                scope.addItemToList = mdAutocompleteService.setup(scope).addItemToList;
                scope.removeItemFromList = mdAutocompleteService.setup(scope).removeItemFromList;
                scope.getInnerHtml = getInnerHtml;
                scope.openMoreinfoDialog = openMoreinfoDialog;

                function selectedItemChange(entity) {
                    scope.mdSelectedItem.value = mdAutocompleteService.setup(scope).selectedItemChange(entity);
                    if (scope.mdChange) {
                        scope.mdChange()(entity);
                    }
                }

                function getInnerHtml() {
                    return element.find('md-inner-html').html() || '';
                }

                function openMoreinfoDialog(ev, entity) {
                    scope.selectedEntity = entity;
                    $mdDialog.show({
                        contentElement: '#' + scope.mdInputName + '_moreInfoDialog',
                        parent: angular.element(document.body),
                        targetEvent: ev,
                        clickOutsideToClose: true
                    });
                }

                mdAutocompleteService.setup(scope).init(true);

                if (scope.mdSelectedItem.jsonField.style.selectMultiple !== undefined && scope.mdSelectedItem.jsonField.style.selectMultiple.requireMatch !== undefined) {
                    scope.requireMatch = scope.mdSelectedItem.jsonField.style.selectMultiple.requireMatch;
                }
            }
        };
    }
})();
