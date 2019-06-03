/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */
var HtmlUtil = {
    /*1.用正则表达式实现html转码*/
    htmlEncodeByRegExp: function (op) {
        if (op != null) {
            var s = "";
            if (op.length == 0) return "";
            s = op.replace(/&/g, "&amp;");
            s = s.replace(/</g, "&lt;");
            s = s.replace(/>/g, "&gt;");
            s = s.replace(/ /g, "&nbsp;");
            s = s.replace(/\'/g, "&#39;");
            s = s.replace(/\"/g, "&quot;");
            return s;
        } else {
            return "";
        }
    },
    /*2.用正则表达式实现html解码*/
    htmlDecodeByRegExp: function (op) {
        if (op != null) {
            var s = "";
            if (op.length == 0) return "";
            s = op.replace(/&amp;/g, "&");
            s = s.replace(/&lt;/g, "<");
            s = s.replace(/&gt;/g, ">");
            s = s.replace(/&nbsp;/g, " ");
            s = s.replace(/&#39;/g, "\'");
            s = s.replace(/&quot;/g, "\"");
            return s;
        } else {
            return "";
        }
    }
};

CKEDITOR.editorConfig = function( config ) {
    config.toolbar = 'MyToolbar'; 

    config.toolbar_MyToolbar =
    [
     { name: 'document', items: ['Source', '-', 'Save', 'NewPage', 'DocProps', 'Preview', 'Print', '-', 'Templates'] },
    { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
    { name: 'editing', items: ['Find', 'Replace', '-', 'SelectAll', '-', 'SpellChecker', 'Scayt'] },
    { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'RemoveFormat', 'TextColor', 'BGColor'] },
    {
        name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv',
        '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl']
    },
    { name: 'links', items: ['Link', 'Unlink', 'Anchor'] },
    { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
    ];
};
