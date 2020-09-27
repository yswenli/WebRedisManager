layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var layerIndex = -1;

    layerIndex = layer.msg('加载中', {
        icon: 16
        , shade: 0.01
    });


    var chatData1 = 0;
    var chatData2 = 0;
    var chatData3 = 0;
    var chatData41 = 0;
    var chatData42 = 0;

    function LineChart1(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
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
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
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
                    name: '%',
                    max: 100,
                    min: 0,
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: '%',
                    type: 'line',
                    smooth: true,
                    symbol: 'none',
                    sampling: 'average',
                    itemStyle: {
                        color: 'rgb(255, 70, 131)'
                    },
                    areaStyle: {
                        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                            offset: 0,
                            color: 'rgb(255, 158, 68)'
                        }, {
                            offset: 1,
                            color: 'rgb(255, 70, 131)'
                        }])
                    },
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

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData1);
            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    function LineChart2(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
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
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
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
                    name: 'mb',
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: 'mb',
                    type: 'line',
                    smooth: true,
                    symbol: 'none',
                    sampling: 'average',
                    itemStyle: {
                        color: 'rgb(255, 70, 131)'
                    },
                    areaStyle: {
                        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                            offset: 0,
                            color: 'rgb(255, 158, 68)'
                        }, {
                            offset: 1,
                            color: 'rgb(255, 70, 131)'
                        }])
                    },
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

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData2);
            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    function LineChart3(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
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
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
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
                    name: 'times',
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: 'times',
                    type: 'bar',
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

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData3);
            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    function LineChart4(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
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
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
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
                    boundaryGap: false,
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
                    name: 'kb',
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: 'input',
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
                },
                {
                    name: 'ouput',
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

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData41);

            var data1 = option.series[1].data;
            data1.shift();
            data1.push(chatData42);

            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    var name = decodeURI(GetRequest().name);

    //加载图表

    LineChart1("redis-cpu-div", "cpu");
    LineChart2("redis-mem-div", "memory");
    LineChart3("redis-cmd-div", "cmd");
    LineChart4("redis-net-div", "net");


    var ws = new WebSocket(`ws://${document.domain}:16666/`);

    ws.onopen = function (evt) {
        console.log("Connection open ...");
        ws.send(`${name}`);
    };
    ws.onmessage = function (event) {

        var redis_info_data = JSON.parse(event.data);

        if (redis_info_data.Code === 1) {
            chatData1 = redis_info_data.Data.Cpu;
            chatData2 = redis_info_data.Data.Memory;
            chatData3 = redis_info_data.Data.Cmds;
            chatData41 = redis_info_data.Data.Input;
            chatData42 = redis_info_data.Data.Output;
        }
    };


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