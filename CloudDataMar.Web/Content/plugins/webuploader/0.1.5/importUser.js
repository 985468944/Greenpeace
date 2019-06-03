(function ($) {
    // 当domReady的时候开始初始化
    $(function () {
        // 检测是否已经安装flash,检测flash的版本
        var flashVersion = (function () {
            var version;

            try {
                version = navigator.plugins['Shockwave Flash'];
                version = version.description;
            } catch (ex) {
                try {
                    version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash')
                            .GetVariable('$version');
                } catch (ex2) {
                    version = '0.0';
                }
            }
            version = version.match(/\d+/g);
            return parseFloat(version[0] + '.' + version[1], 10);
        })(),
        // WebUploader实例
        uploader;
        if (!WebUploader.Uploader.support('flash') && WebUploader.browser.ie) {

            // flash 安装了但是版本过低。
            if (flashVersion) {
                (function (container) {
                    window['expressinstallcallback'] = function (state) {
                        switch (state) {
                            case 'Download.Cancelled':
                                alert('您取消了更新.')
                                break;

                            case 'Download.Failed':
                                alert('安装失败')
                                break;

                            default:
                                alert('安装已成功,请刷新.');
                                break;
                        }
                        delete window['expressinstallcallback'];
                    };

                    var swf = './expressInstall.swf';
                    // insert flash object
                    var html = '<object type="application/' +
                            'x-shockwave-flash" data="' + swf + '" ';

                    if (WebUploader.browser.ie) {
                        html += 'classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" ';
                    }

                    html += 'width="100%" height="100%" style="outline:0">' +
                        '<param name="movie" value="' + swf + '" />' +
                        '<param name="wmode" value="transparent" />' +
                        '<param name="allowscriptaccess" value="always" />' +
                    '</object>';

                    container.html(html);

                })($wrap);

                // 压根就没有安转。
            } else {
                //$wrap.html('<a href="http://www.adobe.com/go/getflashplayer" target="_blank" border="0"><img alt="get flash player" src="http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg" /></a>');
            }

            //return;
        } else if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器.');
            return;
        }

        // 实例化
        uploader = WebUploader.create({
            pick: '#importUser',// 选择文件的按钮。可选。
            auto: true,// 选完文件后，是否自动上传。
            swf: './Uploader.swf',
            chunked: false,
            chunkSize: 512 * 1024,
            server: '/Userinfo/ImportUser',
            // runtimeOrder: 'flash',

            accept: {
                title: 'Excel',
                extensions: 'xls,xlsx',
                mimeTypes: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            },

            fileNumLimit: 2,
            fileSizeLimit: 200 * 1024 * 1024,    // 200 M
            fileSingleSizeLimit: 200 * 1024 * 1024    // 200 M
        });
        uploader.on('ready', function () {
            window.uploader = uploader;
        });
        var result = null;
        uploader.on('uploadAccept', function (file, response) {
            result = response;
            if (response.handler) {
                return false;
            }
        });

        // 文件上传成功，给item添加成功class, 用样式标记上传成功。
        uploader.on('error', function (type) {
            $('#UploadTip').modal('hide');
            switch(type){
                case "Q_EXCEED_NUM_LIMIT"://设置了fileNumLimit且尝试给uploader添加的文件数量超出这个值时派送
                    alert("超过上传数量上限!"); break;
                case "Q_EXCEED_SIZE_LIMIT"://在设置了Q_EXCEED_SIZE_LIMIT且尝试给uploader添加的文件总大小超出这个值时派送。
                    alert("文件总大小超出限制!"); break;
                case "Q_TYPE_DENIED"://当文件类型不满足时触发。。
                    alert("文件不是Excel类型!"); break;
            }
            uploader.reset();
        });
        //文件上传成功，给item添加成功class, 用样式标记上传成功。
        uploader.on('uploadSuccess', function (file) {
            $('#UploadTip').modal('hide');
            $('#mySuccessAlert .modal-body').empty();
            $('#mySuccessAlert .modal-body').append("更新人员信息完成!");
            $('#mySuccessAlert .modal-header').empty();
            $('#mySuccessAlert .modal-header').append("Success");
            $('#mySuccessAlert').modal('show');           
        });

        // 文件上传失败，显示上传出错。
        uploader.on('uploadError', function (file) {
            $('#UploadTip').modal('hide');
            if (result.handler) {
                if (result.error != "") {
                    $('#myDangerAlert .modal-body').empty();
                    $('#myDangerAlert .modal-body').append("<textarea style=\"width:100%;height:188px;overflow-y:auto\">" + result.error + "</textarea>");
                    $('#myDangerAlert .modal-header').empty();
                    $('#myDangerAlert .modal-header').append("<p>人员信息部分更新失败，具体请看提示.</p>");
                    $('#myDangerAlert').modal('show');
                }
                else {
                    $('#mySuccessAlert .modal-body').empty();
                    $('#mySuccessAlert .modal-body').append("更新人员信息完成!");
                    $('#mySuccessAlert .modal-header').empty();
                    $('#mySuccessAlert .modal-header').append("Success");
                    $('#mySuccessAlert').modal('show');                   
                }
            }
            //alert("人员信息更新异常，请联系管理员.");
        });
        uploader.on('uploadFinished', function () {
            $('#UploadTip').modal('hide');
            uploader.reset();
        });
        uploader.on('uploadProgress', function () {
            $('#UploadTip').modal('show');
        })
    });

})(jQuery);