using ErrorHandler;
using Identity.Core.Base;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Core.Controllers
{
    public class TestController : BaseController
    {
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpGet]
        public string[] Get()
        {
            throw new BusinessException(ErrorCodes.UserNotFound);
        }
    }
}
