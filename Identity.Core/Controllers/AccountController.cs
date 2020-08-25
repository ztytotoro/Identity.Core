using Identity.Core.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<IdentityCoreUser> _manager;

        public AccountController(ILogger<AccountController> logger, UserManager<IdentityCoreUser> manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet]
        public IEnumerable<IdentityCoreUser> Get()
        {
            return _manager.Users.ToArray();
        }
    }
}
