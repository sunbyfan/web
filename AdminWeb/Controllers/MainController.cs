using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminWeb.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminWeb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IAdminService _service;
        public MainController(IAdminService service)
        {
            _service = service;
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new {
                name="admin",
                department= "研发部",
                avatar="https://img.alicdn.com/tfs/TB1L6tBXQyWBuNjy0FpXXassXXa-80-80.png",
                userid= 10001,
            });
        }

        // GET api/v1/[controller]/problems[?pageSize=3&pageIndex=10]
        [HttpGet("problems")]
        public async Task<IActionResult> Problems([FromQuery]int pageSize = 10
            , [FromQuery]int pageIndex = 0, [FromQuery] int proType=0)
        {
            string[] asc = new string[2] { "ProblemType", "QuestionNumber" };
            var (rows,totalCount)=await _service.GetProblemList(proType,pageIndex, pageSize, asc);
            return Ok(new {rows, totalCount});
        }

        [HttpGet("users")]
        public async Task<IActionResult> Users([FromQuery]int pageSize = 10
            , [FromQuery]int pageIndex = 0)
        {
            string[] asc = new string[1] { "OrderNumber" };
            var (rows, totalCount) = await _service.GetUsersList(pageIndex, pageSize, asc);
            return Ok(new { rows, totalCount });
        }


    }
}