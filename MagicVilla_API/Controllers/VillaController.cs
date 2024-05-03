﻿using HandlebarsDotNet;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelo;
using MagicVilla_API.Modelo.Dto;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.IO;


namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        [HttpGet]
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
                villasReporteDto.NumeroRegistro = item.Id;
                villasReporteDto.CoRegistro = item.Codigo;
                villasReporteDto.DescripcionArticulo = item.Descripcion;
                villasReporteDto.CantidadArticulo = item.Cantidad;
                villasReporteDto.PrecioUnitario = item.PrecioUnitario;
                villasReporteDto.Total = item.Total;
                villaTituloDto.VillasReporte.Add(villasReporteDto);
            }

            //var data = new
            //{
            //    nombreEmpresa = "EMPRESA 1",
            //    nombreReporte = "REPORTE EMPRESA1!",
            //    descripcionArticulo = "Laptop"
            //};
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto) { 
            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = VillaStore.VillaList.OrderByDescending(v => v.Id).FirstOrDefault().Id+1;
            //agregar un objeto a la lista
            VillaStore.VillaList.Add(villaDto);
            //GetVilla es el name que se le da en el meto get(id)
            return CreatedAtRoute("GetVilla",new {id = villaDto.Id },villaDto);
        }

    }
}
