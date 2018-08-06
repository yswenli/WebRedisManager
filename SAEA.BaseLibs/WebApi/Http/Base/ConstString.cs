/****************************************************************************
*Copyright (c) 2018 Microsoft All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WENLI-PC
*公司名称：Microsoft
*命名空间：SAEA.WebAPI.Http.Base
*文件名： ConstString
*版本号： V1.0.0.0
*唯一标识：4eebbaa7-1781-4521-ab57-4bc9c8d43a84
*当前的用户域：WENLI-PC
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2018/4/16 13:31:23
*描述：
*
*=====================================================================
*修改标记
*修改时间：2018/4/16 13:31:23
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace SAEA.WebAPI.Http.Base
{
    internal struct ConstString
    {
        public const string GETStr = "GET";

        public const string POSTStr = "POST";

        public const string OPTIONSStr = "OPTIONS";



        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        public const string FORMENCTYPE1 = "application/x-www-form-urlencoded";

        /// <summary>
        /// multipart/form-data
        /// </summary>
        public const string FORMENCTYPE2 = "multipart/form-data";

        /// <summary>
        /// application/json
        /// </summary>
        public const string FORMENCTYPE3 = "application/json";
    }
}
