(function () {
    'use strict';

    angular
        .module('app.services')
        .service('FriendlyNameMakerService', [FriendlyNameMakerService]);


    /** @ngInject */
    function FriendlyNameMakerService() {
        var service = this;

        service.makeFriendlyName = function (name) {
            var friendlyName = null;
            if (name) {
                friendlyName = name.replace(/ /g, '_')
                    //.replace(/[^\w]/gi, '')
            }
          
            return friendlyName;
        }
    }
})();