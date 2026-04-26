(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsButton', ['$q', mdCmsButton]);

    function mdCmsButton($q) {
        return {
            restrict: 'E',
            template: '<md-button ng-click="vm.ngClick($event)" ng-disabled="vm.ngDisabled() || vm.loading || !vm.isInitialized" class="md-cms-button-inner-button {{vm.mdClass}}" ng-class="vm.ngClass" type="{{vm.mdType}}">' +
                '<md-progress-circular ng-if="vm.loading" md-diameter="20px" />' +
                '<ng-transclude ng-if="!vm.loading" />' +
                '</md-button> ',
            transclude: true,
            scope: {
                ngClick: '&?',
                mdClick: '&?',
                ngDisabled: '&?',
                'class': '@?',
                ngClass: '@?',
                'type': '@?',
                showError: '=?'
            },
            controller: 'mdCmsButtonController as vm',
            link: function (scope, element, attrs) {
            }
        };
    }
})();
