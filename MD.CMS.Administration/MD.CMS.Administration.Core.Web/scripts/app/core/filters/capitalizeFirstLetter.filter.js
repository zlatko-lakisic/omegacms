(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('capitalize', function () {
            return function (input, scope) {
                if (input != null) {
                    var stringArr = input.split(" ");
                    var result = "";
                    var cap = stringArr.length;
                    for (var x = 0; x < cap; x++) {
                        stringArr[x].toLowerCase();
                        if (x === cap - 1) {
                            result += stringArr[x].substring(0, 1).toUpperCase() + stringArr[x].substring(1);
                        } else {
                            result += stringArr[x].substring(0, 1).toUpperCase() + stringArr[x].substring(1) + " ";
                        }
                    }
                    return result;
                }
            }
        });

})();