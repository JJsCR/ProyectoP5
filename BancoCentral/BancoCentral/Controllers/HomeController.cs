using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace BancoCentral.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ClientesController clientes = new ClientesController();
            clientes.CorreosClientes();

            SqlConnection ConexionSQL = new SqlConnection("Data Source=.;Initial Catalog=BancoCentral;Integrated Security=True");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT convert(date ,fecha),tipoCambioCompra,tipoCambioVenta FROM Indicador WHERE fecha BETWEEN '2018-01-01' and GETDATE()";

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
                salida = salida + "'" + Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy") + "'" + "," + str.Replace(",", ".") + "," + str2.Replace(",", ".");
                salida = salida + "],";
            }
            salida = salida + "]";
            @ViewBag.aqui = salida;
            //-------------------------------------------------------------------------------------------------------------------------------------------

            SqlCommand cmd1 = new SqlCommand();
            cmd1.CommandText = "SELECT convert(date ,fecha),tasaPoliticaMonetaria FROM Indicador WHERE fecha BETWEEN '2018-01-01' and GETDATE()";

            cmd1.CommandType = CommandType.Text;
            cmd1.Connection = ConexionSQL;
            ConexionSQL.Open();

            DataTable Datos1 = new DataTable();
            Datos1.Load(cmd1.ExecuteReader());
            ConexionSQL.Close();

            string salida1;

            salida1 = "[['Task','Tasa Politica Monetaria'],";

            foreach (DataRow dr in Datos1.Rows)
            {
                string str = Convert.ToString(dr[1]);
                salida1 = salida1 + "[";
                salida1 = salida1 + "'" + Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy") + "'" + "," + str.Replace(",", ".");
                salida1 = salida1 + "],";
            }
            salida1 = salida1 + "]";
            @ViewBag.aqui1 = salida1;

            //-------------------------------------------------------------------------------------------------------------------------------------------

            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "SELECT convert(date ,fecha),tasaBasicaPasiva FROM Indicador WHERE fecha BETWEEN '2014-01-01' AND '2019-04-29'";

            cmd2.CommandType = CommandType.Text;
            cmd2.Connection = ConexionSQL;
            ConexionSQL.Open();

            DataTable Datos2 = new DataTable();
            Datos2.Load(cmd2.ExecuteReader());
            ConexionSQL.Close();

            string salida2;

            salida2 = "[['Task','Tasa Basica Pasiva'],";

            foreach (DataRow dr in Datos2.Rows)
            {
                string str = Convert.ToString(dr[1]);
                salida2 = salida2 + "[";
                salida2 = salida2 + "'" + Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy") + "'" + "," + str.Replace(",", ".");
                salida2 = salida2 + "],";
            }
            salida2 = salida2 + "]";
            @ViewBag.aqui2 = salida2;

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