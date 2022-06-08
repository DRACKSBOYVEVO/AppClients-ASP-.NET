 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using ProyectClients.Models;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace ProyectClients.Controllers
{
    public class ClienteController : Controller
    {
       
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();  //Cadena de conexión a la base de datos
        
        private static List<Cliente> ListObjets = new List<Cliente>(); // Lista de objetos = ListObjets //Quiero almacenar todos los una lista de clientes que se encuentren en la lista haciendo select * from CLIENTE

        //Conexion con la base de datos para tener una lista de todos mis contactos
        /* CLASES
         * SqlConnection me permite realizar la conexion con mi base de datos
         * SqlCommand nos ayuda a ejecutar comandos de SQL
         * ExecuteReader nos va a permitir a devolver al  SQLdataReader los resultados de la busqueda
         */
        public ActionResult Inicio()
        {
            ListObjets = new List<Cliente>(); //Vacia la lista una vez recorrido los elementos

            using (SqlConnection ConectionObjets = new SqlConnection(conexion)) { //Paso como parametro la cadena de conexión que se encuentre en Web.config
                SqlCommand cmd = new SqlCommand("SELECT * FROM CLIENTE", ConectionObjets)
                {
                    CommandType = CommandType.Text 
                }; 
                ConectionObjets.Open(); 

                //Este comando me permite leer todos los resultados que nos arroja el comando SELECT * FROM CLIENTE
                using (SqlDataReader dr = cmd.ExecuteReader()) {
                    //Mientras estes leyendo, yo quiero que a mi lista de contacto (ListObjets), quiero que almacenes cada elementos que estes encontrando
                    while (dr.Read()) {
                        //Es una variable y que hace referencia a la clase cliente.

                        Cliente NewClient = new Cliente {
                            //A cada elemento quiero acceder a sus propiedades.
                            //dr esta haciendo la lectura de elementos, columna que lea dr["NameColum"]
                            IdCliente = Convert.ToInt32(dr["IdCliente"]), //No es un string, se convierte a entero
                            Nombres = dr["Nombres"].ToString(),
                            Nit = dr["Nit"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Direccion = dr["Direccion"].ToString(),
                            Ciudad = dr["Ciudad"].ToString(),
                            Correo = dr["Correo"].ToString()
                        };
                        //Agrega el nuevo cliente a la lista de objetos
                        ListObjets.Add(NewClient);
                    }
                }
            }
            //Devuelve toda la información que has encontrado en la vista, y se le pasa como parametro la ListObjets
            return View(ListObjets);
        }

        //Para realizar una accion que nos va devolver un resultado, podemos enviarle elementos.

        //El motodo registrar de tipo post esta guardando los valores en nuestra base de datos, y cuando termine
        //de guardar nos va a redirirgir a la pagina de inicio del controlador cliente
        [HttpPost]
        public ActionResult Registrar(Cliente ObjetClient)
        {
            using (SqlConnection ConectionObjets = new SqlConnection(conexion))
            {
                //haciendo uso de los procedimientos almacenados
                SqlCommand cmd = new SqlCommand("sp_Registrar", ConectionObjets);

                cmd.Parameters.AddWithValue("Nombres", ObjetClient.Nombres);
                cmd.Parameters.AddWithValue("Nit", ObjetClient.Nit);
                cmd.Parameters.AddWithValue("Telefono", ObjetClient.Telefono);
                cmd.Parameters.AddWithValue("Direccion", ObjetClient.Direccion);
                cmd.Parameters.AddWithValue("Ciudad", ObjetClient.Ciudad);
                cmd.Parameters.AddWithValue("Correo", ObjetClient.Correo);

                cmd.CommandType = CommandType.StoredProcedure;
                ConectionObjets.Open();
                cmd.ExecuteNonQuery();

            }
            //Una vez registrado me rediccionara nuevamente a la lista de clientes
            return RedirectToAction("Inicio", "Cliente");
        }

        [HttpPost]
        public ActionResult Eliminar(string IdCliente)
        {
            using (SqlConnection ConectionObjets = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", ConectionObjets);
                cmd.Parameters.AddWithValue("IdCliente", IdCliente);
                cmd.CommandType = CommandType.StoredProcedure;
                ConectionObjets.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Cliente");
        }

        [HttpPost]
        public ActionResult Editar(Cliente ObjCliente)
        {
            using (SqlConnection ConectionObjets = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Editar", ConectionObjets);
                cmd.Parameters.AddWithValue("IdCliente", ObjCliente.IdCliente); //Valor "IdCliente" Parametro IdCliente 
                cmd.Parameters.AddWithValue("Nombres", ObjCliente.Nombres);
                cmd.Parameters.AddWithValue("Nit", ObjCliente.Nit);
                cmd.Parameters.AddWithValue("Telefono", ObjCliente.Telefono);
                cmd.Parameters.AddWithValue("Direccion", ObjCliente.Direccion);
                cmd.Parameters.AddWithValue("Ciudad", ObjCliente.Ciudad);
                cmd.Parameters.AddWithValue("Correo", ObjCliente.Correo);

                cmd.CommandType = CommandType.StoredProcedure;
                ConectionObjets.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Cliente"); //Me retorna de nuevo donde estan todos los contactos
        }

        [HttpGet]
        public ActionResult Eliminar(int? idcliente)
        {
            if (idcliente == null)
                return RedirectToAction("Inicio", "Cliente");

            Cliente ConectionObjets = ListObjets.Where(c => c.IdCliente == idcliente).FirstOrDefault();
            return View(ConectionObjets);
        }

        [HttpGet]
        public ActionResult Editar(int? idcliente) //Mostrar el usuario seleccionado, con el ? va aceptar valores nulos
        {
            //Verificando si nuestro valor es nulo
            if (idcliente == null)
                //Si es = a Null retorna la vista de la lista de clientes
                return RedirectToAction("Inicio", "Cliente");

            //Quiero que me muestre los atributos de este cliente que seleccione

            //voy a buscar el cliente      //Voy a seleccionar de toda nuestra lista el usuario especifico que yo estoy seleccionando
            //En la lista, hare un filtro con where para seleccionar un cliente de la lista.
            //c.IdCliente == idcliente tiene que ser igual al parametro que le pase (int? idcliente) 
            //Quiero que me seleciones el primer valor que encuentres ListObjets.Where(c => c.IdCliente == idcliente)
            //Quiero que me encuentres el cliente especifico que yo quiero.
            Cliente ConectionObjets = ListObjets.Where(c => c.IdCliente == idcliente).FirstOrDefault();
            return View(ConectionObjets); //Quiero que trabajes con el contacto que has encontrado
        }

        [HttpGet] //cuando no trae nada, por defecto es asi. GET para realizar alguna accion de resultado nada mas.
        public ActionResult Registrar()
        {
            return View();
        }
    }
}