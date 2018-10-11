layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;
    //
    var layerIndex = -1;

    //redis列表
    $(function () {
        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.01
        });
        //默认加载redis烈表
        $.get("/api/config/getlist", null, function (data) {
            layer.close(layerIndex);
            if (data.Code == 1) {
                if (data.Data != undefined && data.Data.length > 0) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var html = `<dd class="layui-nav-itemed">
                                <a class='redis_link' href="javascript:;" data-name='${data.Data[i].Name}' title='${JSON.stringify(data.Data[i])}'>&nbsp;&nbsp;<i class="layui-icon layui-icon-template-1"></i> ${data.Data[i].Name}</a>                                
                            </dd>`
                        $("dl.redis-dbs").append(html);
                    }

                    //点击redis实例
                    $("a.redis_link").on("click", function () {
                        var _parent = $(this).parent();
                        var name = $(this).attr("data-name");

                        var isLoaded = _parent.attr("data-loaded");

                        if (isLoaded != undefined) {
                            _parent.removeAttr("data-loaded");
                            _parent.find("dl").remove();
                            return;
                        }
                        _parent.attr("data-loaded", "1");

                        layerIndex = layer.msg('加载中', {
                            icon: 16
                            , shade: 0.01
                        });
                        $.post("/api/redis/connect", "name=" + name, function (dbData) {
                            layer.close(layerIndex);
                            if (dbData.Code == 1) {
                                if (dbData.Data != undefined && dbData.Data.length > 0) {
                                    _parent.find("dl").remove();
                                    _parent.append('<dl class="layui-nav-child redis-db"></dl>');
                                    var db_dl = _parent.find("dl");
                                    for (var j = 0; j < dbData.Data.length; j++) {
                                        var shtml = `<dd><a class='redis_db_link' href='javascript:;' data-name='${name}' data-db='${j}'>&nbsp;&nbsp;&nbsp;&nbsp;<i class="layui-icon layui-icon-circle"></i> db${j}</a></dd>`;
                                        db_dl.append(shtml);
                                    }

                                    $(".layadmin-iframe").attr("src", "/chart.html?name=" + name);

                                    //点击redus db
                                    $("a.redis_db_link").on("click", function () {

                                        debugger;

                                        var sname = $(this).attr("data-name");
                                        var dbindex = $(this).attr("data-db");

                                        $(".layadmin-iframe").attr("src", "/keys.html?name=" + sname + "&dbindex=" + dbindex);

                                    });
                                }
                            }
                            else {
                                layer.msg("操作失败:" + data.Message, { time: 2000 });
                            }
                        })
                    });
                }
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });
    });


    //添加redis按钮
    $("#add_link").on("click", function () {
        layer.open({
            title: '添加redis服务器',
            type: 2,
            area: ['580px', '318px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/RedisAdd.html', 'no'],
            end: function () {
                location.reload();
            }
        });
    });

    //移除redis按钮
    $("#rem_link").on("click", function () {
        layer.open({
            title: '删除redis服务器',
            type: 2,
            area: ['580px', '200px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/remove.html', 'no']
        });
    });

    //提交添加redis表单
    $("#add_btn").on("click", function () {
        var json = $("#add_form").serialize();
        $.post("/api/config/set", json, function (data) {
            if (data.Code == 1) {
                parent.location.reload();
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });
    });
    //提交删除redis表单
    $("#rem_btn").on("click", function () {
        layer.confirm("确认要删除此项redis配置么?", {
            btn: ['确定', '取消']
        },
            function (index) {
                layer.close(index);
                var json = $("#add_form").serialize();
                $.post("/api/config/rem", json, function (data) {
                    if (data.Code == 1) {
                        parent.location.reload();
                    }
                    else {
                        layer.msg("操作失败:" + data.Message, { time: 2000 });
                    }
                });
            }
        );
    });
});

