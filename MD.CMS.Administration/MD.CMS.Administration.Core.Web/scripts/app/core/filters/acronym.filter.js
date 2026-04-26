(function ()
{
    'use strict';

    angular
        .module('app.core')
        .filter('acronym', [acronym]);

    /** @ngInject */
    function acronym()
    {
        return function (text) {
            return text
                .split(/\s/)
                .reduce(function (accumulator, word) {
                    return accumulator + word.charAt(0);
                }, '');
        };
    }

})();