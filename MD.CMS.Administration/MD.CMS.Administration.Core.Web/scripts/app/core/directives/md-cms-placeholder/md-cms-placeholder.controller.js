(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsPlaceholderController', ['$scope', '$q', '$element', mdCmsPlaceholderController]);
    /** @ngInject */
    function mdCmsPlaceholderController($scope, $q, $element) {

        //Private Attributes
        var vm = this;
        var waitingOnPromise = false;

        //Public Attributes
        vm.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
        vm.availableTamplates = {
            'xs': 1,
            's': 2,
            'm': 3,
            'l': 4,
            'xl': 5,
            'custom': 6,
            '1': 'xs',
            '2': 's',
            '3': 'm',
            '4': 'l',
            '5': 'xl',
            '6': 'custom'
        }
        vm.mdTemplate = vm.availableTamplates.m;
        vm.loading = true;

        //Public Methods

        //Private Methods
        function init() {
            angular.element($element).attr('id', vm.uniqueId);

            $scope.$watch(function () {
                return $scope.hasCustomTemplate;
            }, function (hasCustomTemplate, oldValue) {
                if (hasCustomTemplate && hasCustomTemplate != oldValue) {
                    $element.find('md-cms-placeholder-custom-template').appendTo('#' + vm.uniqueId + ' .custom-template');
                }
            });

            $scope.$watch(function () {
                return $scope.mdLoading;
            }, function (mdLoading) {
                if (mdLoading !== undefined && mdLoading != null) {
                    vm.loading = mdLoading;
                }
            });

            $scope.$watch(function () {
                return $scope.mdPromise;
            }, function (mdPromise) {
                if (mdPromise !== undefined && mdPromise != null) {
                    var result = mdPromise();
                    if (result !== undefined && result != null && !waitingOnPromise) {
                        waitingOnPromise = true;
                        $q.when().then(function () {
                            vm.loading = false;
                        }, function (error) {
                            vm.loading = false;
                        });
                    }
                }
            });

            $scope.$watchGroup([function () {
                return $scope.uniqueId;
            }, function () {
                return $scope.mdTemplate;
                }], function (data) {
                if (data[1]) {
                    vm.mdTemplate = vm.availableTamplates[data[1]];
                    if (vm.mdTemplate === undefined || vm.mdTemplate == null) {
                        vm.mdTemplate = vm.availableTamplates.m;
                    }
                }

                if (data[0]) {
                    vm.uniqueId = data[0];
                    angular.element($element).attr('id', vm.uniqueId);
                }
            });
        }

        init();
    }
})();
