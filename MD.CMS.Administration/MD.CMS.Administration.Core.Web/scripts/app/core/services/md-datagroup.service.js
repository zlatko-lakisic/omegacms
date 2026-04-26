(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdDataGroupService', [mdDataGroupService]);

    /** @ngInject */
    function mdDataGroupService() {

        function processBaseOptions(options) {
            var opts = angular.extend({
                data: [],
                rangeProperty: 'x',
                valueProperty: 'y',
                minRangeValue: 0,
                maxRangeValue: 1,
                rangeInterval: 1,
                valueProcessor: function (data) {
                    return data[opts.valueProperty];
                },
                rangeProcessor: function (data) {
                    return data[opts.rangeProperty];
                },
                getRangeArray: function () {
                    var rangeArray = [];
                    for (var i = opts.minRangeValue; i <= opts.maxRangeValue + (opts.rangeInterval * 2); i += opts.rangeInterval) {
                        rangeArray.push(i);
                    }
                    return rangeArray;
                }
            }, options);
            return opts;
        }

        function compareValue(options) {
            return function (a, b) {
                if (options.valueProcessor(a) < options.valueProcessor(b)) {
                    return -1;
                }
                if (options.valueProcessor(a) > options.valueProcessor(b)) {
                    return 1;
                }
                return 0;
            }
        }

        function compareRange(options) {
            return function (a, b) {
                if (options.rangeProcessor(a) < options.rangeProcessor(b)) {
                    return -1;
                }
                if (options.rangeProcessor(a) > options.rangeProcessor(b)) {
                    return 1;
                }
                return 0;
            }
        }

        function baseGroupBy(options) {
            var sorted = options.data.sort(compareRange(options));
            options.minRangeValue = options.rangeProcessor(sorted[0]);
            options.maxRangeValue = options.rangeProcessor(sorted[sorted.length - 1]);
            var rangeArray = options.getRangeArray();
            var data = [];
            for (var i = 0; i < rangeArray.length; i++) {
                var obj = {};

                if (i > 0) {
                    obj[options.valueProperty] = sorted.filter(function (data) {
                        return options.rangeProcessor(data) > rangeArray[i - 1] && options.rangeProcessor(data) <= rangeArray[i];
                    }).reduce(function (total, currentValue) {
                        return total + options.valueProcessor(currentValue);
                    }, 0);
                } else {
                    obj[options.valueProperty] = sorted.filter(function (data) {
                        return options.rangeProcessor(data) == rangeArray[i];
                    }).reduce(function (total, currentValue) {
                        return total + options.valueProcessor(currentValue);
                    }, 0);
                }

                obj[options.rangeProperty] = rangeArray[i];

                data.push(obj);
            }
            return data;
        }

        function groupBySeconds(secondsData, opts) {
            if (secondsData === undefined || secondsData == null) {
                secondsData = [];
            }

            if (opts === undefined) {
                opts = {};
            }
            var options = processBaseOptions(angular.extend(opts, {
                data: secondsData,
                rangeProcessor: function (data) {
                    return data[options.rangeProperty].getTime();
                },
                rangeInterval: 1000
            }));

            return {
                data: baseGroupBy(options),
                group: 'seconds'
            };
        }

        function groupByMinutes(minutesData, opts) {
            if (minutesData === undefined || minutesData == null) {
                minutesData = [];
            }

            if (opts === undefined) {
                opts = {};
            }
            var options = processBaseOptions(angular.extend(opts, {
                data: minutesData,
                rangeProcessor: function (data) {
                    return data[options.rangeProperty].getTime();
                },
                rangeInterval: 1000 * 60
            }));

            return {
                data: baseGroupBy(options),
                group: 'minutes'
            };
        }

        function groupByHours(hoursData, opts) {
            if (hoursData === undefined || hoursData == null) {
                hoursData = [];
            }

            if (opts === undefined) {
                opts = {};
            }
            var options = processBaseOptions(angular.extend(opts, {
                data: hoursData,
                rangeProcessor: function (data) {
                    return data[options.rangeProperty].getTime();
                },
                rangeInterval: 1000 * 60 * 60
            }));

            return {
                data: baseGroupBy(options),
                group: 'hours'
            };
        }

        function groupByDays(daysData, opts) {
            if (daysData === undefined || daysData == null) {
                daysData = [];
            }

            if (opts === undefined) {
                opts = {};
            }
            var options = processBaseOptions(angular.extend(opts, {
                data: daysData,
                rangeProcessor: function (data) {
                    return data[options.rangeProperty].getTime();
                },
                rangeInterval: 1000 * 60 * 60 * 24
            }));

            return {
                data: baseGroupBy(options),
                group: 'days'
            };
        }

        function autoGroup(autoData, opts) {
            if (autoData === undefined || autoData == null) {
                autoData = [];
            }

            if (opts === undefined) {
                opts = {};
            }
            var options = processBaseOptions(angular.extend(opts, {
                data: autoData,
                rangeProcessor: function (data) {
                    return data[options.rangeProperty].getTime();
                }
            }));

            var sorted = options.data.sort(compareRange(options));
            var difference = options.rangeProcessor(sorted[sorted.length - 1]) - options.rangeProcessor(sorted[0]);

            if (difference / (1000 * 60 * 60 * 24) > 1) {
                return groupByDays(autoData, opts);
            } else if (difference / (1000 * 60 * 60) > 1) {
                return groupByHours(autoData, opts);
            } else if (difference / (1000 * 60) > 1) {
                return groupByMinutes(autoData, opts);
            }
            return groupBySeconds(autoData, opts);
        }

        return {
            groupBySeconds: groupBySeconds,
            groupByMinutes: groupByMinutes,
            groupByHours: groupByHours,
            groupByDays: groupByDays,
            autoGroup: autoGroup
        };
    }
}());