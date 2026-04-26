(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('switchmbgb', [filterSwitchmbgb]);

    /** @ngInject */
    function filterSwitchmbgb()
    {
        return function (mbValue, gbValue, mbTail, gbTail) {
            if (mbValue >= 1024) {
                return gbValue + gbTail;
            } else {
                return mbValue + mbTail;
            }
        };
    }

})();