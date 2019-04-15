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
        private BancoCentralEntities1 db = new BancoCentralEntities1();

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
        public bool SearchUser(String email)
        {
            cr.fi.bccr.gee.wsIndicadoresEconomicos indicadores = new cr.fi.bccr.gee.wsIndicadoresEconomicos();


            if (email == null)
            {
                return false;
            }

            Cliente cliente = findUser(email);

            if (cliente == null)
            {
                return false;
            }
            else
            {
                string lastDate = Convert.ToString(cliente.ultimaFechaIng);
                string nowDate = DateTime.Now.ToString("dd/MM/yyyy");

                if (!lastDate.Equals(nowDate))
                {

                    DataSet tablaIndicadores = new DataSet();

                    DataTable compra = indicadores.ObtenerIndicadoresEconomicos("317",
                                                                                lastDate,
                                                                                nowDate,
                                                                                cliente.nombre + " " + cliente.apellidoPaterno, "N").Tables[0].Copy();

                    DataTable venta = indicadores.ObtenerIndicadoresEconomicos("318",
                                                                                lastDate,
                                                                                nowDate,
                                                                                cliente.nombre + " " + cliente.apellidoPaterno, "N").Tables[0].Copy();

                    compra.TableName = "tablaCompra";
                    venta.TableName = "tablaVenta";

                    tablaIndicadores.Tables.Add(compra);
                    tablaIndicadores.Tables.Add(venta);

                    for (int i = 0; i < tablaIndicadores.Tables[0].Rows.Count; i++)
                    {

                        sendEmail("Indicadores Económicos",
                                  "<b>Señor:</b> " + cliente.nombre + " " + cliente.apellidoPaterno + " <b>cedula:</b> " + cliente.cedula + "<br>" + 
                                  " <b>Fecha Tipo Cambio:</b> " + Convert.ToString(tablaIndicadores.Tables[0].Rows[i].ItemArray[1]) + "<br>" +
                                  " <b>Tipo Cambio Compra:</b> " + Convert.ToString(tablaIndicadores.Tables[0].Rows[i].ItemArray[2]) + "<br>" +
                                   " <b>Tipo Cambio Venta:</b> " + Convert.ToString(tablaIndicadores.Tables[1].Rows[i].ItemArray[2]) + "<br><br>" +
                                   "<img src='https://activos.bccr.fi.cr/sitios/bccr/SiteAssets/bccr.jpg'>",
                                  cliente.correo);

                    }
                }

                int resultado = db.Database.ExecuteSqlCommand("UPDATE Cliente SET ultimaFechaIng = '" + DateTime.Now.ToString("yyyy/MM/dd") + "' WHERE idCliente = " + cliente.idCliente);

                if (resultado != 0)
                {

                    login(cliente);
                    return true;
                 
                }
                else
                {
                    return false;
                }
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
            cliente.ultimaFechaIng = DateTime.Now;

            if (findUser(cliente.correo) != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();
                sendEmail("Confirmar cuenta banco central","Cuenta de "+ cliente.nombre +  " creada con éxito", cliente.correo);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public void sendEmail(String asunto, String body, String correo)
        {
            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(correo));
            email.From = new MailAddress("mepcostaricaprueba@hotmail.com");
            email.Subject = asunto;
            email.Body = body;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

            string output = null;
            SmtpClient smtp = instanciaSmtp();

            try
            {
               
                
                    smtp.Send(email);
                
                email.Dispose();
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }

        }

        public SmtpClient instanciaSmtp()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("mepcostaricaprueba@hotmail.com", "prueba2019");
            
            return smtp;
        }
    }
}
