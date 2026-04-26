(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsDiagramElementController', ['$scope', '$q', '$timeout', '$interval', '$element', mdCmsDiagramElementController]);
    /** @ngInject */
    function mdCmsDiagramElementController($scope, $q, $timeout, $interval, $element) {

        //Private Attributes
        var vm = this;

        //Public Attributes
        vm.disableFrame = $scope.disableFrame;
        vm.whiteframe = 0;
        vm.uniqueId = '';
        vm.isAddable = false;
        vm.layoutPadding = false;
        vm.layoutWrap = false;
        vm.editMode = false;
        vm.isInToolbar = $element.closest('md-cms-diagram-toolbar').length > 0;

        //Public Methods
        vm.getWhiteframeClass = getWhiteframeClass;

        //Private Methods
        function getWhiteframeClass() {
            var classNames = {
                'layout-padding': vm.layoutPadding,
                'layout-wrap': vm.layoutWrap,
                'edit-mode': vm.editMode
            };
            classNames['md-whiteframe-' + vm.whiteframe.toString() + 'dp'] = true;
            return classNames;
        }
        function init() {

            $scope.$watch(function () {
                return $scope.whiteframe;
            }, function (whiteframe) {
                vm.whiteframe = whiteframe;
            });

            $scope.$watch(function () {
                return $scope.uniqueId;
            }, function (uniqueId) {
                vm.uniqueId = uniqueId;
            });

            $scope.$watch(function () {
                return $scope.layoutPadding;
            }, function (layoutPadding) {
                vm.layoutPadding = layoutPadding;
            });

            $scope.$watch(function () {
                return $scope.layoutWrap;
            }, function (layoutWrap) {
                vm.layoutWrap = layoutWrap;
            });

            $timeout(function () {

                if (vm.isInToolbar) {
                    $element.addClass('toolbar');
                }

                function emit(data, tileData) {
                    $scope.$emit('md-cms-diagram-element-data', {
                        element: $element,
                        data: data,
                        tileData: tileData,
                        id: $element.data('id'),
                        parentid: $element.data('parentid'),
                        type: 'element',
                        layout: $element.data('layout'),
                        tileid: $element.data('tileid'),
                        droppable: $element.data('droppable') == 'true',
                        toolbar: vm.isInToolbar,
                        group: $element.data('group'),
                        label: $element.data('label'),
                    });
                }

                //emit(vm.data, vm.tileData);

                $scope.$on('md-cms-diagram-events-element-query-data-toolbar-' + $element.data('id'), function (event, data) {
                    vm.data = data.data;
                    vm.tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData(data.tileData)
                    emit(vm.data, vm.tileData);
                });

                $scope.$on('md-cms-diagram-events-element-query-data-' + $element.data('id'), function (event, data) {
                    vm.data = data.data;
                    vm.tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData(data.tileData)
                    emit(vm.data, vm.tileData);
                });

                $scope.$emit('md-cms-diagram-events-element-query-data', {
                    id: $element.data('id'),
                    tileData: vm.tileData,
                    parentid: $element.data('parentid'),
                    type: 'element',
                    layout: $element.data('layout'),
                    tileid: $element.data('tileid'),
                    droppable: $element.data('droppable') == 'true',
                    toolbar: vm.isInToolbar,
                    label: $element.data('label'),
                    group: $element.data('group'),
                });
            });
        }

        init();
    }
})();
