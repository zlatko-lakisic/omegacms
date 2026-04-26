(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('mdUser', ['mdUserService', mdUser]);

    /** @ngInject */
    function mdUser(mdUserService)
    {
        var result = {};
        var serviceInvoked = false;
        var filter = function (userId, expression, defaultValue) {
            if (!result[userId]) {
                if (!serviceInvoked) {
                    serviceInvoked = true;
                    mdUserService.parse(userId, expression, defaultValue).then(function (data) {
                        result[userId] = data;
                        serviceInvoked = false;
                    });
                }
                return '-';
            } else {
                return result[userId];
            }
        };
        filter.$stateful = true;
        return filter;
    }

})();