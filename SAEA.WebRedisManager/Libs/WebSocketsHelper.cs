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
        ConcurrentDictionary<string, DateTime> _dic1 = new ConcurrentDictionary<string, DateTime>();

        WSServer _wsServer = null;

        public WebSocketsHelper(int port = 16666)
        {
            _wsServer = new WSServer(port);
            _wsServer.OnConnected += _wsServer_OnConnected;
            _wsServer.OnMessage += WsServer_OnMessage;
            _wsServer.OnDisconnected += WsServer_OnDisconnected;
        }

        private void _wsServer_OnConnected(string cid)
        {
            _dic1.TryAdd(cid, DateTimeHelper.Now);
        }

        private void WsServer_OnDisconnected(string cid)
        {
            _dic1.TryRemove(cid, out DateTime dt);
        }

        private void WsServer_OnMessage(string cid, WebSocket.Model.WSProtocal msg)
        {
            if (msg != null)
            {
                if (_dic1.ContainsKey(cid) && msg.Content != null && msg.Content.Any())
                {
                    var name = Encoding.UTF8.GetString(msg.Content);

                    if (string.IsNullOrEmpty(name))
                    {
                        return;
                    }

                    Task.Factory.StartNew(() =>
                    {
                        while (_dic1.ContainsKey(cid))
                        {
                            try
                            {
                                var data = SerializeHelper.Serialize(ServerInfoDataHelper.GetInfo(name));

                                _wsServer.Reply(cid, new WSProtocal(WSProtocalType.Text, Encoding.UTF8.GetBytes(data)));

                            }
                            catch
                            {
                                _dic1.TryRemove(cid, out DateTime v);
                                break;
                            }
                            ThreadHelper.Sleep(1000);
                        }
                    });
                }
                else
                {
                    _wsServer.Disconnect(cid);
                }
            }
        }

        public void Start()
        {
            _wsServer.Start();
        }

        public void Stop()
        {
            _wsServer.Stop();
        }
    }
}
