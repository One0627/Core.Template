using Core.Template.IServices;
using Core.Template.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Template.Services
{
    public class UserInfoService : IUserInfoService
    {
        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UserInfo QueryUserInfo()
        {
            return new UserInfo() { Account="admin",Age=18, UserName="管理员",Password="123456" };
        }
    }
}
