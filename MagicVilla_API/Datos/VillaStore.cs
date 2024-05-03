using MagicVilla_API.Modelo.Dto;

namespace MagicVilla_API.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> VillaList = new List<VillaDto> 
        {
            new VillaDto{ 
                Id=1, 
                Codigo = 001,
                Descripcion="Computadora de Escritorio",
                Cantidad=10,
                PrecioUnitario=800, 
                Total = 8000,
            },
            new VillaDto{ 
                Id=2, 
                Codigo = 002,
                Descripcion="Telefono",
                Cantidad=20,
                PrecioUnitario=500,
                Total = 10000,
            },
            new VillaDto{ 
                Id=3, 
                Codigo = 003,
                Descripcion="Laptop",
                Cantidad=10,
                PrecioUnitario=600,
                Total = 6000,
            },
        };
    }
}
