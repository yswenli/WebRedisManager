/****************************************************************************
*项目名称：SAEA.WebRedisManager.Libs
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Libs
*类 名 称：WebSocketsHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/6/5 20:24:16
*描述：
*=====================================================================
*修改时间：2020/6/5 20:24:16
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.WebSocket;
using SAEA.WebSocket.Model;
using SAEA.WebSocket.Type;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.WebRedisManager.Libs
{
    public class WebSocketsHelper
    {
        ConcurrentDictionary<string, DateTime> _dic = new ConcurrentDictionary<string, DateTime>();

        WSServer _wsServer = null;

        public WebSocketsHelper(int port = 16380)
        {
            _wsServer = new WSServer(16380);
            _wsServer.OnMessage += WsServer_OnMessage;
            _wsServer.OnDisconnected += WsServer_OnDisconnected;
        }

        private void WsServer_OnDisconnected(string cid)
        {
            LogHelper.Error("WebSocketServer", new Exception("断开连接"), cid);
        }

        private void WsServer_OnMessage(string cid, WebSocket.Model.WSProtocal msg)
        {
            if (msg != null)
            {
                if (_dic.ContainsKey(cid) && msg.Content != null && msg.Content.Any())
                {
                    var json = Encoding.UTF8.GetString(msg.Content);

                    var requestData = SerializeHelper.Deserialize<RequestData>(json);

                    if (requestData == null)
                    {
                        return;
                    }

                    if (requestData.IsCpu)
                        Task.Factory.StartNew(() =>
                        {
                            while (true)
                            {
                                var data = SerializeHelper.Serialize(ServerInfoDataHelper.GetInfo(requestData.Name, requestData.IsCpu));

                                _wsServer.Reply(cid, new WSProtocal(WSProtocalType.Text, Encoding.UTF8.GetBytes(data)));

                                ThreadHelper.Sleep(1000);
                            }
                        });
                    else
                        Task.Factory.StartNew(() =>
                        {
                            while (true)
                            {
                                var data = SerializeHelper.Serialize(ServerInfoDataHelper.GetInfo(requestData.Name, requestData.IsCpu));

                                _wsServer.Reply(cid, new WSProtocal(WSProtocalType.Text, Encoding.UTF8.GetBytes(data)));

                                ThreadHelper.Sleep(1000);
                            }
                        });

                    return;
                }
                else
                {
                    if (msg.Content != null && msg.Content.Any())
                    {
                        var s = Encoding.UTF8.GetString(msg.Content);

                        if (s == "getinfo")
                        {
                            _dic.TryAdd(cid, DateTimeHelper.Now);

                            return;
                        }
                    }
                }
            }
            _wsServer.Disconnect(cid, new WSProtocal(WSProtocalType.Close, null));
        }

        public void Start()
        {
            _wsServer.Start();
        }

        public void Stop()
        {
            _wsServer.Stop();
        }


        public class RequestData
        {
            public string Name
            {
                get; set;
            }

            public bool IsCpu
            {
                get; set;
            }
        }
    }
}
