(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdMemorySizeService', [mdMemorySizeService]);

    /** @ngInject */
    function mdMemorySizeService() {
        function gigaByteMemorySize(input, isShort) {
            if (isShort === undefined) {
                isShort = false;
            }
            var numValue = parseInt(input);
            numValue = numValue / (1000 * 1000 * 1000);
            return numValue.toFixed(2).toLocaleString() + ' ' + (isShort ? 'Gb' : 'Gigabytes');
        }

        function megaByteMemorySize(input, isShort) {
            if (isShort === undefined) {
                isShort = false;
            }
            var numValue = parseInt(input);
            numValue = numValue / (1000 * 1000);
            return numValue.toFixed(2).toLocaleString() + ' ' + (isShort ? 'Mb' : 'Megabytes');
        }

        function kiloByteMemorySize(input, isShort) {
            if (isShort === undefined) {
                isShort = false;
            }
            var numValue = parseInt(input);
            numValue = numValue / 1000;
            return numValue.toFixed(2).toLocaleString() + ' ' + (isShort ? 'Kb' : 'Kilobytes');
        }

        function byteMemorySize(input, isShort) {
            if (isShort === undefined) {
                isShort = false;
            }
            var numValue = parseInt(input);
            return numValue.toLocaleString() + ' ' + (isShort ? 'B' : 'Bytes');
        }

        function autoMemorySize(input, isShort) {
            if (isShort === undefined) {
                isShort = false;
            }
            var numValue = parseInt(input);
            if ((numValue / (1000 * 1000 * 1000)) >= 1) {
                return gigaByteMemorySize(input, isShort);
            } else if ((numValue / (1000 * 1000)) >= 1) {
                return megaByteMemorySize(input, isShort);
            } else if ((numValue / (1000)) >= 1) {
                return kiloByteMemorySize(input, isShort);
            }
            return byteMemorySize(input, isShort);
        }

        return {
            gigaByteMemorySize: gigaByteMemorySize,
            megaByteMemorySize: megaByteMemorySize,
            kiloByteMemorySize: kiloByteMemorySize,
            byteMemorySize: byteMemorySize,
            autoMemorySize: autoMemorySize
        };
    }
}());