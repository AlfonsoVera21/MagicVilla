using HandlebarsDotNet;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelo;
using MagicVilla_API.Modelo.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.IO;
using System.Net;
using Xceed.Document.NET;
using Xceed.Words.NET;
using static iTextSharp.text.pdf.AcroFields;


namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetVillas()
        {
            // Template de Handlebars
            string rutaArchivo = "Template/repote-ejemplo.html";
            string source = String.Empty;
            source = System.IO.File.ReadAllText(rutaArchivo);
            
            var template = Handlebars.Compile(source);
            VillaReporteTituloDto villaTituloDto = new VillaReporteTituloDto();
            villaTituloDto.NombreEmpresa = "Empresa 2";
            villaTituloDto.NombreReporte = "Reporte Empresa2";
            
            foreach (var item in VillaStore.VillaList)
            {
                VillasReporteDto villasReporteDto = new VillasReporteDto();
                //primer parametro es del template y el segundo parametro es de la lista con datos
                villasReporteDto.NumeroRegistro = item.Id;
                villasReporteDto.CoRegistro = item.Codigo;
                villasReporteDto.DescripcionArticulo = item.Descripcion;
                villasReporteDto.CantidadArticulo = item.Cantidad;
                villasReporteDto.PrecioUnitario = item.PrecioUnitario;
                villasReporteDto.Total = item.Total;
                villaTituloDto.VillasReporte.Add(villasReporteDto);
            }

            var result = template(villaTituloDto);
            // Conversión de HTML a PDF
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(result);
            byte[] pdfFile;
            using (var memoryStream = new MemoryStream())
            {
                doc.Save(memoryStream);
                pdfFile = memoryStream.ToArray(); // Convierte el stream a un array de bytes
            }
            doc.Close(); // Asegura que el documento se cierre correctamente
                         // Retorna el archivo PDF
            return File(pdfFile, "application/pdf", "GeneratedDocument.pdf");
        }

        [HttpGet("generate-word")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GenerateWord(string filename)
        {
            using (DocX document = DocX.Load(filename))
            {
                // Leer el contenido del documento
                string content = document.Text;

                // Modificar el contenido del documento
                document.ReplaceText("Hola", "Hola Amigos");

                // Guardar los cambios en el documento
                document.Save();
            }
            return Ok();
        }

        [HttpGet("id:int",Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.VillaList.FirstOrDefault(v => v.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto) { 
        //    if(villaDto == null)
        //    {
        //        return BadRequest(villaDto);
        //    }
        //    if (villaDto.Id > 0)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //    villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+1;
        //    //agregar un objeto a la lista
        //    VillaStore.VillaList.Add(villaDto);
        //    //GetVilla es el name que se le da en el meto get(id)
        //    return CreatedAtRoute("GetVilla",new {id = villaDto.Id },villaDto);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CargarDatos([FromBody] VillaDto villaDto, string templatePdf)
        {
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //handlebars
            string source = String.Empty;
            source = System.IO.File.ReadAllText(templatePdf);

            var template = Handlebars.Compile(source);
            VillaReporteTituloDto villaTituloDto = new VillaReporteTituloDto();
            villaTituloDto.NombreEmpresa = "Empresa 2";
            villaTituloDto.NombreReporte = "Reporte Empresa2";

            VillasReporteDto villasReporteDto = new VillasReporteDto();
            //primer parametro es del template y el segundo parametro es de la lista con datos
            //villasReporteDto.NumeroRegistro = villaDto.Id;
            villasReporteDto.NumeroRegistro = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            villasReporteDto.CoRegistro = villaDto.Codigo;
            villasReporteDto.DescripcionArticulo = villaDto.Descripcion;
            villasReporteDto.CantidadArticulo = villaDto.Cantidad;
            villasReporteDto.PrecioUnitario = villaDto.PrecioUnitario;
            villasReporteDto.Total = villaDto.Total;
            villaTituloDto.VillasReporte.Add(villasReporteDto);

            var result = template(villaTituloDto);
            // Conversión de HTML a PDF
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument doc = converter.ConvertHtmlString(result);
            byte[] pdfFile;
            using (var memoryStream = new MemoryStream())
            {
                doc.Save(memoryStream);
                pdfFile = memoryStream.ToArray(); // Convierte el stream a un array de bytes
            }
            doc.Close(); // Asegura que el documento se cierre correctamente
            villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //agregar un objeto a la lista
            VillaStore.VillaList.Add(villaDto);
            //GetVilla es el name que se le da en el meto get(id)
            return File(pdfFile, "application/pdf", "GeneratedDocumentPost.pdf");
        }

    }
}
