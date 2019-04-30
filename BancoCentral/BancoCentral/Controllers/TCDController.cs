using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace BancoCentral.Controllers
{
    public class TCDController : Controller
    {
        // GET: TCD
        public ActionResult IndexTCD()
        {
            SqlConnection ConexionSQL = new SqlConnection("Data Source=.;Initial Catalog=BancoCentral;Integrated Security=True");
            //ES la conexion a la base de prueba que yo estaba haciendo me trae los datos y los grafica.
            //Hay que perfecionar la conexion y la consulta para que traiga la fecha sin horas.
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT convert(date ,fecha),tipoCambioCompra,tipoCambioVenta FROM Indicador WHERE fecha BETWEEN '2014-01-01' and GETDATE()";
            //La hice asi porque pense que que a la hora de pegarse al web service se iba a hacer contando la fecha actual 
            //y restandole los 5 anos.
            cmd.CommandType = CommandType.Text;
            cmd.Connection = ConexionSQL;
            ConexionSQL.Open();

            DataTable Datos = new DataTable();
            Datos.Load(cmd.ExecuteReader());
            ConexionSQL.Close();

            string salida;

            salida = "[['Task','Tipo Cambio de Compra','Tipo Cambio de Venta'],";

            foreach (DataRow dr in Datos.Rows)
            {
                string str = Convert.ToString(dr[1]);
                string str2 = Convert.ToString(dr[2]);
                salida = salida + "[";
                salida = salida + "'" + Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy") + "'" + "," + str.Replace(",", ".") + ","+ str2.Replace(",", ".");
                salida = salida + "],";
            }
            salida = salida + "]";
            @ViewBag.aqui = salida;
            return View();
        }
    }
}