using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    [Produces("application/json")]
    [Route("api/echo")]

    public class EchoController: Controller
    {
        private readonly IEchoActorSystem _echoActorSystem;
        private readonly ILogger _logger;

        public EchoController(IEchoActorSystem echoActorSystem, ILogger<EchoController> logger)
        {
            _echoActorSystem = echoActorSystem;
            _logger = logger;
        }

        [HttpGet("{val}")]
        public IActionResult Get(string val)
        {
            if (val == null)
            {
                return BadRequest("Expected format: /api/echo/<value>");
            }
            _echoActorSystem.Send(val);
            return Ok(new { message = "sent: " + val });
        }



        [HttpPost]
        public IActionResult Post([FromBody] EchoMessage msg)
        {
            if (msg == null)
            {
                return BadRequest("Expected format: {\"message\":\"...\"}");
            }

            _echoActorSystem.Send(msg.Message);

            return Ok(new {message = "sent: " + msg});
        }

        public class EchoMessage
        {
            public String Message { get; set; }
        }

    }
}
