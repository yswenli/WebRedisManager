layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var name = decodeURI(GetRequest().name);



    //redis cluster
    function getClusterNodes() {
        var redis_nodes_url = "/api/rediscluster/getclusternodes?name=" + name;
        $.get(redis_nodes_url, null, function (rdata) {
            if (rdata.Code === 1) {

                var tbody = "";

                if (rdata.Data.length === 0) {
                    $(".cluster-content").hide();
                    return;
                }

                for (let item of rdata.Data) {
                    if (item.MasterNodeID === null) {
                        tbody += `<tr><td>${item.NodeID}</td><td>${item.IPPort}</td><td>${item.Status}</td><td>${item.IsMaster}</td><td>${item.MinSlots}</td><td>${item.MaxSlots}</td><td>${item.MasterNodeID}</td><td>
<a class="add_slave" href="javascript:;" data-nodeid="${item.NodeID}" >BeSlaved</a><br/><a class="delete_node" href="javascript:;" data-nodeid="${item.NodeID}" >DeleteNode</a><br/><a class="add_slots" href="javascript:;" data-nodeid="${item.NodeID}">AddSlots</a><br/><a class="delete_slots" href="javascript:;" data-nodeid="${item.NodeID}">DeleteSlots</a><br/><a class="save_config" href="javascript:;" data-nodeid="${item.NodeID}" >SaveConfig</a></td></tr>`;
                    }
                    else {
                        tbody += `<tr style="background-color:#F5F5F5;"><td>${item.NodeID}</td><td>${item.IPPort}</td><td>${item.Status}</td><td>${item.IsMaster}</td><td>${item.MinSlots}</td><td>${item.MaxSlots}</td><td>${item.MasterNodeID}</td><td>
<a class="add_slave" href="javascript:;" data-nodeid="${item.NodeID}" >BeSlaved</a><br/>
<a class="delete_node" href="javascript:;" data-nodeid="${item.NodeID}" >DeleteNode</a></td></tr>`;
                    }

                }

                $("#redis-data-body").html(tbody);


                //add slave

                $(".add_slave").click(function () {

                    var nodeid = $(this).attr("data-nodeid");

                    var addNodeHtml = `<form id="add_node_form" onSubmit="return false;"><table class="layui-table">
<tr><td>MasterID</td><td><input name="MasterID" type="text" autocomplete="off" placeholder="" class="layui-input" lay-verify="required" value="" /></td></tr></table>
          </form>`;
                    layer.open({
                        title: 'Add slave node for redis cluster ',
                        type: 1,
                        area: ['460px', '200px'],
                        fixed: true,
                        resize: false,
                        move: false,
                        maxmin: false,
                        time: 0,
                        content: addNodeHtml,
                        btn: ['yes', 'no'],
                        yes: function (index, layero) {

                            $.post(`/api/rediscluster/addslave?name=${encodeURI(name)}&slavenodeid=${nodeid}`, $("#add_node_form").serialize(), function (rdata) {
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

                //delete node
                $(".delete_node").click(function () {

                    var nodeid = $(this).attr("data-nodeid");

                    layer.confirm('Are you sure you want to do this?', { icon: 3, title: 'WebRedisManager' }, function (index) {
                        $.post(`/api/rediscluster/deletenode?nodeid=${nodeid}&name=${encodeURI(name)}`, null, function (rdata) {
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
                        layer.close(index);
                    });
                });

                //add_slots
                $(".add_slots").click(function () {

                    var html = `<form id="add_slots" onSubmit="return false;"><table class="layui-table"></tr><tr><td>Slots</td><td><input name="SlotStr" type="text" autocomplete="off" placeholder="0-16383" class="layui-input" lay-verify="required" value="" /></td></tr></table></form>`;

                    var nodeid = $(this).attr("data-nodeid");

                    layer.open({
                        title: 'add slots',
                        type: 1,
                        area: ['460px', '200px'],
                        fixed: true,
                        resize: false,
                        move: false,
                        maxmin: false,
                        time: 0,
                        content: html,
                        btn: ['yes', 'no'],
                        yes: function (index, layero) {
                            $.post(`/api/rediscluster/addslots?nodeid=${nodeid}&name=${encodeURI(name)}`, $("#add_slots").serialize(), function (rdata) {
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

                //delete_slots
                $(".delete_slots").click(function () {
                    var html = `<form id="delete_slots" onSubmit="return false;"><table class="layui-table"></tr><tr><td>Slots</td><td><input name="SlotStr" type="text" autocomplete="off" placeholder="0-16383" class="layui-input" lay-verify="required" value="" /></td></tr></table></form>`;

                    var nodeid = $(this).attr("data-nodeid");

                    layer.open({
                        title: 'delete slots',
                        type: 1,
                        area: ['460px', '200px'],
                        fixed: true,
                        resize: false,
                        move: false,
                        maxmin: false,
                        time: 0,
                        content: html,
                        btn: ['yes', 'no'],
                        yes: function (index, layero) {

                            $.post(`/api/rediscluster/delslots?nodeid=${nodeid}&name=${encodeURI(name)}`, $("#delete_slots").serialize(), function (rdata) {
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

                //save config
                $(".save_config").click(function () {
                    var nodeid = $(this).attr("data-nodeid");
                    $.post(`/api/rediscluster/saveconfig?nodeid=${nodeid}&name=${encodeURI(name)}`, null, function (rdata) {
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
                });

            }
            else {
                layer.msg("操作失败:" + rdata.Message, { time: 2000 });
            }
        });
    }
    getClusterNodes();

    //add node
    $("#add_master").click(function () {
        var addNodeHtml = `<form id="add_node_form" onSubmit="return false;"><table class="layui-table"></tr><tr><td>IpPort</td><td><input name="IpPort" type="text" autocomplete="off" placeholder="127.0.0.1:6379" class="layui-input" lay-verify="required" value="" /></td></tr></table>
          </form>`;

        layer.open({
            title: 'add redis cluster node',
            type: 1,
            area: ['460px', '200px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: addNodeHtml,
            btn: ['yes', 'no'],
            yes: function (index, layero) {

                $.post(`/api/rediscluster/addmaster?name=${encodeURI(name)}`, $("#add_node_form").serialize(), function (rdata) {
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