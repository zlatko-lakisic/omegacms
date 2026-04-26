
/**
 * This module defines custom JDOC tags for documenting angularjs.
 */

'use strict';

const directiveTag = require('./lib/directive');
const serviceTag = require('./lib/service');
const filterTag = require('./lib/filter');

exports.defineTags = function (dictionary) {
    dictionary.defineTag(directiveTag.name, directiveTag.options);
    dictionary.defineTag(serviceTag.name, serviceTag.options);
    dictionary.defineTag(filterTag.name, filterTag.options);
};

exports.handlers = {
    newDoclet: function (e) {
        directiveTag.newDocletHandler(e);
        serviceTag.newDocletHandler(e);
        filterTag.newDocletHandler(e);
    }
}