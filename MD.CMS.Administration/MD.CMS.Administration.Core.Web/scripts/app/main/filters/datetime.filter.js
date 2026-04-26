(function () {
    'use strict';

    angular
        .module('app.filters')
        .filter('datetime', [function () {
            return function (input, format) {
                var inputMoment = moment(input);

                if (format === undefined) {
                    format = '[Today at] '

                    if (moment().diff(inputMoment, 'days') > 0) {
                        format = 'dddd [at] ';
                    }

                    if (moment().diff(inputMoment, 'months') > 0) {
                        format = 'ddd, DD MMM';
                    }

                    if (moment().diff(inputMoment, 'years') > 0) {
                        format += ' YYYY';
                    }

                    format += ' hh:mm A'
                } else if (format == 'ago') {
                    return inputMoment.fromNow();
                }


                return inputMoment.format(format);

            };
        }])
        .filter('addtime', [function () {
            return function (input, timestamp) {
                var duration = moment.duration(timestamp);
                var inputMoment = moment(input).add(duration);
                return inputMoment.toDate();
            };
        }]);

})();
