using Core.Template.IServices.Base;
using Core.Template.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Template.IServices
{
    public interface IUserInfoService : IBaseService
    {
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        UserInfo QueryUserInfo();
    }
}
