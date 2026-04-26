/**
 * This module defines a custom jsDoc tag.
 * It allows you to document header parameters of a service.
 */

'use strict';

exports.name = 'service';
exports.options = {
    canHaveType: true,
    canHaveName: true,
    onTagged: function (doclet, tag) {
        doclet.service = {
            'name': tag.value.name,
            'type': tag.value.type ? (tag.value.type.names.length === 1 ? tag.value.type.names[0] : tag.value.type.names) : '',
        };
    },
}
exports.newDocletHandler = function (e) {
    var service = e.doclet.service;
    if (service) {
        e.doclet.kind = 'class';
        e.doclet.scope = 'service';
        e.doclet.description = '';
    }
}