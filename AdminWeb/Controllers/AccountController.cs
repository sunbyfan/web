using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AdminWeb.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdminWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AdminSettings _settings;
        public AccountController(IOptionsSnapshot<AdminSettings> settings)
        {
            _settings = settings.Value;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginViewModel model)
        {
            var json = new mJsonResult();
            if (ModelState.IsValid)
            {
                if (model.username == _settings.AdminLoginName
                    && model.password == _settings.AdminPassword)
                {
                    var claims = new[]
                    {
                        new Claim("name", _settings.AdminLoginName),
                        new Claim("sub","admin"),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecurityKey));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                                    claims: claims,
                                    expires: DateTime.Now.AddDays(2),
                                    signingCredentials: creds
                                );
                    json.statusText = "ok";
                    json.currentAuthority = "admin";
                    json.token = new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    json.currentAuthority = "guest";
                    json.statusText = "用户名或密码错误！";
                }
            }

            return Ok(json);
        }

    }
}