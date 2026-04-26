(function () {
    'use strict';

    angular
        .module('app.core')
        .constant('mdSavedDataKeys', mdSavedDataKeys())
        .provider('mdSavedData', ['$sessionStorageProvider', '$localStorageProvider', mdSavedDataProvider])
        .factory('mdSavedDataService', ['mdSavedData', mdSavedDataService]);

    /** @ngInject */
    function mdSavedDataService(mdSavedData) {
        return {
            storeData: mdSavedData.storeData,
            getData: mdSavedData.getData,
            deleteData: mdSavedData.deleteData
        };
    }

    /** @ngInject */
    function mdSavedDataProvider($sessionStorageProvider, $localStorageProvider) {

        function mdSavedData() {
            this.storeData = function (key, value, localStorage) {
                if (localStorage === undefined || localStorage == null) {
                    localStorage = false;
                }

                if (localStorage) {
                    $localStorageProvider.set(key, value);
                    $localStorageProvider.set(key + '-date', (new Date()).getTime());
                } else {
                    $localStorageProvider.set(key, value);
                    $localStorageProvider.set(key + '-date', (new Date()).getTime());
                }
            }
            this.getData = function (key, defaultValue) {
                if (defaultValue === undefined || defaultValue == null) {
                    defaultValue = null;
                }

                var result = $localStorageProvider.get(key);
                var storageDate = $localStorageProvider.get(key + '-date');

                if (result === undefined || result == null) {
                    result = $localStorageProvider.get(key);
                    storageDate = $localStorageProvider.get(key + '-date');
                }

                if (result === undefined || result == null) {
                    result = defaultValue;
                }

                if (storageDate !== undefined && storageDate != null && ((new Date()).getTime() - (storageDate + mdBusinessLogic.globals.sessionTimeout)) < 0) {
                    result = defaultValue;
                }

                return result;
            },
            this.deleteData = function (key) {
                $sessionStorageProvider.remove(key);
                $localStorageProvider.remove(key);
                $sessionStorageProvider.remove(key + '-date');
                $localStorageProvider.remove(key + '-date');
            }
        }

        var mdSavedDataObj = new mdSavedData();

        this.storeData = mdSavedDataObj.storeData;
        this.getData = mdSavedDataObj.getData;
        this.deleteData = mdSavedDataObj.deleteData;
        this.$get = function () {
            return mdSavedDataObj;
        }
    }

    function mdSavedDataKeys() {
        return {
            globals: {
                loggedOnUser: 'globals.loggedOnUser',
                loggedOnUserToken: 'globals.loggedOnUserToken',
                selectedLanguage: 'globals.selectedLanguage'
            },
            settings: {
                lcid: 'lcid'
            }
        };
    }
}());
