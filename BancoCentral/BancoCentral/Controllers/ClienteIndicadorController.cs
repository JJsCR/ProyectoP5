using BancoCentral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BancoCentral.Controllers
{
    public class ClienteIndicadorController : Controller
    {
        private BancoCentralEntities db = new BancoCentralEntities();

        public void RegistraEnvioCorreo(int idIndicador, int idCliente)                                     //Método para llevar un registro de los correos enviados a los clientes
        {
            string nowDate = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                db.Database.ExecuteSqlCommand("INSERT INTO ClienteIndicador (indicadorId,clienteId,fecha) VALUES (" + idIndicador + ", " + idCliente + ", '" + nowDate + "')");
            }catch(Exception e)
            {

            }
        }
    }
}