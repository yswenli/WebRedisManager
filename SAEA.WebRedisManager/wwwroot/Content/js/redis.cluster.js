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

                $.post("/api/rediscluster/addmaster", $("#add_node_form").serialize(), function (rdata) {
                    if (rdata.Code === 1) {
                        if (rdata.Data === true) {
                            layer.msg("添加成功!");
                        }
                        else {
                            layer.msg("添加失败!");
                        }
                    }
                    else {
                        layer.msg("添加失败：" + rdata.Message);
                    }
                });

            },
            no: function (index, layero) {
                layer.close(index);
            }
        });
    });

});