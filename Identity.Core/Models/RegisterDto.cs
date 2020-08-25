using System.ComponentModel.DataAnnotations;

namespace Identity.Core.Models
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
