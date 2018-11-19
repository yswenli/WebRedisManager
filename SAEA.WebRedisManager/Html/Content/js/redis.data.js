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

        $(".keys-header").html(`keys列表 <i class="layui-icon layui-icon-refresh" style="color:#0094ff;cursor: pointer;" onclick="location.reload();" title="刷新"></i> redisName:<small>${redis_name}</small> dbIndex:<small>${db_index}</small> dbsize:<small>${gdata.Data}</small>`);
    });

    //


    //加载列表

    var dataOffset = 0;

    function loadList(searchKey) {

        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.01
        });

        var rurl = `/api/redis/getkeytypes?name=${redis_name}&dbindex=${db_index}&key=${searchKey}&offset=${dataOffset}`;
        $.get(rurl, null, function (jdata) {

            if (jdata.Code == 1) {

                $("#redis-data-body").html("");

                for (var i = 0; i < jdata.Data.length; i++) {
                    var thtml = `<tr>
                                                                                                <td>${jdata.Data[i].Key}</td>
                                                                                                <td>${jdata.Data[i].Type}</td>
                                                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${jdata.Data[i].Key}" data-type="${jdata.Data[i].Type}">
<a href="javascript:;" class="view-link">查看</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                                                            </tr>`;
                    $("#redis-data-body").append(thtml);
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
                    if (type == "string") {
                        var info_url = `/api/redis/get?name=${redis_name}&dbindex=${db_index}&key=${key}`;
                        $.get(info_url, null, function (vdata) {
                            if (vdata.Code == 1) {
                                layer.alert(vdata.Data);
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
                            content: [`/ItemsView.html?name=${redis_name}&dbindex=${db_index}&id=${key}&type=${typeid}`, 'no']
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
                                if (data.Code == 1) {
                                    location.reload();
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

        if (searchKey == undefined || searchKey == "") {
            loadList("*");
        }
        else {
            loadList(searchKey);
        }
    });


    //添加按钮
    $("#add_link").on("click", function () {
        layer.open({
            title: '添加redis数据',
            type: 2,
            area: ['580px', '318px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: [`/additem.html?name=${redis_name}&dbindex=${db_index}&type=1&id=`, 'no']
        });
    });
});