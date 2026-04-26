(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('mdTrustUrl', ['$sce', mdTrustUrl]);

    /** @ngInject */
    function mdTrustUrl($sce)
    {
        return function (recordingUrl) {
            return $sce.trustAsResourceUrl(recordingUrl);
        };
    }

})();