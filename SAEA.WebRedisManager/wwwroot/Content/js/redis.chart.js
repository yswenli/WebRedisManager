layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var layerIndex = -1;

    layerIndex = layer.msg('加载中', {
        icon: 16
        , shade: 0.01
    });

    //
    function LineChart1(eId, chart_title, redis_info_url, redis_name) {

        var dom1 = document.getElementById(eId);
        if (dom1 === undefined) return;
        var myChart1 = echarts.init(dom1);
        var app1 = {};
        var option1 = null;
        option1 = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app1.timeTicket);

        app1.count = 22;

        app1.timeTicket = setInterval(function () {

            $.get(redis_info_url, "name=" + redis_name + "&isCpu=1", function (redis_info_data) {
                //
                var data0 = option1.series[0].data;
                if (redis_info_data.Code === 2) {
                    data0.shift();
                    data0.push(-1);
                }
                else {
                    //
                    data0.shift();
                    data0.push(redis_info_data.Data);
                }
            });
            option1.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option1.xAxis[0].data.push(axisData);
            myChart1.setOption(option1);

        }, 1100);


        if (option1 && typeof option1 === "object") {
            var startTime = +new Date();
            myChart1.setOption(option1, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }

    function LineChart2(eId, chart_title, redis_info_url, redis_name) {
        var dom2 = document.getElementById(eId);
        if (dom2 === undefined) return;
        var myChart2 = echarts.init(dom2);
        var app2 = {};
        var option2 = null;
        option2 = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app2.timeTicket);

        app2.count = 22;

        app2.timeTicket = setInterval(function () {            

            $.get(redis_info_url, "name=" + redis_name + "&isCpu=0", function (redis_info_data) {
                //
                var data0 = option2.series[0].data;
                if (redis_info_data.Code === 2) {
                    data0.shift();
                    data0.push(-1);
                }
                else {
                    //
                    data0.shift();
                    data0.push(redis_info_data.Data);
                }
            });
            option2.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option2.xAxis[0].data.push(axisData);
            myChart2.setOption(option2);

        }, 1200);


        if (option2 && typeof option2 === "object") {
            var startTime = +new Date();
            myChart2.setOption(option2, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }

    function LineChart11(eId, chart_title, redis_name) {

        var chatData = 0;

        var dom1 = document.getElementById(eId);
        if (dom1 === undefined) return;
        var myChart1 = echarts.init(dom1);
        var app1 = {};
        var option1 = null;
        option1 = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app1.timeTicket);

        app1.count = 22;

        app1.timeTicket = setInterval(function () {

            var data0 = option1.series[0].data;
            data0.shift();
            data0.push(chatData);
            option1.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option1.xAxis[0].data.push(axisData);
            myChart1.setOption(option1);

        }, 1100);


        if (option1 && typeof option1 === "object") {
            var startTime = +new Date();
            myChart1.setOption(option1, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }

        var ws = new WebSocket("ws://127.0.0.1:16666/");
        ws.onopen = function (evt) {
            console.log("Connection open ...");
            ws.send("getinfo");
            ws.send(`{"Name":"${redis_name}","IsCput":true}`);
        };
        ws.onmessage = function (event) {

            if (typeof event.data === String) {

                var redis_info_data = JSON.parse(event.data);

                if (redis_info_data.Code === 1) {
                    
                    chatData = redis_info_data.Data
                }
            }
            console.log("CPU Received Message: " + event.data);
        };
    }

    function LineChart22(eId, chart_title, redis_name) {

        var chatData = 0;

        var dom2 = document.getElementById(eId);
        if (dom2 === undefined) return;
        var myChart2 = echarts.init(dom2);
        var app2 = {};
        var option2 = null;
        option2 = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app2.timeTicket);

        app2.count = 22;

        app2.timeTicket = setInterval(function () {

            var data0 = option2.series[0].data;
            data0.shift();
            data0.push(chatData);
            option2.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option2.xAxis[0].data.push(axisData);
            myChart2.setOption(option2);

        }, 1200);


        if (option2 && typeof option2 === "object") {
            var startTime = +new Date();
            myChart2.setOption(option2, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
        var ws = new WebSocket("ws://127.0.0.1:16666/");
        ws.onopen = function (evt) {
            console.log("Connection open ...");
            ws.send("getinfo");
            ws.send(`{"Name":"${redis_name}","IsCput":false}`);
        };
        ws.onmessage = function (event) {
            if (typeof event.data === String) {

                var redis_info_data = JSON.parse(event.data);

                if (redis_info_data.Code === 1) {
                    chatData = redis_info_data.Data
                }
            }
            console.log("MEM Received Message: " + event.data);
        };
    }

    var name = decodeURI(GetRequest().name);

    //加载图表
    //LineChart1("redis-cpu-div", "cpu使用情况", "/api/redis/getinfo", name);
    //LineChart2("redis-mem-div", "memory使用情况", "/api/redis/getinfo", name);

    LineChart11("redis-cpu-div", "cpu使用情况", name);
    LineChart22("redis-mem-div", "memory使用情况", name);


    $("#redis_name").html(name);


    layer.close(layerIndex);

    $("#redis_info").on("click", function () {
        var redis_info_url = "/api/redis/getinfostring?name=" + name;
        $.get(redis_info_url, null, function (rdata) {
            if (rdata.Code === 1) {
                layer.alert("#当前配置信息：<br/>" + JSON.stringify(rdata.Data.Config) + "<br/>" + rdata.Data.Info, { icon: 1, title: 'Redis服务器信息', area: 'auto', maxWidth: '500px', resize: false, scrollbar: true });
            }
            else {
                layer.msg("操作失败:" + rdata.Message, { time: 2000 });
            }
        });
    });

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
            content: [`/console.html?name=${name}`, 'no']
        }));
    });


    $("#redis_clients").on("click", function () {
        var redis_info_url = "/api/redis/getclients?name=" + name;
        $.get(redis_info_url, null, function (rdata) {
            if (rdata.Code === 1) {
                layer.alert(rdata.Data, { icon: 1, title: 'Redis当前客户端连接信息', area: '500px', resize: false, scrollbar: true });
            }
            else {
                layer.msg("操作失败:" + rdata.Message, { time: 2000 });
            }
        });
    });


    $("#alter_passwords").click(function () {

        var nodeid = $(this).attr("data-nodeid");

        var addNodeHtml = `<form id="add_node_form" onSubmit="return false;"><table class="layui-table">
<tr><td>Passwords</td><td><input name="pwd1" type="password" autocomplete="off" placeholder="Passwords0!" class="layui-input" lay-verify="required" value="" /></td></tr>
<tr><td>Passwords Confirm</td><td><input name="pwd2" type="password" autocomplete="off" placeholder="Passwords0!" class="layui-input" lay-verify="required" value="" /></td></tr></table>
          </form>`;
        layer.open({
            title: 'Alter Passwords ',
            type: 1,
            area: ['460px', '260px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: addNodeHtml,
            btn: ['yes', 'no'],
            yes: function (index, layero) {

                $.post(`/api/redis/alterpwd?name=${encodeURI(name)}`, $("#add_node_form").serialize(), function (rdata) {
                    if (rdata.Code === 1) {
                        if (rdata.Data === true) {
                            layer.msg("操作成功!");
                            setInterval(() => { location.reload(); }, 2000);
                        }
                        else {
                            layer.msg("操作失败,当前服务器配置不正确!");
                        }
                    }
                    else {
                        layer.msg("操作失败：" + rdata.Message);
                    }
                });

            },
            no: function (index, layero) {
                layer.close(index);
            }
        });
    });
});