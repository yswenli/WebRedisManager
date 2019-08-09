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
                    //$(".cluster-content").hide();
                }
                for (let item of rdata.Data) {
                    tbody += `<tr><td>${item.NodeID}</td><td>${item.IPPort}</td><td>${item.Status}</td><td>${item.IsMaster}</td><td>${item.MinSlots}</td><td>${item.MaxSlots}</td><td>${item.MasterNodeID}</td><td>
<a class="delete_node" href="javascript:;" data-nodeid="${item.NodeID}" >DeleteNode</a><br/>、<a class="save_config" href="javascript:;" data-nodeid="${item.NodeID}" >SaveConfig</a><br/>、MigratingSlots<br/>、ImportingSlots</td></tr>`;
                }
                $("#redis-data-body").html(tbody);


                //delete node
                $(".delete_node").click(function () {
                    var nodeid = $(this).attr("data-nodeid");
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
                });

                //save config
                $(".save_config").click(function () {
                    var nodeid = $(this).attr("data-nodeid");
                    $.post(`/api/rediscluster/saveconfig?name=${encodeURI(name)}`, null, function (rdata) {
                        if (rdata.Code === 1) {
                            if (rdata.Data === true) {
                                layer.msg("操作成功!");
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

    $("#add_master").click(function () {
        var addNodeHtml = `<form id="add_node_form"><table class="layui-table"></tr><tr><td>IpPort</td><td><input name="IpPort" type="text" autocomplete="off" placeholder="127.0.0.1:6379" class="layui-input" lay-verify="required" value="" /></td></tr></table>
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

    $("#add_slave").click(function () {
        var addNodeHtml = `<form id="add_node_form"><table class="layui-table">
<tr><td>SlaveNodeID</td><td><input name="SlaveNodeID" type="text" autocomplete="off" placeholder="" class="layui-input" lay-verify="required" value="" /></td></tr>
<tr><td>MasterID</td><td><input name="MasterID" type="text" autocomplete="off" placeholder="" class="layui-input" lay-verify="required" value="" /></td></tr></table>
          </form>`;
        layer.open({
            title: 'Add slave node for redis cluster ',
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

                $.post(`/api/rediscluster/addslave?name=${encodeURI(name)}`, $("#add_node_form").serialize(), function (rdata) {
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