(function () {
  'use strict';

  angular
    .module('app.core')
    .directive('mdDoubleClick', ['$timeout', mdDoubleClick]);

  /** @ngInject */
    function mdDoubleClick($timeout) {
        return {
            restrict: 'A',
            link: function ($scope, $elm, $attrs) {
                var clicks = 0;
                var isTouchScreen = mdBusinessLogic.helpers.touchScreenHelper.isTouchDevice();

                $elm.bind('click', function (evt) {
                    clicks++;
                    if (clicks == 1) {
                        if (isTouchScreen) {
                            $scope.$apply(function () {
                                $scope.$eval($attrs.mdDoubleClick)
                            });
                        } else {
                            $timeout(function () {
                                if (clicks != 1) {
                                    $scope.$apply(function () {
                                        $scope.$eval($attrs.mdDoubleClick)
                                    });
                                }
                                clicks = 0;
                            }, 300);
                        }
                    }
                });

            }
        }
    }
})();
