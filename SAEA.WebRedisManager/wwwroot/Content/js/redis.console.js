layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var redis_name = encodeURI(GetRequest().name);

    var layerIndex = -1;

    $("#cmdTxt").keypress(function (e) {
        if (e.which === 13) {
            $("#runBtn").click();
        }
    });

    $("#runBtn").click(function () {

        var cmd = $("#cmdTxt").val();

        if (cmd !== undefined && cmd !== "") {

            cmd = encodeURI(cmd);

            layerIndex = layer.msg('加载中', {
                icon: 16
                , shade: 0.01
            });            

            $.post("/console/sendcmd", `name=${redis_name}&cmd=${cmd}`, (result) => {
                var v = `输入：\r\n ${decodeURI(cmd)
                    }\r\n\r\n输出：\r\n${result}\r\n\r\n${$("#resultTxt").val()}`;
                $("#resultTxt").val(v);
                $("#resultTxt")[0].scrollTop = 0;
                layer.close(layerIndex);
            });
        }

    });


    $("#clearBtn").click(() => {
        $("#resultTxt").val("");
    });

});