/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Controllers
*类 名 称：RedisClusterController
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2020/1/6 13:39:36
*描述：
*=====================================================================
*修改时间：2020/1/6 13:39:36
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.MVC;
using SAEA.WebRedisManager.Attr;
using SAEA.WebRedisManager.Services;

namespace SAEA.WebRedisManager.Controllers
{
    /// <summary>
    /// Redis cluster controller
    /// </summary>
    [Auth(true)]
    public class RedisClusterController : Controller
    {
        /// <summary>
        /// 获取cluster 节点信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult GetClusterNodes(string name)
        {
            return Json(new RedisClusterService().GetClusterNodes(name));
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ipPort"></param>
        /// <returns></returns>
        public ActionResult AddMaster(string name, string ipPort)
        {
            return Json(new RedisClusterService().AddMaster(name, ipPort));
        }

        /// <summary>
        /// 添加从节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="slaveNodeID"></param>
        /// <param name="masterID"></param>
        /// <returns></returns>
        public ActionResult AddSlave(string name, string slaveNodeID, string masterID)
        {
            return Json(new RedisClusterService().AddSlave(name, slaveNodeID, masterID));
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public ActionResult DeleteNode(string name, string nodeID)
        {
            return Json(new RedisClusterService().DeleteNode(name, nodeID));
        }

        /// <summary>
        /// 添加槽点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nodeID"></param>
        /// <param name="slotStr"></param>
        /// <returns></returns>
        public ActionResult AddSlots(string name, string nodeID, string slotStr)
        {
            return Json(new RedisClusterService().AddSlots(name, nodeID, slotStr));
        }

        /// <summary>
        /// 删除槽点
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nodeID"></param>
        /// <param name="slotStr"></param>
        /// <returns></returns>
        public ActionResult DelSlots(string name, string nodeID, string slotStr)
        {
            return Json(new RedisClusterService().DelSlots(name, nodeID, slotStr));
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public ActionResult SaveConfig(string name, string nodeID)
        {
            return Json(new RedisClusterService().SaveConfig(name, nodeID));
        }
    }
}
