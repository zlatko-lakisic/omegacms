(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsMap', ['$q', 'uiGmapGoogleMapApi', 'mdFeedbackService', '$location', '$timeout', mdCmsMap]);
    /** @ngInject */
    function mdCmsMap($q, uiGmapGoogleMapApi, $mdFeedbackService, $location, $timeout) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-map/md-cms-map.template.html',
            transclude: true,
            scope: {
                mdProvider: "=",
                mdInputName: "@",
                mdFloatingLabel: "@",
                mdMapCoordinates: "=?",
                mdMapZoom: "=?",
                placeholder: "@",
                ngDisabled: "=",
                ngPattern: "=?"
            },
            link: function ($scope, element, attrs) {
                $timeout(function () {
                    element.height(element.parent().height());

                    element.find('ui-gmap-google-map').height(element.height() - element.find('.title').height());
                });
            },
            controller: ['$scope', function ($scope) {

                if ($scope.ngPattern === undefined || $scope.ngPattern == null) {
                    $scope.ngPattern = '';
                }

                if ($scope.mdMapZoom === undefined || $scope.mdMapZoom == null) {
                    $scope.mdMapZoom = 17;
                }

                if ($scope.mdMapCoordinates === undefined || $scope.mdMapCoordinates == null) {
                    $scope.mdMapCoordinates = '0;0';
                }

                //Directive variables
                $scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                $scope.map = {
                    zoom: $scope.mdMapZoom,
                    events: {
                        click: function (map, eventName, originalEventArgs) {
                            var e = originalEventArgs[0];
                            $scope.$apply(function () {
                                $scope.map.marker = new google.maps.Marker({
                                    coords: {
                                        latitude: e.latLng.lat(),
                                        longitude: e.latLng.lng()
                                    }
                                });
                                $scope.map.center = {
                                    latitude: $scope.map.marker.coords.latitude,
                                    longitude: $scope.map.marker.coords.longitude
                                };
                                $scope.mdMapCoordinates = $scope.map.marker.coords.latitude + ';' + $scope.map.marker.coords.longitude;
                            });
                            return false;
                        }
                    },
                    center: {
                        latitude: 0,
                        longitude: 0
                    },
                    options: {
                        scrollwheel: false
                    }
                }

                //Directive methods
                function init(lat, lon) {
                    if (lat === undefined || lat == null || lat == 0 || lon === undefined || lon == null || lon == 0) {
                        if ($location.protocol() == 'https') {
                            if (navigator.geolocation) {
                                navigator.geolocation.getCurrentPosition(function (position) {
                                    renderMap(position.coords.latitude, position.coords.longitude);
                                });
                            }
                        } else {
                            lat = 40.766633299999995;
                            lon = -73.99474479999999;
                            renderMap(lat, lon);
                        }
                    } else {
                        renderMap(lat, lon);
                    }
                    function renderMap(lat, lon) {
                        uiGmapGoogleMapApi.then(function (map) {
                            $scope.map.marker = new google.maps.Marker({
                                coords: {
                                    latitude: lat,
                                    longitude: lon
                                }
                            });
                            $scope.map.center = {
                                latitude: lat,
                                longitude: lon
                            }
                        }, function (error) {
                            $mdFeedbackService.reportError('load', error);
                        });
                    }
                }

                $scope.$watch('mdMapCoordinates', function () {
                    if ($scope.mdMapCoordinates !== undefined && $scope.mdMapCoordinates != null) {
                        init($scope.mdMapCoordinates.split(';')[0], $scope.mdMapCoordinates.split(';')[1]);
                    } else {
                        init();
                    }
                });
            }]
        }
    }
})();
