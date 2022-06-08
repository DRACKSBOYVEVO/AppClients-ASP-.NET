namespace ProyectClients.Models
{
    //Representación visual de la tabla CLIENTE
    public class Cliente
    {
        //Columnas
        public int IdCliente { get; set; }
        public string Nombres { get; set; }
        public string Nit { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string Correo { get; set; }
    }
}