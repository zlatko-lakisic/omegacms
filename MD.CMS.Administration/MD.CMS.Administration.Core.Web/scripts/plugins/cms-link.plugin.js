/**
 * plugin.js
 *
 * Released under LGPL License.
 * Copyright (c) 1999-2015 Ephox Corp. All rights reserved
 *
 * License: http://www.tinymce.com/license
 * Contributing: http://www.tinymce.com/contributing
 */

/*global tinymce:true */

tinymce.PluginManager.add('cmslink', function (editor) {
    function createLinkList(callback) {
        return function () {
            var linkList = editor.settings.link_list;

            if (typeof linkList == "string") {
                tinymce.util.XHR.send({
                    url: linkList,
                    success: function (text) {
                        callback(tinymce.util.JSON.parse(text));
                    }
                });
            } else if (typeof linkList == "function") {
                linkList(callback);
            } else {
                callback(linkList);
            }
        };
    }

    function buildListItems(inputList, itemCallback, startItems) {
        function appendItems(values, output) {
            output = output || [];

            tinymce.each(values, function (item) {
                var menuItem = { text: item.text || item.title };

                if (item.menu) {
                    menuItem.menu = appendItems(item.menu);
                } else {
                    menuItem.value = item.value;

                    if (itemCallback) {
                        itemCallback(menuItem);
                    }
                }

                output.push(menuItem);
            });

            return output;
        }

        return appendItems(inputList, startItems || []);
    }

    function buildMediaContentList(inputList, callback) {
        function appendItems(values, output) {
            output = output || [];

            tinymce.each(values, function (item) {
                var gridItem = {};
                gridItem.type = "control";
                gridItem.title = item.Name;
                gridItem.description = item.Description;
                gridItem.filetype = item.FileType;
                gridItem.value = mdBusinessLogic.settings.appBase + "/uploads/" + item.FullNameFile;
                gridItem.minHeight = 160;
                gridItem.onclick = function (e) {
                    callback(gridItem);
                }
                gridItem.mime = guessMime(gridItem.value);
                gridItem.width = 300;
                gridItem.height = 150;
                gridItem.onPostRender = function () {
                    this.innerHtml("<div style='border:2px solid black;height:160px;position:relative;' class='cms-media-container'>" +
                                    dataToHtml(gridItem) +
                                    "<span style='position:absolute;top:0;left:0;width:100%;height:100%;background:url(data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7);cursor:default'></span>" +
                                    "<p style='position:absolute;left:0;bottom:0;right:0;color:white;padding:5px;background-color:rgba(0,0,0,0.8);'>" + gridItem.title + "</p>" +
                                    "</div>");
                }

                output.push(gridItem);
            });
            return output;
        }
        return appendItems(inputList, []);
    }

    function buildContentList(inputList, callback) {
        function appendItems(values, output) {
            output = output || [];

            tinymce.each(values, function (item) {
                var gridItem = {};
                gridItem.type = "control";
                gridItem.title = item.Title;
                gridItem.value = item.ContentAliases.length ? "/" + item.ContentAliases[0] : null;
                gridItem.minHeight = 160;
                gridItem.onclick = function (e) {
                    callback(gridItem);
                }
                gridItem.width = 300;
                gridItem.height = 150;
                gridItem.onPostRender = function () {
                    this.innerHtml("<div style='border:2px solid black;height:160px;position:relative;' class='cms-media-container'>" +
                                   "<p style='font-size:25px;overflow:hidden;text-overflow:ellipsis;object-fit:contain;max-width:95%;max-height:95%;position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);'>" + (gridItem.value || "No alias") + "</p>" +
                                    "<span style='position:absolute;top:0;left:0;width:100%;height:100%;background:url(data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7);cursor:default'></span>" +
                                    "</div>");
                }

                output.push(gridItem);
            });
            return output;
        }
        return appendItems(inputList, []);
    }

    function addEvents() {
        $('.cms-link-container').click(function () {
            var self = this;
            $('.cms-link-selected').removeClass('cms-link-selected');
            $(self).addClass('cms-link-selected');
            setTimeout(function () { $(self).css('border', '2px solid black') }, 150);
        });
    }

    function dataToHtml(data) {
        switch (data.filetype) {
            case 1:
                return "<img class=\"media\" src=\"" + data.value + "\" />";
            case 2:
                return "<video class=\"media\" width=\"" + data.width + "\" height=\"" + data.height + "\"><source src='" + data.value + "' type=\"" + data.mime + "\"></source></video>";
            case 3:
                return "<p class=\"label\">AUDIO</p>";
            case 4:
                return "<p class=\"label\">PDF</p>";
        }
    }

    function guessMime(url) {
        url = url.toLowerCase();

        if (url.indexOf('.mp3') != -1) {
            return 'audio/mpeg';
        }

        if (url.indexOf('.wav') != -1) {
            return 'audio/wav';
        }
        
        if (url.indexOf('.m4a') != -1) {
            return 'audio/m4a';
        }

        if (url.indexOf('.mp4') != -1) {
            return 'video/mp4';
        }

        if (url.indexOf('.webm') != -1) {
            return 'video/webm';
        }

        if (url.indexOf('.ogg') != -1) {
            return 'video/ogg';
        }

        if (url.indexOf('.swf') != -1) {
            return 'application/x-shockwave-flash';
        }

        return '';
    }

    function showDialog(linkList) {
        var data = {}, selection = editor.selection, dom = editor.dom, selectedElm, anchorElm, initialText;
        var win, onlyText, textListCtrl, linkListCtrl, relListCtrl, targetListCtrl, classListCtrl, linkTitleCtrl, value, mediaContentCtrl, contentCtrl;

        var refreshCallback = editor.settings.cmslink.refreshmedia;
        if (refreshCallback && typeof refreshCallback == 'function') {
            refreshCallback(refreshMediaContentList);
        }
        refreshCallback = editor.settings.cmslink.refreshcontent;
        if (refreshCallback && typeof refreshCallback == 'function') {
            refreshCallback(refreshContentList);
        }

        function linkListChangeHandler(e) {
            var textCtrl = win.find('#text');

            if (!textCtrl.value() || (e.lastControl && textCtrl.value() == e.lastControl.text())) {
                textCtrl.value(e.control.text());
            }

            win.find('#href').value(e.control.value());
        }

        function buildAnchorListControl(url) {
            var anchorList = [];

            tinymce.each(editor.dom.select('a:not([href])'), function (anchor) {
                var id = anchor.name || anchor.id;

                if (id) {
                    anchorList.push({
                        text: id,
                        value: '#' + id,
                        selected: url.indexOf('#' + id) != -1
                    });
                }
            });

            if (anchorList.length) {
                anchorList.unshift({ text: 'None', value: '' });

                return {
                    name: 'anchor',
                    type: 'listbox',
                    label: 'Anchors',
                    values: anchorList,
                    onselect: linkListChangeHandler
                };
            }
        }

        function selectContent(item) {
            win.find("#href").value(item.value);
        }

        function refreshMediaContentList(items) {
            mediaContentCtrl.items().remove();
            mediaContentCtrl.add(buildMediaContentList(items, selectContent));
            mediaContentCtrl.renderNew();
            addEvents();
        }

        function refreshContentList(items) {
            contentCtrl.items().remove();
            contentCtrl.add(buildContentList(items, selectContent));
            contentCtrl.renderNew();
            addEvents();
        }

        function updateText() {
            if (!initialText && data.text.length === 0 && onlyText) {
                this.parent().parent().find('#text')[0].value(this.value());
            }
        }

        function urlChange(e) {
            var meta = e.meta || {};

            if (linkListCtrl) {
                linkListCtrl.value(editor.convertURL(this.value(), 'href'));
            }

            tinymce.each(e.meta, function (value, key) {
                win.find('#' + key).value(value);
            });

            if (!meta.text) {
                updateText.call(this);
            }
        }

        function isOnlyTextSelected(anchorElm) {
            var html = selection.getContent();

            // Partial html and not a fully selected anchor element
            if (/</.test(html) && (!/^<a [^>]+>[^<]+<\/a>$/.test(html) || html.indexOf('href=') == -1)) {
                return false;
            }

            if (anchorElm) {
                var nodes = anchorElm.childNodes, i;

                if (nodes.length === 0) {
                    return false;
                }

                for (i = nodes.length - 1; i >= 0; i--) {
                    if (nodes[i].nodeType != 3) {
                        return false;
                    }
                }
            }

            return true;
        }

        selectedElm = selection.getNode();
        anchorElm = dom.getParent(selectedElm, 'a[href]');
        onlyText = isOnlyTextSelected();

        data.text = initialText = anchorElm ? (anchorElm.innerText || anchorElm.textContent) : selection.getContent({ format: 'text' });
        data.href = anchorElm ? dom.getAttrib(anchorElm, 'href') : '';

        if (anchorElm) {
            data.target = dom.getAttrib(anchorElm, 'target');
        } else if (editor.settings.default_link_target) {
            data.target = editor.settings.default_link_target;
        }

        if ((value = dom.getAttrib(anchorElm, 'rel'))) {
            data.rel = value;
        }

        if ((value = dom.getAttrib(anchorElm, 'class'))) {
            data['class'] = value;
        }

        if ((value = dom.getAttrib(anchorElm, 'title'))) {
            data.title = value;
        }

        if (onlyText) {
            textListCtrl = {
                name: 'text',
                type: 'textbox',
                size: 40,
                label: 'Text to display',
                onchange: function () {
                    data.text = this.value();
                }
            };
        }

        if (linkList) {
            linkListCtrl = {
                type: 'listbox',
                label: 'Link list',
                values: buildListItems(
					linkList,
					function (item) {
					    item.value = editor.convertURL(item.value || item.url, 'href');
					},
					[{ text: 'None', value: '' }]
				),
                onselect: linkListChangeHandler,
                value: editor.convertURL(data.href, 'href'),
                onPostRender: function () {
                    /*eslint consistent-this:0*/
                    linkListCtrl = this;
                }
            };
        }

        if (editor.settings.target_list !== false) {
            if (!editor.settings.target_list) {
                editor.settings.target_list = [
					{ text: 'None', value: '' },
					{ text: 'New window', value: '_blank' }
                ];
            }

            targetListCtrl = {
                name: 'target',
                type: 'listbox',
                label: 'Target',
                values: buildListItems(editor.settings.target_list)
            };
        }

        if (editor.settings.rel_list) {
            relListCtrl = {
                name: 'rel',
                type: 'listbox',
                label: 'Rel',
                values: buildListItems(editor.settings.rel_list)
            };
        }

        if (editor.settings.link_class_list) {
            classListCtrl = {
                name: 'class',
                type: 'listbox',
                label: 'Class',
                values: buildListItems(
					editor.settings.link_class_list,
					function (item) {
					    if (item.value) {
					        item.textStyle = function () {
					            return editor.formatter.getCssText({ inline: 'a', classes: [item.value] });
					        };
					    }
					}
				)
            };
        }

        if (editor.settings.cmslink.mediacontent_list) {
            mediaContentCtrl = {
                type: 'form',
                layout: 'grid',
                columns: 2,
                items: buildMediaContentList(editor.settings.cmslink.mediacontent_list, selectContent),
                maxHeight: Math.min(tinymce.DOM.getViewPort().h - 260, 400),
                minHeight: Math.min(tinymce.DOM.getViewPort().h - 260, 400),
                onPostRender: function () {
                    mediaContentCtrl = this;
                    addEvents();
                    $("#" + this._id + "-body").css("overflow-y", "auto");
                }
            };
            contentCtrl = {
                type: 'form',
                layout: 'grid',
                columns: 2,
                items: buildMediaContentList(editor.settings.cmslink.mediacontent_list, selectContent),
                maxHeight: Math.min(tinymce.DOM.getViewPort().h - 260, 400),
                minHeight: Math.min(tinymce.DOM.getViewPort().h - 260, 400),
                onPostRender: function () {
                    contentCtrl = this;
                    addEvents();
                    $("#" + this._id + "-body").css("overflow-y", "auto");
                }
            };
        }

        if (editor.settings.link_title !== false) {
            linkTitleCtrl = {
                name: 'title',
                type: 'textbox',
                label: 'Title',
                value: data.title
            };
        }

        win = editor.windowManager.open({
            title: 'Insert link',
            data: data,
            bodyType: 'tabpanel',
            body: [
                    {
                        title: 'General',
                        type: 'form',
                        items: [{
                            title: 'General',
                            name: 'href',
                            type: 'filepicker',
                            filetype: 'file',
                            size: 40,
                            autofocus: true,
                            label: 'Url',
                            onchange: urlChange,
                            onkeyup: updateText
                        },
				    textListCtrl,
				    linkTitleCtrl,
				    buildAnchorListControl(data.href),
				    linkListCtrl,
				    relListCtrl,
				    targetListCtrl,
				    classListCtrl]
                    },
                    {
                        title: 'Media Contents',
                        type: 'form',
                        items: [
                            {
                                type: 'textbox',
                                label: 'Search',
                                onsubmit: function (e) {
                                    var callback = editor.settings.cmslink.searchmedia;
                                    if (callback && typeof callback == 'function') {
                                        callback($(this.$el).val(), refreshMediaContentList);
                                    } else {
                                        console.log("There is no search callback function");
                                    }
                                    return false;
                                }
                            },
                            mediaContentCtrl
                        ]
                    },
                    {
                        title: 'Contents',
                        type: 'form',
                        items: [
                            {
                                type: 'textbox',
                                label: 'Search',
                                onsubmit: function (e) {
                                    var callback = editor.settings.cmslink.searchcontent;
                                    if (callback && typeof callback == 'function') {
                                        callback($(this.$el).val(), refreshContentList);
                                    } else {
                                        console.log("There is no search callback function");
                                    }
                                    return false;
                                }
                            },
                            contentCtrl
                        ]
                    }
            ],
            onSubmit: function (e) {
                /*eslint dot-notation: 0*/
                var href;

                data = tinymce.extend(data, e.data);
                href = data.href;

                // Delay confirm since onSubmit will move focus
                function delayedConfirm(message, callback) {
                    var rng = editor.selection.getRng();

                    tinymce.util.Delay.setEditorTimeout(editor, function () {
                        editor.windowManager.confirm(message, function (state) {
                            editor.selection.setRng(rng);
                            callback(state);
                        });
                    });
                }

                function insertLink() {
                    var linkAttrs = {
                        href: href,
                        target: data.target ? data.target : null,
                        rel: data.rel ? data.rel : null,
                        "class": data["class"] ? data["class"] : null,
                        title: data.title ? data.title : null
                    };

                    if (anchorElm) {
                        editor.focus();

                        if (onlyText && data.text != initialText) {
                            if ("innerText" in anchorElm) {
                                anchorElm.innerText = data.text;
                            } else {
                                anchorElm.textContent = data.text;
                            }
                        }

                        dom.setAttribs(anchorElm, linkAttrs);

                        selection.select(anchorElm);
                        editor.undoManager.add();
                    } else {
                        if (onlyText) {
                            editor.insertContent(dom.createHTML('a', linkAttrs, dom.encode(data.text)));
                        } else {
                            editor.execCommand('mceInsertLink', false, linkAttrs);
                        }
                    }
                }

                if (!href) {
                    editor.execCommand('unlink');
                    return;
                }

                // Is email and not //user@domain.com
                if (href.indexOf('@') > 0 && href.indexOf('//') == -1 && href.indexOf('mailto:') == -1) {
                    delayedConfirm(
						'The URL you entered seems to be an email address. Do you want to add the required mailto: prefix?',
						function (state) {
						    if (state) {
						        href = 'mailto:' + href;
						    }

						    insertLink();
						}
					);

                    return;
                }

                // Is not protocol prefixed
                if ((editor.settings.link_assume_external_targets && !/^\w+:/i.test(href)) ||
					(!editor.settings.link_assume_external_targets && /^\s*www[\.|\d\.]/i.test(href))) {
                    delayedConfirm(
						'The URL you entered seems to be an external link. Do you want to add the required http:// prefix?',
						function (state) {
						    if (state) {
						        href = 'http://' + href;
						    }

						    insertLink();
						}
					);

                    return;
                }

                insertLink();
            }
        });
    }

    editor.addButton('cmslink', {
        icon: 'link',
        tooltip: 'Insert/edit link',
        shortcut: 'Meta+K',
        onclick: createLinkList(showDialog),
        stateSelector: 'a[href]'
    });

    editor.addShortcut('Meta+K', '', createLinkList(showDialog));
    editor.addCommand('mceLink', createLinkList(showDialog));

    this.showDialog = showDialog;

    editor.addMenuItem('cmslink', {
        icon: 'link',
        text: 'Insert/edit link',
        shortcut: 'Meta+K',
        onclick: createLinkList(showDialog),
        stateSelector: 'a[href]',
        context: 'insert',
        prependToContext: true
    });
});
