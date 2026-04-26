(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsSelect', ['$q', mdCmsSelect]);
    /** @ngInject */
    function mdCmsSelect($q) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-select/md-cms-select.template.html',
            transclude: true,
            scope: {
                mdModel: "=",
                mdMultiple: "=?",
                mdInputName: "@",
                placeholder: "@",
                mdFloatingLabel: "@",
                ngDisabled: "=",
                mdUseAutoComplete: "=?",
                mdRequireMatch: "=?"
            },
            link: function (scope, element, attrs) {
                //Directive variables
                scope.uniqueId = '';
                scope.listValueList = [];
                scope.useAutoComplete = false;
                scope.searchText = '';
                scope.selectedValues = [];
                scope.selectedItem = '';
                scope.inputUseAutoComplete = false;
                scope.inputRequireMatch = false;
                scope.inputMultiple = false;

                //Directive methods
                scope.getInnerHtml = getInnerHtml;
                scope.onChange = onChange;
                scope.querySearch = querySearch;

                function getInnerHtml() {
                    return element.find('md-inner-html').html() || '';
                }

                function onChange() {
                    if (scope.inputMultiple) {
                        scope.mdModel.value = scope.selectedValues.join(scope.mdModel.delimiter);
                    }
                }

                function init() {
                    if (scope.mdUseAutoComplete !== undefined) {
                        scope.inputUseAutoComplete = scope.mdUseAutoComplete;
                    }
                    if (scope.mdRequireMatch !== undefined) {
                        scope.inputRequireMatch = scope.mdRequireMatch;
                    }
                    if (scope.mdMultiple !== undefined) {
                        scope.inputMultiple = scope.mdMultiple;
                    }

                    scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                    scope.listValueList = scope.mdModel.listValue.split(scope.mdModel.delimiter);
                    scope.useAutoComplete = scope.inputUseAutoComplete || scope.listValueList.length > 10;

                    if (scope.mdModel.value != null) {
                        if (scope.inputMultiple) {
                            scope.selectedValues = scope.mdModel.value.split(scope.mdModel.delimiter);
                            for (var i = scope.selectedValues.length - 1; i >= 0; i--) {
                                if (scope.selectedValues[i] === undefined || scope.selectedValues[i] == null || scope.selectedValues[i].length == 0) {
                                    scope.selectedValues.splice(i, 1);
                                }
                            }
                        }
                    }
                }

                function querySearch(query) {
                    var results = query ? scope.listValueList.filter(createFilterFor(query)) : scope.listValueList;
                    return results;
                }

                function createFilterFor(query) {
                    var lowercaseQuery = query.toLowerCase();

                    return function filterFn(value) {
                        return (value.toLowerCase().indexOf(lowercaseQuery) === 0);
                    };

                }

                init();
            }
        };
    }
})();
