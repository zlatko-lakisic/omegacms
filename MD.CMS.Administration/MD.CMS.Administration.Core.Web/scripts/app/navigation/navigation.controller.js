(function ()
{
    'use strict';

    angular
        .module('app.navigation')
        .controller('NavigationController', ['$scope', '$rootScope', NavigationController]);

    /** @ngInject */
    function NavigationController($scope, $rootScope)
    {
        var vm = this;

        // Data
        vm.bodyEl = angular.element('body');
        $scope.$watch('$root.navigationFolded', function (newValue, oldValue) {
            vm.folded = newValue;
        });
        $scope.$root.navigationFolded = false;
        vm.folded = $scope.$root.navigationFolded;
        vm.msScrollOptions = {
            suppressScrollX: true
        };
        $rootScope.systemVersion = mdBusinessLogic.globals.systemVersion;
        $rootScope.systemName = mdBusinessLogic.globals.systemName;

        // Methods
        vm.toggleMsNavigationFolded = toggleMsNavigationFolded;

        //////////

        /**
         * Toggle folded status
         */
        function toggleMsNavigationFolded()
        {
            $scope.$root.navigationFolded = !$scope.$root.navigationFolded;
        }

        // Close the mobile menu on $stateChangeSuccess
        $scope.$on('$stateChangeSuccess', function ()
        {
            vm.bodyEl.removeClass('ms-navigation-horizontal-mobile-menu-active');
        });
    }

})();