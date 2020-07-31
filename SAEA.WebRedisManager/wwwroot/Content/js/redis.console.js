// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(H)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd HH:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d H:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

//
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

            var input = `${new Date().Format("yyyy-MM-dd HH:mm:ss")}输入：\r\n ${decodeURI(cmd)}\r\n\r\n`;

            $.post("/console/sendcmd", `name=${redis_name}&cmd=${cmd}`, (result) => {

                var output = `${new Date().Format("yyyy-MM-dd HH:mm:ss")}输出：\r\n${result}\r\n\r\n${$("#resultTxt").val()}`;

                $("#resultTxt").val(input + output);
                $("#resultTxt")[0].scrollTop = 0;

                layer.close(layerIndex);
            });
        }

    });


    $("#clearBtn").click(() => {
        $("#resultTxt").val("");
    });

});