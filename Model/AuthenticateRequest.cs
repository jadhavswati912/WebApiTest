using System.ComponentModel;

namespace Product_Details.Model
{
    public class AuthenticateRequest
    {
        [DefaultValue("System")]
        public  required string UserName { get; set; }
        [DefaultValue("System")]
        public required string Password { get; set; }
    }
}
