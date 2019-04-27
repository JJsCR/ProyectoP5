using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BancoCentral.Models;

namespace BancoCentral.Controllers
{
    public class WebServiceController : Controller
    {

        cr.fi.bccr.gee.wsIndicadoresEconomicos indicadores = new cr.fi.bccr.gee.wsIndicadoresEconomicos();
        private EmailController emailController = new EmailController();
        private ClienteIndicadorController clienteIndicador = new ClienteIndicadorController();

        public DataSet ObtenerIndicadoresWebService(string lastDate, string nowDate)
        {

            DataSet tablaIndicadores = new DataSet();
            string cliente = "Proyecto Progra5 Fidélitas JKL Masters";

            try
            {

                DataTable politicaMonetaria = indicadores.ObtenerIndicadoresEconomicos("3541",
                                                                            lastDate,
                                                                            nowDate,
                                                                            cliente, "N").Tables[0].Copy();

                DataTable basicaPasiva = indicadores.ObtenerIndicadoresEconomicos("423",
                                                                            lastDate,
                                                                            nowDate,
                                                                            cliente, "N").Tables[0].Copy();

                DataTable compra = indicadores.ObtenerIndicadoresEconomicos("317",
                                                                            lastDate,
                                                                            nowDate,
                                                                            cliente, "N").Tables[0].Copy();

                DataTable venta = indicadores.ObtenerIndicadoresEconomicos("318",
                                                                            lastDate,
                                                                            nowDate,
                                                                            cliente, "N").Tables[0].Copy();

                politicaMonetaria.TableName = "tablaMonetaria";
                basicaPasiva.TableName = "tablaPasiva";
                compra.TableName = "tablaCompra";
                venta.TableName = "tablaVenta";

                tablaIndicadores.Tables.Add(politicaMonetaria);
                tablaIndicadores.Tables.Add(basicaPasiva);
                tablaIndicadores.Tables.Add(compra);
                tablaIndicadores.Tables.Add(venta);

            }
            catch (Exception e)
            {

            }

            return tablaIndicadores;
        }

       


        public void WebServiceCorreo(List<Indicador> tablaIndicadores, List<Cliente> clientes)
        {
            foreach (Cliente cliente in clientes)
            {
                foreach (Indicador indicador in tablaIndicadores)
                {

                    emailController.sendEmail("Indicadores Económicos",
                              "<b>Señor:</b> " + cliente.nombre + " " + cliente.apellidoPaterno + " <b>cedula:</b> " + cliente.cedula + "<br>" +
                              " <b>Fecha Tipo Cambio:</b> " + Convert.ToString(indicador.fecha) + "<br>" +
                              " <b>Tasa Política Monetaria:</b> " + Convert.ToString(indicador.tasaPoliticaMonetaria) + "<br>" +
                              " <b>Tasa Básica Pasiva:</b> " + Convert.ToString(indicador.tasaBasicaPasiva) + "<br>" +
                              " <b>Tipo Cambio Compra:</b> " + Convert.ToString(indicador.tipoCambioCompra) + "<br>" +
                               " <b>Tipo Cambio Venta:</b> " + Convert.ToString(indicador.tipoCambioVenta) + "<br><br>" +
                               "<img src='https://activos.bccr.fi.cr/sitios/bccr/SiteAssets/bccr.jpg'>",
                              cliente.correo);

                    clienteIndicador.RegistraEnvioCorreo(indicador.idIndicador,cliente.idCliente);

                }
            }

            
        }
    }
}