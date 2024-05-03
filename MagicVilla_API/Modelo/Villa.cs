namespace MagicVilla_API.Modelo
{
    public class Villa
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public double PrecioUnitario { get; set; }
        public double Total { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
