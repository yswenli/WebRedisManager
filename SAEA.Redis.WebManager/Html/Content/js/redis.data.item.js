layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var layerIndex = -1;

    layerIndex = layer.msg('加载中', {
        icon: 16
        , shade: 0.01
    });

    var redis_name = GetRequest().name;
    var db_index = GetRequest().dbindex;
    var item_type = GetRequest().type;
    var item_id = GetRequest().id;

    var item_typeStr = "hash"
    switch (item_type * 1) {
        case 2:
            item_typeStr = "hash";
            break;
        case 3:
            item_typeStr = "set";
            break;
        case 4:
            item_typeStr = "zset";
            break;
        case 5:
            item_typeStr = "list";
            break;
    }

    $(".keys-header").html(`redis_name:${redis_name} db:${db_index} type:${item_typeStr} id:${item_id}`);

    //加载列表


    function loadList() {
        var rurl = `/api/redis/GetItems?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}`;
        $.get(rurl, null, function (jdata) {

            if (jdata.Code == 1) {

                switch (item_type * 1) {
                    case 2:
                        var table_content = `<table class="layui-table">
                            <colgroup>
                                <col width="150">
                                <col width="200">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>key</th>
                                    <th>value</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (var datakey in jdata.Data) {
                            var thtml = `<tr>
                                                                <td>${datakey}</td>
                                                                <td>${jdata.Data[datakey]}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-id="${datakey}" data-val="${jdata.Data[datakey]}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    case 3:
                        var table_content = `<table class="layui-table">
                            <colgroup>
                                <col width="150">
                                <col width="200">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>key</th>
                                    <th>value</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (var i = 0; i < jdata.Data.length; i++) {
                            var thtml = `<tr>
                                                                <td>${item_id}</td>
                                                                <td>${jdata.Data[i]}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${jdata.Data[i]}">
<a href="javascript:;" class="view-link">查看</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    case 4:
                        var table_content = `<table class="layui-table">
                            <colgroup>
                                <col width="150">
                                <col width="200">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>value</th>
                                    <th>score</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (var i = 0; i < jdata.Data.length; i++) {
                            var thtml = `<tr>
                                                                <td>${jdata.Data[i].Value}</td>
                                                                <td>${jdata.Data[i].Score}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${jdata.Data[i].Value}">
<a href="javascript:;" class="view-link">查看</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    default:
                        var table_content = `<table class="layui-table">
                            <colgroup>
                                <col width="150">
                                <col width="200">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>key</th>
                                    <th>value</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (var i = 0; i < jdata.Data.length; i++) {
                            var thtml = `<tr>
                                                                <td>${item_id}</td>
                                                                <td>${jdata.Data[i]}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${jdata.Data[i]}">
<a href="javascript:;" class="view-link">查看</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                }

                //编辑
                $(".edit-link").on("click", function () {
                    var type = $(this).parent().attr("data-type");
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
                            time: 0,
                            content: [`/html/ItemsView.html?name=${redis_name}&dbindex=${db_index}&key=${key}&type=${type}`, 'no']
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
                            $.get(`/api/redis/del?name=${redis_name}&dbindex=${db_index}&key=${key}`, null, function (data) {
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


                layer.close(layerIndex);
            }
            else {
                layer.msg("操作失败:" + sdata.Message, { time: 2000 });
            }

        });
    }

    loadList();


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
            content: [`/html/additem.html?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}`, 'no']
        });
    });
    //修改按钮
    $("#ren_link").on("click", function () {
        layer.open({
            title: '修改id',
            type: 2,
            area: ['580px', '240px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: [`/html/rename.html?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}`, 'no']
        });
    });
});