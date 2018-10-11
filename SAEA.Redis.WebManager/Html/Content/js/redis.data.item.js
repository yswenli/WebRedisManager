layui.use(['jquery', 'layer', 'form', 'laypage'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$, laypage = layui.laypage;

    var layerIndex = -1;

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

    var searchKey = "*";

    //加载列表

    var dataOffset = 0;

    function loadList() {

        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.01
        });

        var rurl = `/api/redis/GetItems?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}&key=${searchKey}&offset=${dataOffset}`;
        $.get(rurl, null, function (jdata) {

            if (jdata.Code == 1) {

                var table_content = "";

                var thtml = "";

                switch (item_type * 1) {
                    case 2:
                        table_content = `<table class="layui-table">
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
                            thtml = `<tr>
                                                                <td>${datakey}</td>
                                                                <td>${jdata.Data[datakey]}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-id="${item_id}" data-key="${datakey}" data-val="${jdata.Data[datakey]}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    case 3:
                        table_content = `<table class="layui-table">
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
                            thtml = `<tr>
                                                                <td>${item_id}</td>
                                                                <td>${jdata.Data[i]}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${jdata.Data[i]}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    case 4:
                        table_content = `<table class="layui-table">
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
                            thtml = `<tr>
                                                                <td>${jdata.Data[i].Value}</td>
                                                                <td>${jdata.Data[i].Score}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${jdata.Data[i].Score}" data-val="${jdata.Data[i].Value}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    default:
                        table_content = `<table class="layui-table">
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
                            thtml = `<tr>
                                                                <td>${item_id}</td>
                                                                <td>${jdata.Data[i]}</td>
                                                                <td data-name="${redis_name}" data-dbindex="${db_index}" data-key="${i}" data-val="${jdata.Data[i]}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                }

                //编辑
                $(".edit-link").off();
                $(".edit-link").on("click", function () {
                    debugger;
                    var key = $(this).parent().attr("data-key");
                    var val = $(this).parent().attr("data-val");
                    var edit_form_html = "";
                    switch (item_type * 1) {
                        case 2:
                            edit_form_html = `<form id="edit_form" class="layui-form layui-form-pane" action="">
                                                <input type="hidden" name="name" value="${redis_name}" />
                                                <input type="hidden" name="dbindex" value="${db_index}" />
                                                <input type="hidden" name="type" value="${item_type}" />
                                                <input type="hidden" name="id" value="${item_id}" />
                                                <div class="layui-form-item">
                                                    <label class="layui-form-label">key</label>
                                                    <div class="layui-input-block">
                                                        <input type="text" name="key" autocomplete="off" placeholder="key" class="layui-input" value="${key}" readonly="readonly" />
                                                    </div>
                                                </div>
                                                <div class="layui-form-item">
                                                    <label class="layui-form-label">value</label>
                                                    <div class="layui-input-block">
                                                        <input type="text" name="value" autocomplete="off" placeholder="value" class="layui-input" value="${val}" />
                                                    </div>
                                                </div>
                                            </form>`;
                            $("#edit-form-container").html(edit_form_html);
                            break;
                        case 3:
                            edit_form_html = `<form id="edit_form" class="layui-form layui-form-pane" action="">
                                                <input type="hidden" name="name" value="${redis_name}" />
                                                <input type="hidden" name="dbindex" value="${db_index}" />
                                                <input type="hidden" name="type" value="${item_type}" />
                                                <input type="hidden" name="id" value="${item_id}" />
                                                <input type="hidden" name="key" value="${key}" />
                                                <div class="layui-form-item">
                                                    <label class="layui-form-label">value</label>
                                                    <div class="layui-input-block">
                                                        <input type="text" name="value" autocomplete="off" placeholder="value" class="layui-input" value="${key}" />
                                                    </div>
                                                </div>
                                            </form>`;
                            $("#edit-form-container").html(edit_form_html);
                            break;
                        case 4:                            
                            edit_form_html = `<form id="edit_form" class="layui-form layui-form-pane" action="">
                                                <input type="hidden" name="name" value="${redis_name}" />
                                                <input type="hidden" name="dbindex" value="${db_index}" />
                                                <input type="hidden" name="type" value="${item_type}" />
                                                <input type="hidden" name="id" value="${item_id}" />
                                                <div class="layui-form-item">
                                                    <label class="layui-form-label">score</label>
                                                    <div class="layui-input-block">
                                                        <input type="text" name="key" autocomplete="off" placeholder="score" class="layui-input" value="${key}"/>
                                                    </div>
                                                </div>
                                                <div class="layui-form-item">
                                                    <label class="layui-form-label">value</label>
                                                    <div class="layui-input-block">
                                                        <input type="text" name="value" autocomplete="off" placeholder="value" class="layui-input" value="${val}" />
                                                    </div>
                                                </div>
                                            </form>`;
                            $("#edit-form-container").html(edit_form_html);

                            $.ajaxSettings.async = false;
                            $.post(`/api/redis/delitem?name=${redis_name}&dbindex=${db_index}&id=${item_id}&type=${item_type}&key=${key}&value=${val}`, null, null);
                            $.ajaxSettings.async = true;
                            break;

                        case 5:
                            edit_form_html = `<form id="edit_form" class="layui-form layui-form-pane" action="">
                                                <input type="hidden" name="name" value="${redis_name}" />
                                                <input type="hidden" name="dbindex" value="${db_index}" />
                                                <input type="hidden" name="type" value="${item_type}" />
                                                <input type="hidden" name="id" value="${item_id}" />
                                                <input type="hidden" name="key" value="${key}" />
                                                <div class="layui-form-item">
                                                    <label class="layui-form-label">value</label>
                                                    <div class="layui-input-block">
                                                        <input type="text" name="value" autocomplete="off" placeholder="value" class="layui-input" value="${val}" />
                                                    </div>
                                                </div>
                                            </form>`;
                            $("#edit-form-container").html(edit_form_html);
                            break;
                    }

                    layer.open({
                        title: '编辑',
                        type: 1,
                        area: ['580px', '210px'],
                        fixed: true,
                        resize: false,
                        move: false,
                        anim: 1,
                        time: 0,
                        content: $("#edit-form-container").html(),
                        btn: ['确定', '关闭'],
                        yes: function (index, layero) {
                            debugger;
                            var pdata = $($("#edit_form")[1]).serialize();
                            $.post("/api/redis/edit", pdata, function (edata) {
                                layer.close(index);
                                location.reload();
                            });
                        },
                        btn2: function (index, layero) {
                            var pdata = $($("#edit_form")[1]).serialize();
                            $.post("/api/redis/edit", pdata, function (edata) {
                                layer.close(index);
                                location.reload();
                            });
                            layer.close(index);
                        }
                    })
                });
                //移除
                $(".del-link").off();
                $(".del-link").on("click", function () {

                    var key = $(this).parent().attr("data-key");

                    layer.confirm("确认要删除此项数据么?", {
                        btn: ['确定', '取消']
                    },
                        function (index) {
                            switch (item_type * 1) {
                                case 2:
                                    $.post(`/api/redis/delitem?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}&key=${key}`, null, function (data) {
                                        if (data.Code == 1) {
                                            location.reload();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                                case 3:
                                    $.post(`/api/redis/delitem?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}&key=${key}`, null, function (data) {
                                        if (data.Code == 1) {
                                            location.reload();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                                case 4:
                                    var val = $(this).parent().attr("data-val");
                                    $.post(`/api/redis/delitem?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}&value=${val}`, null, function (data) {
                                        if (data.Code == 1) {
                                            location.reload();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                                case 5:
                                    $.post(`/api/redis/delitem?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}&key=${key}`, null, function (data) {
                                        if (data.Code == 1) {
                                            location.reload();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                            }

                        }
                    );
                });

                layer.close(layerIndex);
            }
            else {
                layer.msg("操作失败:" + sdata.Message, { time: 2000 });
            }
            //
            layer.close(layerIndex);
        });
    }

    loadList();

    //查询
    $("#search_btn").click(function () {
        searchKey = $("#search-key").val();
        if (searchKey == undefined || searchKey == "") {
            searchKey = "*";
        }
        loadList();
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
            content: [`/additem.html?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}`, 'no']
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
            content: [`/rename.html?name=${redis_name}&dbindex=${db_index}&type=${item_type}&id=${item_id}`, 'no']
        });
    });
});