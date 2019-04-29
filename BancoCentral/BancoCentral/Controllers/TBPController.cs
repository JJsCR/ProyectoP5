using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace BancoCentral.Controllers
{
    public class TBPController : Controller
    {
        // GET: TBP
        public ActionResult IndexTBP()
        {
            SqlConnection ConexionSQL = new SqlConnection("Data Source=LAPTOP-B1A7M9FH\\SQLEXPRESS;Initial Catalog=Prueba;Integrated Security=True");
            //ES la conexion a la base de prueba que yo estaba haciendo me trae los datos y los grafica.
            //Hay que perfecionar la conexion y la consulta para que traiga la fecha sin horas.
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Select convert(date ,DES_FECHA), NUM_VALOR  from Indicador where COD_INDICADORINTERNO = 317";
            //La hice asi porque pense que que a la hora de pegarse al web service se iba a hacer contando la fecha actual 
            //y restandole los 5 anos.
            cmd.CommandType = CommandType.Text;
            cmd.Connection = ConexionSQL;
            ConexionSQL.Open();

            DataTable Datos = new DataTable();
            Datos.Load(cmd.ExecuteReader());
            ConexionSQL.Close();

            string salida;

            salida = "[['Task','Hours'],";

            foreach (DataRow dr in Datos.Rows)
            {
                salida = salida + "[";
                salida = salida + "'" + dr[0] + "'" + "," + dr[1];
                salida = salida + "],";
            }
            salida = salida + "]";
            @ViewBag.aqui = salida;
            return View();
        }

    }
}