(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsGridTileController', ['$scope', '$attrs', '$timeout', '$element', mdCmsGridTileController]);
    /** @ngInject */
    function mdCmsGridTileController($scope, $attrs, $timeout, $element) {
        this.$attrs = $attrs;

        //Private Attributes
        var vm = this;

        //Public Attributes
        vm.uniqueId = null;
        vm.tileData = null;
        vm.data = null;
        vm.editMode = false;
        vm.disableFrame = $scope.disableFrame;
        vm.whiteframe = 0;
        vm.isAddable = false;
        vm.layoutPadding = false;
        vm.layoutWrap = false;
        vm.missingTitleBar = false;
        vm.isInToolbar = $element.closest('md-cms-grid-toolbar').length > 0;
        vm.isNestable = false;
        vm.layout = '';
        vm.minHeight = 100;
        vm.minWidth = 30;

        //Public Methods
        vm.getWhiteframeClass = getWhiteframeClass;
        vm.getHeaderClass = getHeaderClass;

        //Private Methods
        function getHeaderClass() {
            return {
                'md-whiteframe-4dp': true,
                'gjs-grid-toolbar-tile': vm.isInToolbar,
                'gjs-grid-tile': !vm.isInToolbar
            };
        }

        function getWhiteframeClass() {
            var classNames = {
                'layout-padding': vm.layoutPadding,
                'layout-wrap': vm.layoutWrap,
                'edit-mode': vm.editMode,
                'gjs-grid-toolbar-tile': vm.isInToolbar,
                'gjs-grid-tile': !vm.isInToolbar
            };
            classNames['md-whiteframe-' + vm.whiteframe.toString() + 'dp'] = true;
            return classNames;
        }

        function init() {
            if ($scope.tileData !== undefined) {
                vm.tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData($scope.tileData);
            }

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
                return $scope.isAddable;
            }, function (isAddable) {
                vm.isAddable = isAddable;
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

            $scope.$watch(function () {
                return $scope.minHeight;
            }, function (minHeight) {
                if (minHeight !== undefined && !isNaN(minHeight)) {
                    vm.minHeight = minHeight;
                    if (vm.tileData != null) {
                        vm.tileData.setMinHeight(vm.minHeight);
                    }
                }
            });

            $scope.$watch(function () {
                return $scope.minWidth;
            }, function (minWidth) {
                if (minWidth !== undefined && !isNaN(minWidth)) {
                    vm.minWidth = minWidth;
                    if (vm.tileData != null) {
                        vm.tileData.setMinWidth(vm.minWidth);
                    }
                }
            });

            $scope.$watch(function () {
                return $scope.withTitleBar;
            }, function (withTitleBar) {
                if (withTitleBar !== undefined) {
                    vm.missingTitleBar = !withTitleBar;
                }
            });

            $scope.$watch(function () {
                return $scope.isNestable;
            }, function (isNestable) {
                if (isNestable !== undefined) {
                    vm.isNestable = isNestable;
                    $element.attr('data-droppable', vm.isNestable);
                }
            });

            $scope.$watch(function () {
                return vm.tileData;
            }, function (tileData) {
                if (tileData !== undefined && tileData != null) {
                    $timeout(function () {
                        if (!vm.isInToolbar) {
                            $element.removeClass(function (index, className) {
                                return (className.match(/(^|\s)flex-\S+/g) || []).join(' ');
                            });
                            $element.removeClass(function (index, className) {
                                return (className.match(/(^|\s)flexheight-\S+/g) || []).join(' ');
                            });

                            $element.addClass('flex-gt-md-' + tileData.getWidth());

                            $element.addClass('flex-md-' + tileData.getWidth('medium'));

                            $element.addClass('flex-sm-' + tileData.getWidth('small'));

                            $element.addClass('flexheight-' + tileData.getHeight());
                        }
                    });
                }
            }, true);

            $timeout(function () {

                $element.attr('data-gjs-type', 'block-' + $element.attr('data-type'));

                function emit(data, tileData, toolbar) {
                    $scope.$emit('md-cms-grid-tile-data', {
                        element: $element,
                        data: data,
                        tileData: tileData,
                        id: $element.data('id'),
                        parentid: $element.data('parentid'),
                        type: $element.data('type'),
                        layout: $element.data('layout'),
                        tileid: $element.data('tileid'),
                        droppable: $element.data('droppable') == 'true',
                        toolbar: toolbar,
                        group: $scope.group
                    });
                }

                //emit(vm.data, vm.tileData);

                $scope.$on('md-cms-grid-events-tile-query-data-toolbar-' + $element.data('id'), function (event, data) {
                    vm.data = data.data;
                    vm.tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData(data.tileData)
                    emit(vm.data, vm.tileData, true);
                });

                $scope.$on('md-cms-grid-events-tile-query-data-' + $element.data('id'), function (event, data) {
                    vm.data = data.data;
                    vm.tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData(data.tileData)
                    emit(vm.data, vm.tileData, false);
                });

                $scope.$emit('md-cms-grid-events-tile-query-data', {
                    id: $element.data('id'),
                    tileData: vm.tileData,
                    parentid: $element.data('parentid'),
                    type: $element.data('type'),
                    layout: $element.data('layout'),
                    tileid: $element.data('tileid'),
                    droppable: $element.data('droppable') == 'true',
                    toolbar: vm.isInToolbar
                });
            });
        }

        init();
    }
})();
