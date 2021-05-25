using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace StockService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class StockController : ControllerBase
    {

        public StockController()
        {
        }

    }
}
