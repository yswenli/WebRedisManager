/// <reference path="formhelper.js" />
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
}
function htmlEncode(text) {
    return text.replace(/&/g, '&amp;').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}
function htmlDecode(text) {
    return text.replace(/&gt;/g, ">").replace(/&lt;/g, "<").replace(/&quot;/g, '"').replace(/&amp;/g, "&");
}


layui.use(['jquery', 'layer', 'form', 'laypage'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$, laypage = layui.laypage;

    var layerIndex = -1;

    var redis_name = decodeURIComponent(GetRequest().name);
    var db_index = GetRequest().dbindex;
    var item_type = GetRequest().type;
    var item_id = decodeURIComponent(GetRequest().id);

    var item_typeStr = "hash";

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

    $.get(`/api/redis/getcount?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&ID=${encodeURIComponent(item_id)}`, null, function (gdata) {

        $(".keys-header").html(`redis_name:${redis_name} db:${db_index} type:${item_typeStr} id:${item_id} count:${gdata.Data} 【<a href="javascript:;" id="redis_console" style="color:#009688;">Redis Console</a>】`);

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

    var searchKey = "*";

    //加载列表

    var dataOffset = 0;

    function loadList() {

        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.01
        });

        var rurl = `/api/redis/getitems?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&id=${encodeURIComponent(item_id)}&key=${searchKey}&offset=${dataOffset}`;

        $.get(rurl, null, function (jdata) {

            if (jdata.Code === 1) {

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
                                    <th style="width:80%;">value</th>
                                    <th style="width:70px;">操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (var datakey in jdata.Data) {
                            thtml = `<tr>
                                                                <td>${htmlEncode(datakey)}</td>
                                                                <td>${htmlEncode(jdata.Data[datakey])}</td>
                                                                <td data-name="${encodeURIComponent(redis_name)}" data-dbindex="${db_index}" data-id="${encodeURIComponent(item_id)}" data-key="${encodeURIComponent(datakey)}" data-val="${encodeURIComponent(jdata.Data[datakey])}">
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
                                    <th style="width:80%;">value</th>
                                    <th style="width:70px;">操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (var i = 0; i < jdata.Data.length; i++) {
                            thtml = `<tr>
                                                                <td>${htmlEncode(item_id)}</td>
                                                                <td>${htmlEncode(jdata.Data[i])}</td>
                                                                <td data-name="${encodeURIComponent(redis_name)}" data-dbindex="${db_index}" data-key="${encodeURIComponent(item_id)}" data-val="${encodeURIComponent(jdata.Data[i])}">
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
                                    <th>score</th>
                                    <th style="width:80%;">value</th>
                                    <th style="width:70px;">操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (i = 0; i < jdata.Data.length; i++) {
                            thtml = `<tr>
                                        <td>${jdata.Data[i].Score}</td>
                                        <td>${htmlEncode(jdata.Data[i].Value)}</td>
                                        <td data-name="${encodeURIComponent(redis_name)}" data-dbindex="${db_index}" data-key="${jdata.Data[i].Score}" data-val="${encodeURIComponent(jdata.Data[i].Value)}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    case 5:
                        table_content = `<table class="layui-table">
                            <colgroup>
                                <col width="150">
                                <col width="200">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th>key</th>
                                    <th style="width:80%;">value</th>
                                    <th style="width:70px;">操作</th>
                                </tr>
                            </thead>
                            <tbody id="redis-data-body"></tbody>
                        </table>`;
                        $("#table-container").html(table_content);
                        for (i = 0; i < jdata.Data.length; i++) {
                            thtml = `<tr>
                                                                <td>${htmlEncode(item_id)}</td>
                                                                <td>${htmlEncode(jdata.Data[i])}</td>
                                                                <td data-name="${encodeURIComponent(redis_name)}" data-dbindex="${db_index}" data-key="${i}" data-val="${encodeURIComponent(jdata.Data[i])}">
<a href="javascript:;" class="edit-link">编辑</a> | <a href="javascript:;" class="del-link">删除</a></td>
                                                            </tr>`;
                            $("#redis-data-body").append(thtml);
                        }
                        break;
                    default:
                        layer.msg("未知的类型");
                        break;
                }

                
                var totalHeight = $(parent.window.document).find("iframe").height();
                var bodyMaxHeight = totalHeight - 233;
                $("#table-container").css({ "max-height": bodyMaxHeight, "overflow": "scroll" });

                //编辑
                var edit_form_html = "";
                $(".edit-link").off();
                $(".edit-link").on("click", function () {
                    var key = decodeURIComponent($(this).parent().attr("data-key"));
                    key = htmlEncode(key);
                    var val = decodeURIComponent($(this).parent().attr("data-val"));
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
                                                        <textarea id="redis_value" type="text" name="value" autocomplete="off" placeholder="value" class="layui-textarea" lay-verify="required" rows="16">${val}</textarea>
                                                    </div>
                                                </div>
                                            </form>`;
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
                                                        <textarea id="redis_value" type="text" name="value" autocomplete="off" placeholder="value" class="layui-textarea" lay-verify="required" rows="18">${val}</textarea>
                                                    </div>
                                                </div>
                                            </form>`;
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
                                                        <textarea id="redis_value" type="text" name="value" autocomplete="off" placeholder="value" class="layui-textarea" lay-verify="required" rows="16">${val}</textarea>
                                                    </div>
                                                </div>
                                            </form>`;
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
                                                        <textarea id="redis_value" type="text" name="value" autocomplete="off" placeholder="value" class="layui-textarea" lay-verify="required" rows="18">${val}</textarea>
                                                    </div>
                                                </div>
                                            </form>`;
                            break;
                    }

                    layer.open({
                        title: '编辑',
                        type: 1,
                        area: ['590px', '520px'],
                        fixed: true,
                        resize: false,
                        move: false,
                        anim: 1,
                        time: 0,
                        content: edit_form_html,
                        btn: ['确定', '关闭'],
                        yes: function (index, layero) {
                            key = htmlDecode(key);
                            $.ajaxSettings.async = false;
                            $.post(`/api/redis/delitem?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&id=${encodeURIComponent(item_id)}&type=${item_type}&key=${encodeURIComponent(key)}&value=${encodeURIComponent(val)}`, null, null);
                            $.ajaxSettings.async = true;

                            var pdata = new FormHelper().SerializeForm("edit_form");

                            $.post("/api/redis/edit", pdata, function (edata) {
                                layer.close(index);
                                $("#search_btn").click();
                            });
                        },
                        btn2: function (index, layero) {
                            layer.close(index);
                        }
                    });
                });
                //移除
                $(".del-link").off();
                $(".del-link").on("click", function () {

                    var delLink = $(this).parent();

                    var key = encodeURIComponent(delLink.attr("data-key"));

                    var val = delLink.attr("data-val");
                    if (val !== undefined) {
                        val = encodeURIComponent(val);
                    }

                    layer.confirm("确认要删除此项数据么?", {
                        btn: ['确定', '取消']
                    },
                        function (index) {
                            switch (item_type * 1) {
                                case 2:
                                    $.post(`/api/redis/delitem?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&id=${encodeURIComponent(item_id)}&key=${key}&value=${val}`, null, function (data) {
                                        if (data.Code === 1) {
                                            $("#search_btn").click();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                                case 3:
                                    $.post(`/api/redis/delitem?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&key=${key}&value=${val}`, null, function (data) {
                                        if (data.Code === 1) {
                                            $("#search_btn").click();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                                case 4:
                                    $.post(`/api/redis/delitem?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&id=${encodeURIComponent(item_id)}&value=${val}`, null, function (data) {
                                        if (data.Code === 1) {
                                            $("#search_btn").click();
                                        }
                                        else {
                                            layer.msg("操作失败:" + data.Message, { time: 2000 });
                                        }
                                    });
                                    break;
                                case 5:
                                    $.post(`/api/redis/delitem?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&id=${encodeURIComponent(item_id)}&key=${key}&value=${val}`, null, function (data) {
                                        if (data.Code === 1) {
                                            $("#search_btn").click();
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
        if (searchKey === undefined || searchKey === "") {
            searchKey = "*";
        }
        loadList();
    });

    $("#search-key").keypress(function (e) {
        if (e.which === 13) {
            $("#search_btn").click();
        }
    });

    //添加按钮
    $("#add_link").on("click", function () {
        layer.open({
            title: 'set redis数据',
            type: 2,
            area: ['580px', '544px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: [`/additem.html?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&id=${encodeURIComponent(item_id)}`, 'no']
        });
    });
    //修改按钮
    $("#ren_link").on("click", function () {
        layer.open({
            title: '修改id',
            type: 2,
            area: ['580px', '376px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: [`/rename.html?name=${encodeURIComponent(redis_name)}&dbindex=${db_index}&type=${item_type}&id=${encodeURIComponent(item_id)}`, 'no']
        });
    });
});