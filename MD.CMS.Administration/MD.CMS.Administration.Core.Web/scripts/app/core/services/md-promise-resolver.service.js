(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdPromiseResolverService', ['$q', mdPromiseResolverService]);

    /** @ngInject */
    function mdPromiseResolverService($q) {

        function resolvePromises(promiseArrays, onSuccess, onError) {
            if (onError === undefined) {
                onError = function (ex) { };
            }

            try {
                var promiseArray = [];
                if (Array.isArray(promiseArrays)) {
                    for (var po = 0; po < promiseArrays.length; po++) {
                        for (var i = 0; i < promiseArrays[po].length; i++) {
                            var event = promiseArrays[po][i];
                            promiseArray.push(event());
                        }
                    }
                } else {
                    for (var i = 0; i < promiseArrays.length; i++) {
                        var event = promiseArrays[i];
                        promiseArray.push(event());
                    }
                }
                
                $q.all(promiseArray).then(function (data) {
                    try {
                        onSuccess(data);
                    } catch (e) {
                        onError(e);
                    }
                });
            } catch (e) {
                onError(e);
            }
        }

        return {
            resolve: resolvePromises
        };
    }
}());
