using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminWeb
{
    public class AdminSettings
    {
        /// <summary>
        /// 秘钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 后台用户名
        /// </summary>
        public string AdminLoginName { get; set; }

        /// <summary>
        /// 后台密码
        /// </summary>
        public string AdminPassword { get; set; }
    }
}
