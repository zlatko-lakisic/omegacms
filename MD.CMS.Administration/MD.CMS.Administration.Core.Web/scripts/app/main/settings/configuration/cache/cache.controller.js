(function () {
    'use strict';

    angular
        .module('app.settings.configuration.cache')
        .controller('CacheController', ['$scope', '$state', 'mdFeedbackService', 'mdDataGroupService', 'mdMemorySizeService', 'allDataCache', CacheController]);

    /** @ngInject */
    function CacheController($scope, $state, mdFeedbackService, mdDataGroupService, mdMemorySizeService, allDataCache) {

        var vm = this;

        var colors = ['#03A9F4', '#3F51B5', '#E91E63', '#009688']


        vm.allDataCache = allDataCache;

        var bigChartData = processBigChartData();

        vm.widget1 = {
            title: 'Overview',
            onlineUsers: processBigChartData().length,
            bigChart: {
                options: {
                    chart: {
                        type: 'lineWithFocusChart',
                        color: ['#4caf50', '#3f51b5', '#ff5722'],
                        height: 400,
                        margin: {
                            top: 32,
                            right: 32,
                            bottom: 64,
                            left: 65
                        },
                        isArea: true,
                        useInteractiveGuideline: true,
                        duration: 1,
                        clipEdge: true,
                        clipVoronoi: false,
                        interpolate: 'cardinal',
                        showLegend: false,
                        x: function (d) {
                            return d.x;
                        },
                        y: function (d) {
                            return d.y;
                        },
                        xAxis: {
                            showMaxMin: false,
                            tickFormat: function (d) {
                                var date = new Date(new Date().setTime(d));
                                return d3.time.format(getGroupFormat(bigChartData.group))(date);
                            }
                        },
                        yAxis: {
                            showMaxMin: false,
                            tickFormat: function (d) {
                                return mdMemorySizeService.autoMemorySize(d, true);
                            }
                        },
                        x2Axis: {
                            showMaxMin: false,
                            tickFormat: function (d) {
                                var date = new Date(new Date().setTime(d));
                                return d3.time.format(getGroupFormat(bigChartData.group))(date);
                            }
                        },
                        y2Axis: {
                            showMaxMin: false
                        },
                        interactiveLayer: {
                            tooltip: {
                                gravity: 's',
                                classes: 'gravity-s'
                            }
                        },
                        legend: {
                            margin: {
                                top: 8,
                                right: 0,
                                bottom: 32,
                                left: 0
                            },
                            rightAlign: false
                        }
                    }
                },
                data: bigChartData,
                totalSize: allDataCache.reduce(function (total, currentValue) {
                    return currentValue.CacheObjects.reduce(function (total, currentValue) {
                        return total + currentValue.ByteSize;
                    }, 0);
                }, 0)
            },
            smallCharts: {}
        };
        vm.showValue = showValue;
        vm.invalidateValue = invalidateValue;

        function init() {
            for (var i = 0; i < allDataCache.length; i++) {
                vm.widget1.smallCharts[allDataCache[i].ProviderName] = buildProviderChartOptions(allDataCache[i].ProviderName, allDataCache[i].CacheObjects.map(function (obj) {
                    return {
                        x: obj.CacheTime,
                        y: obj.ByteSize
                    };
                }), colors[i]);
            }
        }

        function showValue(cacheValue) {
            mdFeedbackService.reportJsonValue(cacheValue);
        }

        function invalidateValue(provider, cacheKey) {
            (new mdBusinessLogic.dataAccess.controllers.cacheController()).invalidateDataCache(provider, cacheKey, function (data) {
                $scope.$apply(function () {
                    mdFeedbackService.reportInfo('delete', function () {
                        $state.reload();
                    });
                });
            }, function (error) {
                $scope.$apply(function () {
                    mdFeedbackService.reportError('delete', error);
                });
            });
        }

        function processBigChartData() {
            var data = [];
            for (var i = 0; i < allDataCache.length; i++) {
                for (var j = 0; j < allDataCache[i].CacheObjects.length; j++) {
                    var dataObj = data.filter(function (d) { return d.key == allDataCache[i].CacheObjects[j].CacheSource; })[0];
                    if (dataObj === undefined) {
                        dataObj = {
                            key: allDataCache[i].CacheObjects[j].CacheSource,
                            unsorted: [],
                            sorted: {},
                            values: []
                        };
                        data.push(dataObj);
                    }

                    dataObj.unsorted.push({
                        x: allDataCache[i].CacheObjects[j].CacheTime,
                        y: allDataCache[i].CacheObjects[j].ByteSize
                    });
                }
            }

            for (var i = 0; i < data.length; i++) {
                data[i].sorted = mdDataGroupService.autoGroup(data[i].unsorted);
                data[i].values = data[i].sorted.data;
            }

            return data;
        }

        function getGroupFormat(group) {
            switch (group) {
                case 'days':
                    return '%A, %B %d %I:%M %p';
                case 'hours':
                    return '%b %d %I:%M %p';
                case 'minutes':
                    return '%I:%M %p';
                default:
                    return '%I:%M %p';
            }
        }

        function buildProviderChartOptions(propertyName, data, color) {
            var smallChartData = mdDataGroupService.autoGroup(data);
            return {
                title: propertyName,
                value: data.length,
                options: {
                    chart: {
                        type: 'lineChart',
                        color: [color],
                        height: 40,
                        margin: {
                            top: 4,
                            right: 4,
                            bottom: 4,
                            left: 4
                        },
                        isArea: true,
                        interpolate: 'cardinal',
                        clipEdge: true,
                        duration: 500,
                        showXAxis: false,
                        showYAxis: false,
                        showLegend: false,
                        useInteractiveGuideline: true,
                        x: function (d) {
                            return d.x;
                        },
                        y: function (d) {
                            return d.y;
                        },
                        xAxis: {
                            tickFormat: function (d) {
                                var date = new Date(new Date().setTime(d));
                                return d3.time.format(getGroupFormat(smallChartData.group))(date);
                            }
                        },
                        yAxis: {
                            tickFormat: function (d) {
                                return mdMemorySizeService.autoMemorySize(d);
                            }
                        },
                        interactiveLayer: {
                            tooltip: {
                                gravity: 's',
                                classes: 'gravity-s'
                            }
                        }
                    }
                },
                data: [{
                    key: propertyName,
                    values: smallChartData.data
                }],
                totalSize: data.reduce(function (total, currentValue) {
                    return total + currentValue.y;
                }, 0)
            };
        }

        init();
    }
})();
