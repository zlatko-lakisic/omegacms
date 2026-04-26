(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsDiagramToolbarController', ['$scope', '$q', '$timeout', '$mdSidenav', '$element', mdCmsDiagramToolbarController]);
    /** @ngInject */
    function mdCmsDiagramToolbarController($scope, $q, $timeout, $mdSidenav, $element) {

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
            $mdSidenav('md-cms-diagram-right-toolbar').toggle();
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
            $scope.$on('md-cms-diagram-events-toggle-edit-mode', function (event, data) {
                vm.editMode = data;
            });

            $scope.$on('md-cms-diagram-events-element-added', function (event, data) {
                toggleRightToolbar();
            });

            $scope.$watch(function () {
                return vm.editMode;
            }, function (newValue) {
                if (newValue) {
                    $timeout(function () {
                        $scope.$emit('md-cms-diagram-toolbar-tilecount', $element.find('md-cms-diagram-element').length);
                        for (var i = 0; i < tileData.length; i++) {
                            $scope.$emit('md-cms-diagram-toolbar-element-data', tileData[i]);
                        }
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

            $scope.$on('md-cms-diagram-element-data', function (event, data) {
                event.stopPropagation();
                tileData.push(data);
            });

            $scope.$watch(function () {
                return $scope.mdTitle;
            }, function (mdTitle) {
                if (mdTitle !== undefined) {
                    vm.title = mdTitle;
                }
            });

            $timeout(function () {
                $scope.$emit('md-cms-diagram-toolbar-events-loaded', vm.toolbarUniqueId);
            }, 500);

            $scope.$watch(function () { return countLoaded; }, function (countLoaded) {
                if (countLoaded) {
                    var accordion = $element.find('#accordion');
                    var groups = [];
                    $element.find('md-cms-diagram-element').each(function (el) {
                        var element = angular.element(el);
                        var group = element.data('group');
                        if (group && groups.indexOf(group) < 0) {
                            groups.push(element.data('group'));
                            accordion.append('<div id="group_' + group.toLowerCase() + '"></div>');
                        }
                    });
                }
            });
        }

        init();
    }
})();
