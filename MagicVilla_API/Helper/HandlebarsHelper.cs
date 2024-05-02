using HandlebarsDotNet;

namespace MagicVilla_API.Helper
{
    public class HandlebarsHelper
    {
        //public async Task<byte[]> RenderAndConvertToPdfAsync<T>(string template, T model)
        //{
        //    // Render the Handlebars template
        //    var html = await RenderAsync(template, model);

        //    // Convert the HTML to PDF
        //    return await ConvertHtmlToPdfAsync(html);
        //}

        private async Task<string> RenderAsync(string template, object model)
        {
            // Compile the Handlebars template
            var compiler = Handlebars.Compile(template);

            // Render the template with the provided model
            var html = compiler(model);

            return html;
        }

        //private async Task<byte[]> ConvertHtmlToPdfAsync(string html)
        //{
        //    // Create a new PDF document
        //    using var ms = new MemoryStream();
        //    var writer = new PdfWriter(ms);
        //    var pdf = new PdfDocument(writer);
        //    var document = new Document(pdf);

        //    // Convert the HTML to a PDF
        //    /*using var htmlStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
        //    var htmlConverter = new HtmlConverter();
        //    htmlConverter.ConvertToPdf(htmlStream, document);*/

        //    // Save the PDF to a byte array
        //    /*await document.CloseAsync();
        //    return ms.ToArray();*/
        //}
    }
}
