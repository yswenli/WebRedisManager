/**

 layui瀹樼綉

*/

layui.define(['code', 'element', 'table', 'util'], function (exports) {
    var $ = layui.jquery
        , element = layui.element
        , layer = layui.layer
        , form = layui.form
        , util = layui.util
        , device = layui.device()

        , $win = $(window), $body = $('body');


    //闃绘IE7浠ヤ笅璁块棶
    if (device.ie && device.ie < 8) {
        layer.alert('Layui鏈€浣庢敮鎸乮e8锛屾偍褰撳墠浣跨敤鐨勬槸鍙よ€佺殑 IE' + device.ie + '锛屼綘涓殑鑲畾涓嶆槸绋嬪簭鐚匡紒');
    }

    var home = $('#LAY_home');


    layer.ready(function () {
        var local = layui.data('layui');

        //鍗囩骇鎻愮ず
        if (local.version && local.version !== layui.v) {
            layer.open({
                type: 1
                , title: '鏇存柊鎻愮ず' //涓嶆樉绀烘爣棰樻爮
                , closeBtn: false
                , area: '300px;'
                , shade: false
                , offset: 'b'
                , id: 'LAY_updateNotice' //璁惧畾涓€涓猧d锛岄槻姝㈤噸澶嶅脊鍑�
                , btn: ['鏇存柊鏃ュ織', '鏈曚笉鎯冲崌']
                , btnAlign: 'c'
                , moveType: 1 //鎷栨嫿妯″紡锛�0鎴栬€�1
                , content: ['<div class="layui-text">'
                    , 'layui 宸叉洿鏂板埌锛�<strong style="padding-right: 10px; color: #fff;">v' + layui.v + '</strong> <br>璇锋敞鎰忓崌绾э紒'
                    , '</div>'].join('')
                , skin: 'layui-layer-notice'
                , yes: function (index) {
                    layer.close(index);
                    setTimeout(function () {
                        location.href = '/doc/base/changelog.html';
                    }, 500);
                }
                , end: function () {
                    layui.data('layui', {
                        key: 'version'
                        , value: layui.v
                    });
                }
            });
        }
        layui.data('layui', {
            key: 'version'
            , value: layui.v
        });


        //鍏憡
        ; !function () {
            return layui.data('layui', {
                key: 'notice_20180530'
                , remove: true
            });

            if (local.notice_20180530 && new Date().getTime() - local.notice_20180530 < 1000 * 60 * 60 * 24 * 5) {
                return;
            };

            layer.open({
                type: 1
                , title: 'layui 瀹樻柟閫氱敤鍚庡彴绠＄悊妯℃澘'
                , closeBtn: false
                , area: ['300px', '280px']
                , shade: false
                //,offset: 'c'
                , id: 'LAY_Notice' //璁惧畾涓€涓猧d锛岄槻姝㈤噸澶嶅脊鍑�
                , btn: ['鍓嶅線鍥磋', '鏈曚笉鎯崇湅']
                , btnAlign: 'b'
                , moveType: 1 //鎷栨嫿妯″紡锛�0鎴栬€�1
                , resize: false
                , content: ['<div style="padding: 15px; text-align: center; background-color: #e2e2e2;">'
                    , '<a href="/admin/std/dist/views/" target="_blank"><img src="//cdn.layui.com/upload/2018_5/168_1527691799254_76462.jpg" alt="layuiAdmin" style="width: 100%; height:149.78px;"></a>'
                    , '</div>'].join('')
                , success: function (layero, index) {
                    var btn = layero.find('.layui-layer-btn');
                    btn.find('.layui-layer-btn0').attr({
                        href: '/admin/std/dist/views/'
                        , target: '_blank'
                    });

                    layero.find('a').on('click', function () {
                        layer.close(index);
                    });
                }
                , end: function () {
                    layui.data('layui', {
                        key: 'notice_20180530'
                        , value: new Date().getTime()
                    });
                }
            });
        }();

    });


    //鐐瑰嚮浜嬩欢
    var events = {
        //鑱旂郴鏂瑰紡
        contactInfo: function () {
            layer.alert('<div class="layui-text">濡傛湁鍚堜綔鎰忓悜锛屽彲鑱旂郴锛�<br>閭锛歺ianxin@layui-inc.com</div>', {
                title: '鑱旂郴'
                , btn: false
                , shadeClose: true
            });
        }
    }

    $body.on('click', '*[site-event]', function () {
        var othis = $(this)
            , attrEvent = othis.attr('site-event');
        events[attrEvent] && events[attrEvent].call(this, othis);
    });


    //鎼滅储缁勪欢
    form.on('select(component)', function (data) {
        var value = data.value;
        location.href = '/doc/' + value;
    });

    //鍒囨崲鐗堟湰
    form.on('select(tabVersion)', function (data) {
        var value = data.value;
        location.href = value === 'new' ? '/' : ('/' + value + '/doc/');
    });


    //棣栭〉banner
    setTimeout(function () {
        $('.site-zfj').addClass('site-zfj-anim');
        setTimeout(function () {
            $('.site-desc').addClass('site-desc-anim')
        }, 5000)
    }, 100);


    //鏁板瓧鍓嶇疆琛ラ浂
    var digit = function (num, length, end) {
        var str = '';
        num = String(num);
        length = length || 2;
        for (var i = num.length; i < length; i++) {
            str += '0';
        }
        return num < Math.pow(10, length) ? str + (num | 0) : num;
    };


    //涓嬭浇鍊掕鏃�
    var setCountdown = $('#setCountdown');
    if ($('#setCountdown')[0]) {
        $.get('/api/getTime', function (res) {
            util.countdown(new Date(2017, 7, 21, 8, 30, 0), new Date(res.time), function (date, serverTime, timer) {
                var str = digit(date[1]) + ':' + digit(date[2]) + ':' + digit(date[3]);
                setCountdown.children('span').html(str);
            });
        }, 'jsonp');
    }



    for (var i = 0; i < $('.adsbygoogle').length; i++) {
        (adsbygoogle = window.adsbygoogle || []).push({});
    }


    //灞曠ず褰撳墠鐗堟湰
    $('.site-showv').html(layui.v);

    //鑾峰彇涓嬭浇鏁�
    $.get('//fly.layui.com/api/handle?id=10&type=find', function (res) {
        $('.site-showdowns').html(res.number);
    }, 'jsonp');

    //璁板綍涓嬭浇
    $('.site-down').on('click', function () {
        $.get('//fly.layui.com/api/handle?id=10', function () { }, 'jsonp');
    });

    //鑾峰彇Github鏁版嵁
    var getStars = $('#getStars');
    if (getStars[0]) {
        $.get('https://api.github.com/repos/sentsin/layui', function (res) {
            getStars.html(res.stargazers_count);
        }, 'json');
    }

    //鍥哄畾Bar
    if (global.pageType !== 'demo') {
        util.fixbar({
            bar1: true
            , click: function (type) {
                if (type === 'bar1') {
                    location.href = '//fly.layui.com/';
                }
            }
        });
    }

    //绐楀彛scroll
    ; !function () {
        var main = $('.site-tree').parent(), scroll = function () {
            var stop = $(window).scrollTop();

            if ($(window).width() <= 750) return;
            var bottom = $('.footer').offset().top - $(window).height();
            if (stop > 61 && stop < bottom) {
                if (!main.hasClass('site-fix')) {
                    main.addClass('site-fix');
                }
                if (main.hasClass('site-fix-footer')) {
                    main.removeClass('site-fix-footer');
                }
            } else if (stop >= bottom) {
                if (!main.hasClass('site-fix-footer')) {
                    main.addClass('site-fix site-fix-footer');
                }
            } else {
                if (main.hasClass('site-fix')) {
                    main.removeClass('site-fix').removeClass('site-fix-footer');
                }
            }
            stop = null;
        };
        scroll();
        $(window).on('scroll', scroll);
    }();

    //绀轰緥椤甸潰婊氬姩
    $('.site-demo-body').on('scroll', function () {
        var elemDate = $('.layui-laydate')
            , elemTips = $('.layui-table-tips');
        if (elemDate[0]) {
            elemDate.each(function () {
                var othis = $(this);
                if (!othis.hasClass('layui-laydate-static')) {
                    othis.remove();
                }
            });
            $('input').blur();
        }
        if (elemTips[0]) elemTips.remove();

        if ($('.layui-layer')[0]) {
            layer.closeAll('tips');
        }
    });

    //浠ｇ爜淇グ
    layui.code({
        elem: 'pre'
    });

    //鐩綍
    var siteDir = $('.site-dir');
    if (siteDir[0] && $(window).width() > 750) {
        layer.ready(function () {
            layer.open({
                type: 1
                , content: siteDir
                , skin: 'layui-layer-dir'
                , area: 'auto'
                , maxHeight: $(window).height() - 300
                , title: '鐩綍'
                //,closeBtn: false
                , offset: 'r'
                , shade: false
                , success: function (layero, index) {
                    layer.style(index, {
                        marginLeft: -15
                    });
                }
            });
        });
        siteDir.find('li').on('click', function () {
            var othis = $(this);
            othis.find('a').addClass('layui-this');
            othis.siblings().find('a').removeClass('layui-this');
        });
    }

    //鍦╰extarea鐒︾偣澶勬彃鍏ュ瓧绗�
    var focusInsert = function (str) {
        var start = this.selectionStart
            , end = this.selectionEnd
            , offset = start + str.length

        this.value = this.value.substring(0, start) + str + this.value.substring(end);
        this.setSelectionRange(offset, offset);
    };

    //婕旂ず椤甸潰
    $('body').on('keydown', '#LAY_editor, .site-demo-text', function (e) {
        var key = e.keyCode;
        if (key === 9 && window.getSelection) {
            e.preventDefault();
            focusInsert.call(this, '  ');
        }
    });

    var editor = $('#LAY_editor')
        , iframeElem = $('#LAY_demo')
        , demoForm = $('#LAY_demoForm')[0]
        , demoCodes = $('#LAY_demoCodes')[0]
        , runCodes = function () {
            if (!iframeElem[0]) return;
            var html = editor.val();

            html = html.replace(/=/gi, "layequalsign");
            html = html.replace(/script/gi, "layscrlayipttag");
            demoCodes.value = html.length > 100 * 1000 ? '<h1>鍗фЫ锛屼綘鐨勪唬鐮佽繃闀�</h1>' : html;

            demoForm.action = '/api/runHtml/';
            demoForm.submit();

        };
    $('#LAY_demo_run').on('click', runCodes), runCodes();

    //璁╁鑸湪鏈€浣充綅缃�
    var thisItem = $('.site-demo-nav').find('dd.layui-this');
    if (thisItem[0]) {
        var itemTop = thisItem.offset().top
            , winHeight = $(window).height()
            , elemScroll = $('.layui-side-scroll');
        if (itemTop > winHeight - 120) {
            elemScroll.animate({ 'scrollTop': itemTop / 2 }, 200)
        }
    }


    //鏌ョ湅浠ｇ爜
    $(function () {
        var DemoCode = $('#LAY_democode');
        DemoCode.val([
            DemoCode.val()
            , '<body>'
            , global.preview
            , '\n<script src="//res.layui.com/layui/dist/layui.js" charset="utf-8"></script>'
            , '\n<!-- 娉ㄦ剰锛氬鏋滀綘鐩存帴澶嶅埗鎵€鏈変唬鐮佸埌鏈湴锛屼笂杩癹s璺緞闇€瑕佹敼鎴愪綘鏈湴鐨� -->'
            , $('#LAY_democodejs').html()
            , '\n</body>\n</html>'
        ].join(''));
    });

    //鐐瑰嚮鏌ョ湅浠ｇ爜閫夐」
    element.on('tab(demoTitle)', function (obj) {
        if (obj.index === 1) {
            if (device.ie && device.ie < 9) {
                layer.alert('寮虹儓涓嶆帹鑽愪綘閫氳繃ie8/9 鏌ョ湅浠ｇ爜锛佸洜涓猴紝鎵€鏈夌殑鏍囩閮戒細琚牸寮忔垚澶у啓锛屼笖娌℃湁鎹㈣绗︼紝褰卞搷闃呰');
            }
        }
    })


    //鎵嬫満璁惧鐨勭畝鍗曢€傞厤
    var treeMobile = $('.site-tree-mobile')
        , shadeMobile = $('.site-mobile-shade')

    treeMobile.on('click', function () {
        $('body').addClass('site-mobile');
    });

    shadeMobile.on('click', function () {
        $('body').removeClass('site-mobile');
    });



    //鎰氫汉鑺�
    ; !function () {
        if (home.data('date') === '4-1') {

            if (local['20180401']) return;

            home.addClass('site-out-up');
            setTimeout(function () {
                layer.photos({
                    photos: {
                        "data": [{
                            "src": "//cdn.layui.com/upload/2018_4/168_1522515820513_397.png",
                        }]
                    }
                    , anim: 2
                    , shade: 1
                    , move: false
                    , end: function () {
                        layer.msg('鎰氬叕锛屽揩閱掗啋锛�', {
                            shade: 1
                        }, function () {
                            layui.data('layui', {
                                key: '20180401'
                                , value: true
                            });
                        });
                    }
                    , success: function (layero, index) {
                        home.removeClass('site-out-up');

                        layero.find('#layui-layer-photos').on('click', function () {
                            layer.close(layero.attr('times'));
                        }).find('.layui-layer-imgsee').remove();
                    }
                });
            }, 1000 * 3);
        }
    }();



    exports('global', {});
});