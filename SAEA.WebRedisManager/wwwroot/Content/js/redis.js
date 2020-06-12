layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;
    //
    var layerIndex = -1;

    //redis列表
    $(function () {

        var atimer = setInterval(function () {
            $.get("/user/authenticated", null, function (adata) {
                if (adata.Code === 1 && adata.Data === false) {
                    layer.msg("当前操作需要登录", { time: 2000 }, function () {
                        clearInterval(atimer);
                        location.href = "/login.html";
                    });
                }
            });
        }, 2000);


        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.01
        });
        //默认加载redis烈表

        $.get("/api/config/getlist", null, function (data) {

            layer.close(layerIndex);

            if (data.Code === 1) {

                if (data.Data !== undefined && data.Data.length > 0) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var html = `<dd class="layui-nav-itemed">
                                <a class='redis_link' href="javascript:;" data-name='${data.Data[i].Name}' title='${JSON.stringify(data.Data[i])}'>&nbsp;&nbsp;<i class="layui-icon layui-icon-template-1"></i> ${data.Data[i].Name}</a>                                
                            </dd>`;
                        $("dl.redis-dbs").append(html);
                    }
                    //搜索

                    $("#search_list").keyup(function () {

                        var searchText = $(this).val();

                        if (searchText === "") {
                            $(".redis_link").each(function (index) {
                                $(this).parent().show();
                            });
                        }

                        $(".redis_link").each(function (index) {
                            if ($(this).text().indexOf(searchText) === -1 && $(this).attr("title").indexOf(searchText) === -1) {
                                $(this).parent().hide();
                            }
                            else {
                                $(this).parent().show();
                            }
                        });
                    });

                    //点击redis实例
                    $("a.redis_link").on("click", function () {
                        var _parent = $(this).parent();
                        var name = encodeURI($(this).attr("data-name"));

                        var isLoaded = _parent.attr("data-loaded");

                        if (isLoaded !== undefined) {
                            _parent.removeAttr("data-loaded");
                            _parent.find("dl").remove();
                            return;
                        }
                        _parent.attr("data-loaded", "1");

                        layerIndex = layer.msg('加载中', {
                            icon: 16
                            , shade: 0.01
                            , time: 30000
                        });
                        $.post("/api/redis/connect?name=" + encodeURI(name), null, function (dbData) {
                            layer.close(layerIndex);
                            if (dbData.Code === 1) {
                                if (dbData.Data !== undefined && dbData.Data.length > 0) {
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

                                        var sname = $(this).attr("data-name");
                                        var dbindex = $(this).attr("data-db");

                                        $(".layadmin-iframe").attr("src", "/keys.html?name=" + encodeURI(sname) + "&dbindex=" + dbindex);

                                    });
                                }
                            }
                            else {
                                layer.msg("操作失败:" + dbData.Message, { time: 2000 });
                            }
                        });
                    });
                    //
                    $("textarea[name='configs']").val(JSON.stringify(data.Data, " ", 4));
                }
            }
            else if (data.Code === 3) {
                layer.msg("操作失败:" + data.Message, { time: 2000 }, function () {
                    location.href = "/login.html";
                });
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });


    });


    //添加redis按钮
    $("#add_link").on("click", function () {
        layer.open({
            title: 'Set Redis Server',
            type: 2,
            area: ['580px', '320px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/redisadd.html', 'no'],
            end: function () {
                location.reload();
            }
        });
    });

    //移除redis按钮
    $("#rem_link").on("click", function () {
        layer.open({
            title: 'Remove Redis Server',
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
    //redis server configs按钮
    $("#conf_link").on("click", function () {
        layer.open({
            title: 'Redis Server Configs',
            type: 2,
            area: ['670px', '560px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/configs.html', 'no']
        });
    });

    //users list按钮
    $("#account_link").on("click", function () {
        $(".layadmin-iframe").attr("src", "/userlist.html");
    });

    //提交添加redis表单
    $("#add_btn").on("click", function () {
        //var str = $("#add_form").serialize(); //layui jquery bug
        var name = encodeURIComponent($("input[name='name']").val());
        var ip = encodeURIComponent($("input[name='ip']").val());
        var port = encodeURIComponent($("input[name='port']").val());
        var password = encodeURIComponent($("input[name='password']").val());
        var str = `name=${name}&ip=${ip}&port=${port}&password=${password}`;
        $.post("/api/config/set", str, function (data) {
            if (data.Code === 1) {
                parent.location.reload();
            }
            else if (data.Code === 3) {
                layer.msg("操作失败:" + data.Message, { time: 2000, shade: 0.3, shadeClose: false }, function () {
                    top.location.href = "/login.html";
                });
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });
    });

    //提交删除redis表单
    $("#rem_btn").on("click", function () {
        layer.confirm("确认要删除redis服务器么?", {
            btn: ['确定', '取消']
        },
            function (index) {
                layer.close(index);
                var json = $("#add_form").serialize();
                $.post("/api/config/rem", json, function (data) {
                    if (data.Code === 1) {
                        parent.location.reload();
                    }
                    else if (data.Code === 3) {
                        layer.msg("操作失败:" + data.Message, { time: 2000, shade: 0.3, shadeClose: false }, function () {
                            top.location.href = "/login.html";
                        });
                    }
                    else {
                        layer.msg("操作失败:" + data.Message, { time: 2000 });
                    }
                });
            }
        );
    });
    //导入redis表单
    $("#conf_btn").on("click", function () {
        var configs = encodeURIComponent($("textarea[name='configs']").val());
        var str = `configs=${configs}`;
        $.post("/api/config/SetConfigs", str, function (data) {
            if (data.Code === 1) {
                parent.location.reload();
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });
    });
});

