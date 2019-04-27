using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BancoCentral.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ClientesController clientes = new ClientesController();
            clientes.CorreosClientes();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}