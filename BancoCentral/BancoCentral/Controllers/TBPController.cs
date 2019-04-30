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
            SqlConnection ConexionSQL = new SqlConnection("Data Source=.;Initial Catalog=BancoCentral;Integrated Security=True");
            //ES la conexion a la base de prueba que yo estaba haciendo me trae los datos y los grafica.
            //Hay que perfecionar la conexion y la consulta para que traiga la fecha sin horas.
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT convert(date ,fecha),tasaBasicaPasiva FROM Indicador WHERE fecha BETWEEN '2014-01-01' AND '2019-04-29'";
            //La hice asi porque pense que que a la hora de pegarse al web service se iba a hacer contando la fecha actual 
            //y restandole los 5 anos.
            cmd.CommandType = CommandType.Text;
            cmd.Connection = ConexionSQL;
            ConexionSQL.Open();

            DataTable Datos = new DataTable();
            Datos.Load(cmd.ExecuteReader());
            ConexionSQL.Close();

            string salida;

            salida = "[['Task','Tasa Basica Pasiva'],";

            foreach (DataRow dr in Datos.Rows)
            {
                string str = Convert.ToString(dr[1]);
                salida = salida + "[";
                salida = salida + "'" + Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy") + "'" + "," +str.Replace(",",".");
                salida = salida + "],";
            }
            salida = salida + "]";
            @ViewBag.aqui = salida;
            return View();
        }


    }
}