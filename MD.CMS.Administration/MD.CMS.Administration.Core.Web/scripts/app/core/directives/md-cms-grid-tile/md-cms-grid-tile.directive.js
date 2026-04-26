(function () {
    'use strict';
    angular
        .module('app.core')
        .directive('mdCmsGridTile', ['$compile', '$timeout', mdCmsGridTile])
        //.directive('mdCmsGridTileNested', [mdCmsGridTileNested])
        .directive('mdCmsGridTileHeader', [mdCmsGridTileHeader])
        .directive('mdCmsGridTileHeaderInfo', ['$timeout', mdCmsGridTileHeaderInfo]);
    /** @ngInject */
    function mdCmsGridTile($compile, $timeout) {

        function setupTileData(tileData) {
            if (tileData === undefined || tileData == null) {
                tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();
            }
            return tileData;
        }

        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-grid-tile/md-cms-grid-tile.template.html',
            transclude: true,
            require: ['?^^mdCmsGrid', '?^^mdCmsGridToolbar'],
            scope: {
                uniqueId: '@',
                tileData: '=?',
                isAddable: '=?',
                addableClass: '@',
                disableFrame: '=?',
                ngClass: '=?',
                metaData: '=?',
                whiteframe: '@',
                layoutPadding: '@',
                layoutWrap: '@',
                group: '=?',
                isNestable: '=?',
                minHeight: '@',
                minWidth: '@'
            },
            controller: 'mdCmsGridTileController as vm',
            link: function (scope, element, attrs) {

                if (scope.whiteframe === undefined) {
                    scope.whiteframe = 4;
                }

                var tileData = new mdBusinessLogic.dataAccess.entities.grid.gridTileData();

                if (scope.tileData !== undefined) {
                    tileData.construct(scope.tileData);
                }
                scope.tileData = tileData;

                if (scope.uniqueId === undefined) {
                    scope.tileData = setupTileData(scope.tileData);
                    /*if (scope.tileData.uniqueId !== undefined && scope.tileData.uniqueId != null) {
                        scope.uniqueId = scope.tileData.uniqueId;
                    } else {
                        scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                        scope.tileData.uniqueId = scope.uniqueId;
                    }*/
                }
                //angular.element(element).attr('id', scope.uniqueId);

                attrs.$addClass('item md-cms-grid-tile layout-fill layout-column');

                if (scope.ngClass === undefined) {
                    scope.ngClass = {};
                }

                if (scope.addableClass === undefined) {
                    scope.addableClass = 'md-cms-grid-tile-new';
                }

                if (scope.isAddable) {
                    attrs.$addClass(scope.addableClass);
                }

                if (scope.disableFrame === undefined) {
                    scope.disableFrame = false;
                }

                if (scope.tileData !== undefined && scope.uniqueId !== undefined) {
                    scope.tileData.id = scope.uniqueId;
                }

                if (scope.isNestable === undefined) {
                    scope.isNestable = false;
                }

                scope.$on('md-cms-grid-events-reinit', function (event, data) {
                    angular.element(element).removeAttr('style');
                });

                if (scope.$parent.$last) {
                    scope.$emit('md-cms-grid-events-tile-last-loaded');
                }

                /*if (!scope.isAddable) {
                    var width = tileData.getWidth();
                    var widthPercent = Math.round((width / 12) * 100);
                    element.css({ 'width': widthPercent.toString() + '%' });

                    $timeout(function () {
                        var height = element.width() / width;
                        element.css({ 'min-height': height });
                    });
                }*/

                $timeout(function () {
                    var existingTileHeaders = element.children('md-whiteframe').children('md-cms-grid-tile-header');
                    scope.withTitleBar = existingTileHeaders.length > 0;
                    if (scope.withTitleBar) {
                        existingTileHeaders.prependTo(element);
                    }

                    if (scope.withTitleBar && existingTileHeaders.find('.handle').length == 0) {
                        //var el = $compile('<button class="handle"><md-tooltip md-direction="top">Drag me</md-tooltip></button>')(scope);
                        //existingTileHeaders.children('ng-transclude').append(el);
                    }
                });
            }
        }
    }

    function mdCmsGridTileNested() {
        return {
            restrict: 'E',
            transclude: true,
            replace: true,
            //require: ['^mdCmsGridTile']
        }
    }

    function mdCmsGridTileHeader() {
        return {
            restrict: 'E',
            template: '<md-cms-grid-tile-header-info ng-if="showHeaderTileInfo" flex /><ng-transclude layout="row" layout-align="end center" flex />',
            transclude: true,
            require: ['^mdCmsGridTile'],
            link: function (scope, element, attrs) {
                scope.showHeaderTileInfo = true;
                if (element.find('ng-transclude md-cms-grid-tile-header-info').length > 0) {
                    scope.showHeaderTileInfo = false;
                }
            }
        }
    }

    function mdCmsGridTileHeaderInfo($timeout) {
        return {
            restrict: 'E',
            template: '<div class="md-cms-grid-tile-header-info-box" ng-hide="true">{{info}}</div>',
            link: function (scope, element, attrs) {
                scope.showInfo = false;
                scope.info = 'span-size';
                scope.$on('md-cms-grid-tile-header-info', function (event, data) {
                    if (!event.defaultPrevented && data.showInfo !== undefined) {
                        if (data.showInfo) {
                            scope.showInfo = true;
                            scope.info = data.info;
                        } else {
                            if (data.info !== undefined) {
                                if (data.timeout === undefined || isNaN(data.timeout)) {
                                    data.timeout = 1000;
                                }

                                scope.showInfo = true;
                                scope.info = data.info;

                                $timeout(function () {
                                    scope.showInfo = false;
                                    scope.info = '';
                                }, data.timeout);
                            } else {
                                scope.showInfo = false;
                                scope.info = '';
                            }
                        }
                    }
                    event.defaultPrevented = true;
                });
            }
        }
    }
})();
