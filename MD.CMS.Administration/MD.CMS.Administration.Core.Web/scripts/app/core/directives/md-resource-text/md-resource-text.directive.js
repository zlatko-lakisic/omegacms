(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdResourceText', ['$rootScope', 'mdResourceService', mdResourceTextDirective]);
    /** @ngInject */
    function mdResourceTextDirective($rootScope, mdResourceService) {
        return {
            restrict: 'EA',
            template: '<ng-transclude />{{text}}',
            transclude: true,
            scope: {
                name: "@",
                text: "@",
                replacevalue0: "@",
                replacevalue1: "@",
                replacevalue2: "@",
                replacevalue3: "@",
                replacevalue4: "@",
                replacevalue5: "@",
                replacevalue6: "@",
                replacevalue7: "@",
                replacevalue8: "@",
                replacevalue9: "@",
                replacevalue10: "@",
                replacevalue11: "@",
                replacevalue12: "@",
                replacevalue13: "@",
                replacevalue14: "@",
                replacevalue15: "@"
            },
            compile: function (element, attributes) {
                return {
                    pre: function(scope, element, attributes){
                        scope.text = ""
                    },
                    post: function (scope, element, attributes) {
                        var replaceNodeNames = [];
                        for (var i in element[0].attributes) {
                            var attr = element[0].attributes[i];
                            if (attr !== undefined &&
                                attr != null &&
                                attr.name !== undefined &&
                                attr.name != null &&
                                attr.name.lastIndexOf('replacevalue', 0) === 0 &&
                                attr.name != 'replacevalue') {
                                replaceNodeNames.push({
                                    'index': parseInt(attr.name.replace('replacevalue', '')),
                                    'value': attributes[attr.name]
                                });
                            }
                        }
                        for (var i = 0 - 1; i < 16; i++) {
                            var elementExists = false;
                            for (var e in replaceNodeNames) {
                                if (replaceNodeNames[e].index == i) {
                                    elementExists = true;
                                    break;
                                }
                            }
                            if (!elementExists) {
                                replaceNodeNames.push({
                                    'index': i,
                                    'value': ''
                                });
                            }
                        }
                        scope.text = mdResourceService.formatStringFromArray(attributes.name, replaceNodeNames);
                    }
                }
            }
        };
    }
})();
