using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using BancoCentral.Models;

namespace BancoCentral.Controllers
{
    public class ClientesController : Controller
    {
        private BancoCentralEntities db = new BancoCentralEntities();
        private EmailController emailController = new EmailController();
        private WebServiceController webService = new WebServiceController();
        private IndicadorController indicadorController = new IndicadorController();

        // GET: Clientes
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.Distrito);
            return View(cliente.ToList());
        }

        public void login(Cliente cliente)
        {
           Session["nombreCliente"] = cliente.nombre + " " + cliente.apellidoPaterno;
        }

        public ActionResult CerrarSesion()
        {
            Session["nombreCliente"] = null;

            return RedirectToAction("Index", "Home");
        }

        // GET: Clientes/Details/5
        public void CorreosClientes()
        {
            List<Cliente> clientes = findUsers();
            string lastDate = "";
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd");

            try
            {

                DateTime llenarIndicadores = db.Database.SqlQuery<DateTime>("select top 1 ISNULL(MAX(fecha), ' ') from Indicador;").FirstOrDefault();

                if (DateTime.Compare(llenarIndicadores, Convert.ToDateTime("1/1/1900 00:00:00")) == 0)
                {

                    indicadorController.CreateIndicador(webService.ObtenerIndicadoresWebService("01/01/2010", nowDate));

                    if (clientes == null)
                    {
                        return;
                    }

                    webService.WebServiceCorreo(indicadorController.ObtenerIndicadoresBaseDatos(nowDate, nowDate), clientes);
                    return;
                }

                if (clientes == null)
                {
                    return;
                }
                else
                {
                    DateTime lastDateAux = db.Database.SqlQuery<DateTime>("select top 1 ISNULL(MAX(fecha), ' ') from ClienteIndicador;").FirstOrDefault();


                    if (DateTime.Compare(lastDateAux, Convert.ToDateTime("1/1/1900 00:00:00")) == 0)
                    {
                        webService.WebServiceCorreo(indicadorController.ObtenerIndicadoresBaseDatos(nowDate, nowDate), clientes);
                        return;
                    }
                    else
                    {
                        lastDate = Convert.ToString(lastDateAux.ToString("yyyy/MM/dd"));
                    }

                    if (!lastDate.Equals(nowDate))
                    {
                        lastDateAux = lastDateAux.AddDays(1);
                        lastDate = Convert.ToString(lastDateAux.ToString("yyyy/MM/dd"));

                        indicadorController.CreateIndicador(webService.ObtenerIndicadoresWebService(lastDate, nowDate));

                        webService.WebServiceCorreo(indicadorController.ObtenerIndicadoresBaseDatos(lastDate, nowDate), clientes);
                    }
                    else
                    {
                        return;
                    }

                }
            }
            catch (Exception e)
            {

            }
        }

        public List<Cliente> findUsers()
        {
            List<Cliente> clientes = new List<Cliente>();

            clientes = db.Database.SqlQuery<Cliente>("SELECT * FROM Cliente").ToList();
            if (clientes == null)
            {
                return null;
            }
            else
            {
                return clientes;
            }
        }

        public Cliente findUser(string email)
        {
            Cliente cliente = db.Database.SqlQuery<Cliente>("SELECT * FROM Cliente WHERE correo = '" + email + "'").FirstOrDefault();
            if (cliente == null)
            {
                return null;
            }
            else
            {
                return cliente;
            }
        }

        public bool findUserBool(string email)
        {
            Cliente cliente = db.Database.SqlQuery<Cliente>("SELECT * FROM Cliente WHERE correo = '" + email + "'").FirstOrDefault();
            if (cliente == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.distritoId = new SelectList(db.Distrito, "idDistrito", "nombre");
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCliente,nombre,apellidoPaterno,apellidoMaterno,cedula,correo,profesion,distritoId")] Cliente cliente)
        {

            if (findUser(cliente.correo) != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();
                emailController.sendEmail("Confirmar cuenta banco central","Cuenta de "+ cliente.nombre +  " creada con éxito", cliente.correo);
            }
            return RedirectToAction("Index","Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
