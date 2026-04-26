(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsGridToolbarController', ['$scope', '$element', '$timeout', '$mdSidenav', mdCmsGridToolbarController]);
    /** @ngInject */
    function mdCmsGridToolbarController($scope, $element, $timeout, $mdSidenav) {

        //Private Attributes
        var vm = this;
        var tileData = [];
        var countLoaded = false;

        //Public Attributes
        vm.toolbarUniqueId = mdBusinessLogic.helpers.Guid.create().value;
        vm.toolbarButtonUniqueId = mdBusinessLogic.helpers.Guid.create().value;
        vm.editMode = false;
        vm.title = null;
        vm.open = false;
        vm.buttonStyle = {
            'top': '50%'
        };
        vm.toolbarStyle = {
            'top': '0'
        };

        //Public Methods
        vm.toggleRightToolbar = toggleRightToolbar;

        //Private Methods
        function toggleRightToolbar() {
            vm.open = !vm.open;
            $mdSidenav('md-cms-grid-right-toolbar').toggle();
        }

        function updateToolbarPosition(data, delay) {
            if (delay === undefined) {
                delay = 0;
            }
            var buttonHeight = angular.element('#' + vm.toolbarButtonUniqueId).height();
            var height = data.containerHeight;

            var buttonOffset = height / 2 + data.currentScrollTop - (buttonHeight / 2);
            var toolbarOffset = data.currentScrollTop;
            $timeout(function () {
                vm.buttonStyle['top'] = buttonOffset;
                vm.toolbarStyle['top'] = toolbarOffset;
                vm.toolbarStyle['height'] = height;
            }, delay);
        }

        function init() {
            $scope.$on('md-cms-grid-events-toggle-edit-mode', function (event, data) {
                vm.editMode = data;
            });

            $scope.$on('md-cms-grid-events-tile-added', function (event, data) {
                toggleRightToolbar();
            });

            $scope.$watch(function () {
                return $scope.mdTitle;
            }, function (mdTitle) {
                if (mdTitle !== undefined) {
                    vm.title = mdTitle;
                }
            });

            $scope.$watch(function () {
                return vm.editMode;
            }, function (newValue) {
                if (newValue) {
                    $timeout(function () {
                        $scope.$emit('md-cms-grid-toolbar-tilecount', $element.find('md-cms-grid-tile').length);
                        countLoaded = true;
                    }, 500);
                    $scope.$emit('ms-scroll-get-info');
                } else {
                }
            });

            $scope.$on('ms-scroll-event-scroll-y', function (event, data) {
                if (vm.editMode) {
                    updateToolbarPosition(data);
                }
            });

            $scope.$on('md-cms-grid-tile-data', function (event, data) {
                event.stopPropagation();
                tileData.push(data);
                $scope.$emit('md-cms-grid-toolbar-tile-data', data);
            });

            $timeout(function () {
                $scope.$emit('md-cms-grid-toolbar-events-loaded', vm.toolbarUniqueId);
            }, 500);
        }

        init();
    }
})();
