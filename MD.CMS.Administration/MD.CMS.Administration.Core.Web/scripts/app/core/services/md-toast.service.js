(function ()
{
    'use strict';

    angular
        .module('app.core')
        .factory('mdToastService', ['$mdToast', '$document', mdToastService]);

    /** @ngInject */
    function mdToastService($mdToast, $document)
    {

        var service = {
            showNavigationToast: showNavigationToast,
            showBodyToast: showBodyToast,
            showToast: showToast
        };

        return service;

        function showNavigationToast(toastText) {
            showToast(toastText, $document[0].querySelector('md-toolbar#toolbar > div > div.flex'), 'top right', reportBug);
        }

        function showBodyToast(toastText) {
            showToast(toastText, $document[0].querySelector('body'), 'top right');
        }

        function showToast(toastText, toastParent, toastPosition, hideDelay, toastAction, toastPromise) {
            if (toastParent === undefined) {
                toastParent = $document[0].querySelector('body');
            }
            if (toastPosition === undefined) {
                toastPosition = 'top right';
            }
            if (hideDelay === undefined) {
                hideDelay = 5000
            }

            var toast = $mdToast.show(
                    $mdToast.simple()
                    .textContent(toastText)
                    .position(toastPosition)
                    .hideDelay(hideDelay)
                    .action(toastAction != undefined ? toastAction : '')
                );
            if (toastAction != undefined && toastPromise != undefined) {
                toast.then(toastPromise);
            }
            return toast;
        }
    }
}());