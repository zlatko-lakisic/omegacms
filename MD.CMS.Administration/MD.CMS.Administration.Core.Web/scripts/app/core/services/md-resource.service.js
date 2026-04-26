(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdResourceService', [mdResourceService]);

    /**
    * @name mdResourceService
    * @service mdResourceService
    */
    function mdResourceService() {
        function loadDefault(variable, defaultValue) {
            if (variable === undefined || variable == null) {
                variable = defaultValue;
            }
            return variable;
        }
        return {
            /**
             * Format string from array
             * @memberof mdResourceService
             * @param {any} resourceFileName
             * @param {any} replaceNodeNames
             */
            formatStringFromArray: function (resourceFileName, replaceNodeNames) {
                var text = '';
                if (resourceFileName !== undefined) {
                    var resourceKey = undefined;
                    if (resourceKey === undefined && resourceFileName.split('.').length == 2) {
                        resourceKey = resourceFileName.split('.')[1];
                        resourceFileName = resourceFileName.split('.')[0];
                    }
                    if (resourceFileName != '' && resourceKey !== undefined && resourceKey != '') {
                        text = mdBusinessLogic.globals.resources[resourceFileName][resourceKey];
                        for (var i in replaceNodeNames) {
                            var node = replaceNodeNames[i];
                            var textToReplace = (node.value === undefined || node.value == null) ? '' : node.value;
                            if (textToReplace.split('.').length == 2) {
                                var textToReplaceDictionary = textToReplace.split('.')[0];
                                var textToReplaceKey = textToReplace.split('.')[1];
                                if (textToReplaceDictionary !== undefined &&
                                    textToReplaceDictionary != '' &&
                                    mdBusinessLogic.globals.resources[textToReplaceDictionary] !== undefined &&
                                    textToReplaceKey !== undefined &&
                                    textToReplaceKey != '' &&
                                    mdBusinessLogic.globals.resources[textToReplaceDictionary][textToReplaceKey] !== undefined) {
                                    textToReplace = mdBusinessLogic.globals.resources[textToReplaceDictionary][textToReplaceKey];
                                }
                            }
                            text = text.replace('{' + node.index + '}', textToReplace);
                        }
                        text = text.trim();
                    }
                }
                return text;
            },
            /**
             * Format string
             * @memberof mdResourceService
             * @param {any} resourceFileName
             * @param {any} replacevalue0
             * @param {any} replacevalue1
             * @param {any} replacevalue2
             * @param {any} replacevalue3
             * @param {any} replacevalue4
             * @param {any} replacevalue5
             * @param {any} replacevalue6
             * @param {any} replacevalue7
             * @param {any} replacevalue8
             * @param {any} replacevalue9
             * @param {any} replacevalue10
             * @param {any} replacevalue11
             * @param {any} replacevalue12
             * @param {any} replacevalue13
             * @param {any} replacevalue14
             * @param {any} replacevalue15
             */
            formatString: function (resourceFileName, 
                replacevalue0, 
                replacevalue1, 
                replacevalue2, 
                replacevalue3,
                replacevalue4,
                replacevalue5,
                replacevalue6,
                replacevalue7,
                replacevalue8,
                replacevalue9,
                replacevalue10,
                replacevalue11,
                replacevalue12,
                replacevalue13,
                replacevalue14,
                replacevalue15) {
                return this.formatStringFromArray(resourceFileName,
                    [
                        {
                            'index': 0,
                            'value': loadDefault(replacevalue0, '')
                        },
                        {
                            'index': 1,
                            'value': loadDefault(replacevalue1, '')
                        },
                        {
                            'index': 2,
                            'value': loadDefault(replacevalue2, '')
                        },
                        {
                            'index': 3,
                            'value': loadDefault(replacevalue3, '')
                        },
                        {
                            'index': 4,
                            'value': loadDefault(replacevalue4, '')
                        },
                        {
                            'index': 5,
                            'value': loadDefault(replacevalue5, '')
                        },
                        {
                            'index': 6,
                            'value': loadDefault(replacevalue6, '')
                        },
                        {
                            'index': 7,
                            'value': loadDefault(replacevalue7, '')
                        },
                        {
                            'index': 8,
                            'value': loadDefault(replacevalue8, '')
                        },
                        {
                            'index': 9,
                            'value': loadDefault(replacevalue9, '')
                        },
                        {
                            'index': 10,
                            'value': loadDefault(replacevalue10, '')
                        },
                        {
                            'index': 11,
                            'value': loadDefault(replacevalue11, '')
                        },
                        {
                            'index': 12,
                            'value': loadDefault(replacevalue12, '')
                        },
                        {
                            'index': 13,
                            'value': loadDefault(replacevalue13, '')
                        },
                        {
                            'index': 14,
                            'value': loadDefault(replacevalue14, '')
                        },
                        {
                            'index': 15,
                            'value': loadDefault(replacevalue15, '')
                        }
                    ]);
            }
        };
    }
}());