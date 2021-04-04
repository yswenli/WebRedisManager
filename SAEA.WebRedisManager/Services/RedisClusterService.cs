/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Services
*类 名 称：RedisClusterService
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SAEA.Common;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.RedisSocket.Model;

namespace SAEA.WebRedisManager.Services
{
	class RedisClusterService
	{
		/// <summary>
		/// 获取cluster 节点信息
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public JsonResult<List<ClusterNode>> GetClusterNodes(string name)
		{
			try
			{
				List<ClusterNode> result = new List<ClusterNode>();

				if (CurrentRedisClient.IsCluster(name))
				{
					result = CurrentRedisClient.GetClusterNodes(name);
				}

				return new JsonResult<List<ClusterNode>>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("获取cluster 节点信息", ex, name);
				return new JsonResult<List<ClusterNode>>() { Code = 2, Message = ex.Message };
			}
		}

		/// <summary>
		/// 添加节点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="ipPort"></param>
		/// <returns></returns>
		public JsonResult<bool> AddMaster(string name, string ipPort)
		{
			try
			{
				var result = false;

				if (CurrentRedisClient.IsCluster(name))
				{
					var ipp = ipPort.ToIPPort();

					result = CurrentRedisClient.AddNode(name, ipp.Item1, ipp.Item2);
				}

				return new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("添加节点", ex, name, ipPort);
				return new JsonResult<bool>() { Code = 2, Message = ex.Message };
			}
		}

		/// <summary>
		/// 添加从节点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="slaveNodeID"></param>
		/// <param name="masterID"></param>
		/// <returns></returns>
		public JsonResult<bool> AddSlave(string name, string slaveNodeID, string masterID)
		{
			try
			{
				var result = false;

				if (CurrentRedisClient.IsCluster(name))
				{
					result = CurrentRedisClient.AddSlave(name, slaveNodeID, masterID);
				}

				return new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("添加从节点", ex, name, slaveNodeID, masterID);
				return new JsonResult<bool>() { Code = 2, Message = ex.Message };
			}
		}

		/// <summary>
		/// 删除节点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nodeID"></param>
		/// <returns></returns>
		public JsonResult<bool> DeleteNode(string name, string nodeID)
		{
			try
			{
				var result = false;

				if (CurrentRedisClient.IsCluster(name))
				{
					result = CurrentRedisClient.DeleteNode(name, nodeID);
				}

				return new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("删除节点", ex, name, nodeID);
				return new JsonResult<bool>() { Code = 2, Message = ex.Message };
			}
		}

		/// <summary>
		/// 添加槽点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nodeID"></param>
		/// <param name="slotStr"></param>
		/// <returns></returns>
		public JsonResult<bool> AddSlots(string name, string nodeID, string slotStr)
		{
			try
			{
				var result = false;

				if (CurrentRedisClient.IsCluster(name))
				{
					var slotList = new List<int>();

					var index = slotStr.IndexOf("-");

					if (index > -1)
					{
						var begin = int.Parse(slotStr.Substring(0, index));

						var end = int.Parse(slotStr.Substring(index + 1));

						for (int i = begin; i <= end; i++)
						{
							slotList.Add(i);
						}
					}
					else
					{
						var slotArr = slotStr.Split(",", StringSplitOptions.RemoveEmptyEntries);
						if (slotArr != null && slotArr.Any())
						{
							foreach (var item in slotArr)
							{
								slotList.Add(int.Parse(item));
							}
						}
					}
					result = CurrentRedisClient.AddSlots(name, nodeID, slotList.ToArray());
				}

				return new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("添加槽点", ex, name, nodeID,slotStr);
				return new JsonResult<bool>() { Code = 2, Message = ex.Message };
			}
		}

		/// <summary>
		/// 删除槽点
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nodeID"></param>
		/// <param name="slotStr"></param>
		public JsonResult<bool> DelSlots(string name, string nodeID, string slotStr)
        {
			try
			{
				var result = false;

				if (CurrentRedisClient.IsCluster(name))
				{
					var slotList = new List<int>();

					var index = slotStr.IndexOf("-");

					if (index > -1)
					{
						var begin = int.Parse(slotStr.Substring(0, index));

						var end = int.Parse(slotStr.Substring(index + 1));

						for (int i = begin; i <= end; i++)
						{
							slotList.Add(i);
						}
					}
					else
					{
						var slotArr = slotStr.Split(",", StringSplitOptions.RemoveEmptyEntries);
						if (slotArr != null && slotArr.Any())
						{
							foreach (var item in slotArr)
							{
								slotList.Add(int.Parse(item));
							}
						}
					}
					result = CurrentRedisClient.DelSlots(name, nodeID, slotList.ToArray());
				}

				return new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("删除槽点", ex, name, nodeID, slotStr);
				return new JsonResult<bool>() { Code = 2, Message = ex.Message };
			}
		}

		/// <summary>
		/// 保存配置
		/// </summary>
		/// <param name="name"></param>
		/// <param name="nodeID"></param>
		public JsonResult<bool> SaveConfig(string name, string nodeID)
        {
			try
			{
				var result = false;

				if (CurrentRedisClient.IsCluster(name))
				{
					result = CurrentRedisClient.SaveConfig(name, nodeID);
				}

				return new JsonResult<bool>() { Code = 1, Data = result, Message = "OK" };
			}
			catch (Exception ex)
			{
				LogHelper.Error("保存配置", ex, name, nodeID);
				return new JsonResult<bool>() { Code = 2, Message = ex.Message };
			}
		}

	}
}
