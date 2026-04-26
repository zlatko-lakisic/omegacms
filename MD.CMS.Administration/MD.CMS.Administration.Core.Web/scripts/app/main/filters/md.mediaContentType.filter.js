(function ()
{
    'use strict';

    angular
        .module('app.filters')
        .filter('mediaContentType', function () {
            return function (input, scope) {
                if (input != null) {
                    return mdBusinessLogic.dataAccess.entities.mediaContentInputType[input];
                }
            }
        });

})();
