namespace MagicVilla_API.Modelo.Dto
{
    public class VillaReporteTituloDto
    {
        public string NombreEmpresa { get; set; }
        public string NombreReporte { get; set; }
        public List<VillasReporteDto> VillasReporte { get; set; }
        public VillaReporteTituloDto()
        {
            VillasReporte = new List<VillasReporteDto>();
        }
    }
}
