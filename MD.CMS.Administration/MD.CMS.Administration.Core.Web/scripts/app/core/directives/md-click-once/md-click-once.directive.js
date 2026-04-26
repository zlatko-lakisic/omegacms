(function () {
    'use strict';

    angular
        .module('app.core')
      .directive('mdClickOnce', ['$timeout', mdClickOnceDirective]);

    /** @ngInject */
    function mdClickOnceDirective($timeout) {
        return {
            restrict: 'A',
            link: function(scope, element, attrs) {
                element.bind('click', function() {
                    $timeout(function() {
                        element.attr('disabled', true);
                    });
                });
            }
        };
    }
})();
