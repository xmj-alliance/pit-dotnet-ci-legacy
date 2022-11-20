using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {

        private readonly IAppSecret appSecret;

        public APIController(IAppSecret appSecret)
        {
            this.appSecret = appSecret;
        }

        [HttpGet]
        public CommonMessage ReadyState()
        {
            var message = new CommonMessage() {
                OK = true,
                Message = "API works!",
            };

            if (appSecret is null || appSecret.Title is null) {
                message.Message += " But we haven't not heard any secret.";
            } else {
                var secretIntro = appSecret.IsHighlyClassified ? 
                $" And we know \"{appSecret.Value}\" is highly classified with level {appSecret.Level} clearance.":
                $" And we know \"{appSecret.Value}\" is just a normal secret.";
                message.Message += secretIntro;
            }
            
            return message;
        }
    }

}