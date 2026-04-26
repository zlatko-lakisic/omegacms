(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('gigabytememorysize', ['mdMemorySizeService', function (mdMemorySizeService) {
            return mdMemorySizeService.gigaByteMemorySize;
        }])
        .filter('megabytememorysize', ['mdMemorySizeService', function (mdMemorySizeService) {
            return mdMemorySizeService.megaByteMemorySize;
        }])
        .filter('kilobytememorysize', ['mdMemorySizeService', function (mdMemorySizeService) {
            return mdMemorySizeService.kiloByteMemorySize;
        }])
        .filter('bytememorysize', ['mdMemorySizeService', function (mdMemorySizeService) {
            return mdMemorySizeService.byteMemorySize;
        }])
        .filter('memorysize', ['mdMemorySizeService', function (mdMemorySizeService) {
            return mdMemorySizeService.autoMemorySize;
        }]);

})();