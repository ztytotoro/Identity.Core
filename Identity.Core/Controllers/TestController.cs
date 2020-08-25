using Identity.Core.Base;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Core.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public string[] Get()
        {
            return new string[]
            {
                "a",
                "b",
                "c"
            };
        }
    }
}
