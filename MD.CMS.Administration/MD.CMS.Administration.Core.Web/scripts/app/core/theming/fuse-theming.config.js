(function ()
{
    'use strict';

    angular
        .module('app.core')
        .config(['omegaThemingProvider', config]);

    /** @ngInject */
    function config(omegaThemingProvider)
    {
        omegaThemingProvider.apply();
    }

})();