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

            var t1 = new Date();

            var input = `${t1.Format("yyyy-MM-dd hh:mm:ss.S")}\r\nCommand:${decodeURI(cmd)},`;

            $.post("/console/sendcmd", `name=${redis_name}&cmd=${cmd}`, (result) => {

                var t2 = new Date();

                var output = `Cost:${t2.getTime() - t1.getTime()},Result:\r\n${result}\r\n\r\n${$("#resultTxt").val()}`;

                $("#resultTxt").val(input + output);
                $("#resultTxt")[0].scrollTop = 0;

                layer.close(layerIndex);
            });
        }

    });


    $("#clearBtn").click(() => {
        $("#resultTxt").val("");
    });

    $(".autocomplete").keyup(function () {

        var cur = $(this);

        var wordStr = cur.val();

        if ($("#autocomplete_div").html() === undefined) {
            $("body").append("<div id='autocomplete_div' style='position:absolute;margin:0px;padding:3px;border:1px solid #ccc;display:block;width:150px;min-height:50px;height:auto;overflow:hidden;background:#fafafa;'></div>")
        }

        $("#autocomplete_div").css("left", cur.offset().left).css("top", cur.offset().top + 35);

        $("#autocomplete_div").mouseleave(function () {
            $("#autocomplete_div").slideUp(300);
        });

        $("#autocomplete_div").html("");

        $.post("/api/console/getcmd?t=" + (new Date().getMilliseconds()), { input: wordStr }, function (data) {
            if (data.Code === 1) {
                var dhtml = "";
                data.Data.forEach(function (element) {
                    dhtml += `<div>${element}</div>`;
                });
                $("#autocomplete_div").html(dhtml).slideDown(300);
                $("#autocomplete_div div").hover(function () { $(this).css({ "background": "#0563C1", "color": "#fff", "cursor":"pointer" }); }, function () { $(this).css({ "background": "#fff", "color": "#000" }); });
            }
            else {
                $("#autocomplete_div").slideUp(300);
            }
            $("#autocomplete_div div").click(function () {
                cur.val($(this).html());
                $("#autocomplete_div").slideUp(300);
            });
        });
    });
    $(".autocomplete").click(function () {
        $(this).keyup();
    });
});