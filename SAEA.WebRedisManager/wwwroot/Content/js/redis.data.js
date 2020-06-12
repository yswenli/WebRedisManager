function htmlEncode(text) {
    return text.replace(/&/g, '&amp;').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}
function htmlDecode(text) {
    return text.replace(/&gt;/g, ">").replace(/&lt;/g, "<").replace(/&quot;/g, '"').replace(/&amp;/g, "&");
}

function isJSON(str) {
    if (typeof str === 'string') {
        try {
            var obj = JSON.parse(str);
            if (str.indexOf('{') > -1) {
                return true;
            } else {
                return false;
            }
        }
        catch (e) {
            console.log(e);
            return false;
        }
    }
    return false;
}

layui.use(['jquery', 'layer', 'form', 'laypage'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$, laypage = layui.laypage;

    var layerIndex = -1;

    layerIndex = layer.msg('加载中', {
        icon: 16
        , shade: 0.01
    });

    var redis_name = GetRequest().name;

    var db_index = GetRequest().dbindex;

    var searchKey = $("#search-key").val();

    //keys列表
    $.get(`/api/redis/getdbsize?name=${redis_name}&dbindex=${db_index}`, null, function (gdata) {

        layer.close(layerIndex);

        $(".keys-header").html(`【<a style="color:#009688;" href="/chart.html?name=${redis_name}">Back</a>】 Keys List <i class="layui-icon layui-icon-refresh" style="color:#009688;cursor: pointer;" onclick="location.reload();" title="刷新"></i> redisName:<small>${decodeURI(redis_name)}</small> dbIndex:<small>${db_index}</small> dbsize:<small>${gdata.Data}</small>【<a href="javascript:;" id="redis_console" style="color:#009688;">Redis Console</a>】`);

        $("#redis_console").click(() => {
            layer.full(layer.open({
                title: '命令行模式',
                type: 2,
                area: ['580px', '318px'],
                fixed: true,
                resize: false,
                move: false,
                maxmin: true,
                scrollbar: true,
                time: 0,
                content: [`/console.html?name=${redis_name}`, 'no']
            }));
        });
    });

    //



    //加载列表

    var dataOffset = 0;

    function loadList(searchKey) {

        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.3, time: 50000
        });

        var rurl = `/api/redis/getkeytypes?name=${redis_name}&dbindex=${db_index}&key=${searchKey}&offset=${dataOffset}`;

        $.get(rurl, null, function (jdata) {

            if (jdata.Code === 1) {

                $("#redis-data-body").html("");

                for (var i = 0; i < jdata.Data.length; i++) {

                    var tkey = jdata.Data[i].Key;

                    if (tkey.indexOf("%")) {
                        tkey = tkey.replace(/%/g, '%25');
                    }
                    var tkey1 = htmlEncode(decodeURIComponent(tkey));
                    var tkey2 = encodeURIComponent(tkey);

                    var thtml = `<tr>
                                    <td>${jdata.Data[i].Type}</td>                                    
                                    <td>${tkey1}</td>
                                    <td><img src="/content/js/css/modules/layer/default/loading-2.gif" width="18" /></td>
                                    <td class="redis-data-td" data-name="${encodeURIComponent(redis_name)}" data-dbindex="${db_index}" data-key="${tkey2}" data-type="${jdata.Data[i].Type}">
<a href="javascript:;" class="view-link">查看</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                                                            </tr>`;
                    $("#redis-data-body").append(thtml);

                    $(".redis-data-td").each(function (tdindex) {
                        var ttl_td = $(this).prev();
                        var td_key = $(this).attr("data-key");
                        var td_url = `/api/redis/getttl?name=${redis_name}&dbindex=${db_index}&key=${td_key}`;
                        $.get(td_url, null, function (tddata) {
                            ttl_td.html(tddata.Data);
                        });
                    });
                }
                //查看
                $(".view-link").on("click", function () {
                    var type = $(this).parent().attr("data-type");
                    var typeid = 2;
                    switch (type) {
                        case "hash":
                            typeid = 2;
                            break;
                        case "set":
                            typeid = 3;
                            break;
                        case "zset":
                            typeid = 4;
                            break;
                        case "list":
                            typeid = 5;
                            break;
                    }

                    var key = $(this).parent().attr("data-key");

                    if (type === "string") {

                        var info_url = `/api/redis/get?name=${redis_name}&dbindex=${db_index}&key=${key}`;

                        $.get(info_url, null, function (vdata) {

                            if (vdata.Code === 1) {

                                if (isJSON(vdata.Data)) {
                                    var jsonMsg = htmlEncode(JSON.stringify(JSON.parse(vdata.Data), " ", 4));
                                    layer.alert(`<pre>${jsonMsg}</pre>`);
                                }
                                else {
                                    layer.alert(htmlEncode(vdata.Data));
                                }
                            }
                            else {
                                layer.msg("操作失败:" + data.Message, { time: 2000 });
                            }
                        });
                    }
                    else {
                        layer.full(layer.open({
                            title: '查看数据',
                            type: 2,
                            area: ['580px', '318px'],
                            fixed: true,
                            resize: false,
                            move: false,
                            maxmin: true,
                            scrollbar: true,
                            time: 0,
                            content: [`/itemsview.html?name=${redis_name}&dbindex=${db_index}&id=${key}&type=${typeid}`, 'no']
                        }));
                    }
                });
                //移除
                $(".del-link").on("click", function () {

                    var key = $(this).parent().attr("data-key");

                    layer.confirm("确认要删除此项数据么?", {
                        btn: ['确定', '取消']
                    },
                        function (index) {
                            layer.close(index);
                            $.post(`/api/redis/del?name=${redis_name}&dbindex=${db_index}&key=${key}`, null, function (data) {
                                if (data.Code === 1) {
                                    $("#search_btn").click();
                                }
                                else {
                                    layer.msg("操作失败:" + data.Message, { time: 2000 });
                                }
                            });
                        }
                    );
                });
            }
            else {
                layer.msg("操作失败:" + sdata.Message, { time: 2000 });
            }
            //
            layer.close(layerIndex);
        });
    }

    loadList("*");

    //查询
    $("#search_btn").click(function () {

        searchKey = $("#search-key").val();

        if (searchKey === undefined || searchKey === "") {
            loadList("*");
        }
        else {
            loadList(encodeURIComponent(searchKey));
        }
    });

    $("#search-key").keypress(function (e) {
        if (e.which === 13) {
            $("#search_btn").click();
        }
    });

    $("#batch_remove_btn").click(() => {

        layer.confirm("此操作为按通配符进行批量删除，确定执行此操作么？", {
            btn: ['确定', '取消'], icon: 3
        },
            function (index) {

                searchKey = $("#search-key").val();

                if (searchKey === undefined || searchKey === "") {
                    layer.msg('输入框内容不能为空！', {
                        icon: 2, time: 2000
                    }, function () {
                        layer.closeAll();
                    });
                }
                else {
                    layerIndex = layer.msg('正在批量删除中', {
                        icon: 16
                        , shade: 0.3
                        , time: 50000
                    });

                    var brurl = `/api/redis/batchremove?name=${redis_name}&dbindex=${db_index}&key=${searchKey}`;

                    $.get(brurl, null, function (olen) {

                        layer.msg(`批量删除已完成，已成功删除${olen.Data}条`, {
                            icon: 1, time: 2000
                        }, function () {
                            $("#search_btn").click();
                        });

                    });
                }
            }
        );
    });


    //添加按钮
    $("#add_link").on("click", function () {
        layer.open({
            title: '添加redis数据',
            type: 2,
            area: ['580px', '544px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: [`/additem.html?name=${redis_name}&dbindex=${db_index}&type=1&id=`, 'no']
        });
    });
});