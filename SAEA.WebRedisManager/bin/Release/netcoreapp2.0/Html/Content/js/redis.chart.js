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
        if (dom1 == undefined) return;
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
                    name: '占用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '占用率',
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

            axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');

            $.get(redis_info_url, "name=" + redis_name + "&isCpu=1", function (redis_info_data) {
                //debugger;
                var data0 = option1.series[0].data;
                if (redis_info_data.Code == 2) {
                    data0.shift();
                    data0.push(-1);
                }
                else {
                    //debugger;
                    data0.shift();
                    data0.push(redis_info_data.Data);
                }
            });
            option1.xAxis[0].data.shift();
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
        if (dom2 == undefined) return;
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
                    name: '占用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '占用率',
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

            axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');

            $.get(redis_info_url, "name=" + redis_name + "&isCpu=0", function (redis_info_data) {
                //debugger;
                var data0 = option2.series[0].data;
                if (redis_info_data.Code == 2) {
                    data0.shift();
                    data0.push(-1);
                }
                else {
                    //debugger;
                    data0.shift();
                    data0.push(redis_info_data.Data);
                }
            });
            option2.xAxis[0].data.shift();
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

    var name = GetRequest().name;

    //加载图表
    LineChart1("redis-cpu-div", "cpu使用情况", "/api/redis/getinfo", name);
    LineChart2("redis-mem-div", "memory使用情况", "/api/redis/getinfo", name);

    $("#redis_name").html(name);

    layer.close(layerIndex);

    $("#redis_name").on("click", function () {
        var redis_info_url = "/api/redis/GetInfoString?name=" + name;
        $.get(redis_info_url, null, function (rdata) {
            debugger;
            if (rdata.Code === 1) {
                layer.alert("#当前配置信息：<br/>" + JSON.stringify(rdata.Data.Config) + "<br/>" + rdata.Data.Info, { title: 'Redis服务器信息', maxWidth: '500px' });
            }
            else {
                layer.msg("操作失败:" + rdata.Message, { time: 2000 });
            }
        });
    });

    //redis cluster
    function getClusterNodes() {
        var redis_nodes_url = "/api/RedisCluster/GetClusterNodes?name=" + name;
        $.get(redis_nodes_url, null, function (rdata) {
            if (rdata.Code === 1) {
                var tbody = "";
                for (let item of rdata.Data) {
                    tbody += `<tr><td>${item.NodeID}</td><td>${item.IPPort}</td><td>${item.Status}</td><td>${item.IsMaster}</td><td>${item.MinSlots}</td><td>${item.MaxSlots}</td><td>${item.MasterNodeID}</td><td>DeleteNode<br/>、Replicate<br/>、MigratingSlots<br/>、ImportingSlots</td></tr>`;
                }
                $("#redis-data-body").html(tbody);
            }
            else {
                layer.msg("操作失败:" + rdata.Message, { time: 2000 });
            }
        });
    }
    getClusterNodes();

});