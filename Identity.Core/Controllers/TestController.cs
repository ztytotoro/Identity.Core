using ErrorHandler;
using Identity.Core.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Core.Controllers
{
    public class TestController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public string[] Get()
        {
            throw new BusinessException(ErrorCodes.UserNotFound);
        }
    }
}
