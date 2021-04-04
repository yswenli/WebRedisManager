/****************************************************************************
*项目名称：SAEA.WebRedisManager.Controllers
*CLR 版本：4.0.30319.42000
*机器名称：WALLE-PC
*命名空间：SAEA.WebRedisManager.Services
*类 名 称：ConsoleService
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

using SAEA.Common;
using SAEA.Redis.WebManager.Libs;
using SAEA.Redis.WebManager.Models;
using SAEA.WebRedisManager.Libs;

namespace SAEA.WebRedisManager.Services
{
    class ConsoleService
    {
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string SendCmd(string name, string cmd)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(cmd))
                {
                    return CurrentRedisClient.Send(name, cmd);
                }
                return "输入的命令不能为空~";
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConsoleService.SendCmd", ex, name, cmd);
                return "请求发生异常:" + ex.Message;
            }
        }
        /// <summary>
        /// 获取输入的命令
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public JsonResult<IEnumerable<string>> GetCMD(string input)
        {
            try
            {
                return new JsonResult<IEnumerable<string>>() { Code = 1, Data = RedisCmdHelper.GetList(input) };
            }
            catch (Exception ex)
            {
                LogHelper.Error("ConsoleService.GetCMD", ex, input);
                return new JsonResult<IEnumerable<string>>() { Code = 2, Message = ex.Message };
            }
        }
    }
}
