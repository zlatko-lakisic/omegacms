(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsButtonController', ['$scope', '$q', '$element', 'mdFeedbackService', mdCmsButtonController]);
    /** @ngInject */
    function mdCmsButtonController($scope, $q, $element, $mdFeedbackService) {

        //Private Attributes
        var vm = this;
        var _ngClick = function () { };
        var showError = false;

        //Public Attributes
        vm.mdClass = '';
        vm.mdtype = '';
        vm.ngClass = {};
        vm.loading = false;
        vm.isInitialized = false;

        //Public Methods
        vm.ngClick = ngClick;
        vm.ngDisabled = function () { return false; };

        //Private Methods
        function ngClick(event) {
            if (!vm.loading && _ngClick) {
                vm.loading = true;
                try {
                    var result = _ngClick({ e: event });
                    if (result) {
                        switch (typeof result) {
                            case 'object': {
                                switch (mdBusinessLogic.helpers.checkType.getTypeName(result)) {
                                    case 'Promise': {
                                        if (result.$$state && result.$$state.status == 1) {
                                            vm.loading = false;
                                        } else {
                                            $q.when(result).then(function () {
                                                vm.loading = false;
                                            }, function (error) {
                                                vm.loading = false;
                                                if (showError && error) {
                                                    $mdFeedbackService.reportError("load", error);
                                                }
                                            });
                                        }
                                    } break;
                                }
                            } break;
                        }
                    }
                } catch (e) {
                    vm.loading = false;
                    console.error(e);
                    if (showError && error) {
                        $mdFeedbackService.reportError("load", e);
                    }
                }
            }
        }

        function init() {
            $scope.$watch(function () {
                return $scope.ngClick;
            }, function (ngClick) {
                    if (ngClick !== undefined) {
                        _ngClick = ngClick;
                    }
            });

            $scope.$watch(function () {
                return $scope.mdClick;
            }, function (ngClick) {
                if (ngClick !== undefined) {
                    _ngClick = ngClick;
                }
            });

            $scope.$watch(function () {
                return $scope.ngDisabled;
            }, function (ngDisabled) {
                vm.ngDisabled = ngDisabled;
            });

            $scope.$watch(function () {
                return $scope['class'];
            }, function (_class) {
                vm.mdClass = _class;
                if (vm.mdClass !== undefined) {
                    $element.removeClass(vm.mdClass);
                }
            });

            $scope.$watch(function () {
                return $scope.ngClass;
            }, function (ngClass) {
                vm.ngClass = ngClass;
            });

            $scope.$watch(function () {
                return $scope['type'];
            }, function (_type) {
                vm.mdtype = _type;
            });

            $scope.$watch(function () {
                return $scope.showError;
            }, function (_showError) {
                if (_showError) {
                    showError = _showError;
                }
            });

            $scope.$watch(function () {
                return mdBusinessLogic.dataAccess.controllers.systemInfoController.getIsInitialized();
            }, function (isInitialized) {
                vm.isInitialized = isInitialized;
            });
        }

        init();
    }
})();
