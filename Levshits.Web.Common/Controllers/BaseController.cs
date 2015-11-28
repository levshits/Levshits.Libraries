using System.Web.Mvc;
using Levshits.Logic.Common;
using Levshits.Logic.Interfaces;

namespace Levshits.Web.Common.Controllers
{
    public abstract class BaseController : Controller
    {
        public ICommandBus CommandBus { get; set; }
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}