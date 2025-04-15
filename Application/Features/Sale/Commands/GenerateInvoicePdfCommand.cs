using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ApplicationLayer.Features.Sale.Commands
{
    public class GenerateInvoicePdfCommand(int saleId, string outputDirectory) : IRequest<string>
    {
        public int SaleId { get; set; } = saleId;
        public string OutputDirectory { get; set; } = outputDirectory;

    }

    public class GenerateInvoicePdfCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<GenerateInvoicePdfCommand, string>
    {
        public async Task<string> Handle(GenerateInvoicePdfCommand request, CancellationToken cancellationToken)
        {
            var sale = await unitOfWork.SaleRepository.GetSaleWithDetailsAsync(request.SaleId);
            if (sale is null)
                throw new InvalidOperationException("Venta no encontrada.");

            var document = new PdfDocument();
            var page = document.AddPage();
            page.Orientation = PdfSharpCore.PageOrientation.Landscape;

            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 10, XFontStyle.Regular);
            var boldFont = new XFont("Verdana", 10, XFontStyle.Bold);
            var titleFont = new XFont("Verdana", 14, XFontStyle.Bold);

            double margin = 40;
            double yPoint = margin;

            gfx.DrawString("LLB Solutions", titleFont, XBrushes.DarkBlue, new XRect(0, yPoint, page.Width, 20), XStringFormats.TopCenter);
            yPoint += 30;
            gfx.DrawString("Factura de Compra", boldFont, XBrushes.Black, new XRect(0, yPoint, page.Width, 20), XStringFormats.TopCenter);
            yPoint += 40;

            gfx.DrawString($"Fecha: {sale.Date:dd/MM/yyyy HH:mm}", font, XBrushes.Black, new XPoint(margin, yPoint));
            gfx.DrawString($"Método de Pago: {sale.PaymentMethod}", font, XBrushes.Black, new XPoint(page.Width / 2, yPoint));
            yPoint += 20;

            if (sale.Client != null)
            {
                gfx.DrawString($"Cliente: {sale.Client.Name}", font, XBrushes.Black, new XPoint(margin, yPoint));
                gfx.DrawString($"Email: {sale.Client.Email}", font, XBrushes.Black, new XPoint(page.Width / 2, yPoint));
                yPoint += 20;
                gfx.DrawString($"Puntos actuales: {sale.Client.LoyaltyPoints}", font, XBrushes.Black, new XPoint(margin, yPoint));
                yPoint += 30;
            }

            gfx.DrawString("Productos:", boldFont, XBrushes.Black, new XPoint(margin, yPoint));
            yPoint += 20;

            double[] colWidths = { 300, 80, 100, 100 };
            double x = margin;

            gfx.DrawString("Producto", boldFont, XBrushes.Black, new XPoint(x, yPoint));
            gfx.DrawString("Cant.", boldFont, XBrushes.Black, new XPoint(x + colWidths[0], yPoint));
            gfx.DrawString("Precio", boldFont, XBrushes.Black, new XPoint(x + colWidths[0] + colWidths[1], yPoint));
            gfx.DrawString("Total", boldFont, XBrushes.Black, new XPoint(x + colWidths[0] + colWidths[1] + colWidths[2], yPoint));
            yPoint += 18;

            foreach (var detail in sale.SaleDetails!)
            {
                var total = detail.Quantity * detail.UnitPrice;
                gfx.DrawString(detail.Product?.Name ?? "-", font, XBrushes.Black, new XPoint(x, yPoint));
                gfx.DrawString(detail.Quantity.ToString(), font, XBrushes.Black, new XPoint(x + colWidths[0], yPoint));
                gfx.DrawString($"${detail.UnitPrice:F2}", font, XBrushes.Black, new XPoint(x + colWidths[0] + colWidths[1], yPoint));
                gfx.DrawString($"${total:F2}", font, XBrushes.Black, new XPoint(x + colWidths[0] + colWidths[1] + colWidths[2], yPoint));
                yPoint += 18;
            }

            yPoint += 30;
            gfx.DrawString("Resumen:", boldFont, XBrushes.Black, new XPoint(margin, yPoint)); yPoint += 20;
            gfx.DrawString($"Total: ${sale.Total:F2}", font, XBrushes.Black, new XPoint(margin, yPoint)); yPoint += 18;
            gfx.DrawString($"Puntos usados: {sale.PointsUsed}", font, XBrushes.Black, new XPoint(margin, yPoint)); yPoint += 18;
            gfx.DrawString($"Puntos ganados: {sale.PointsEarned}", font, XBrushes.Black, new XPoint(margin, yPoint));

            var fileName = $"Factura_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            var filePath = Path.Combine(request.OutputDirectory, fileName);

            using var stream = File.Create(filePath);
            document.Save(stream);
            stream.Close();

            return filePath;
        }
    }



}
