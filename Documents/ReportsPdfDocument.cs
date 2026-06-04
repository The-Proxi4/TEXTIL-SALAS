using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using textil_salas.ViewModels;

namespace textil_salas.Documents;

public class ReportsPdfDocument : IDocument
{
    private readonly ReportViewModel _vm;

    public ReportsPdfDocument(ReportViewModel vm)
    {
        _vm = vm;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(20);
            page.Content().Column(col =>
            {
                col.Item().Text("Reportes administrativos").FontSize(20).Bold();
                col.Item().Text($"Tipo: {_vm.ReportType}").FontSize(12);
                col.Item().Text($"Periodo: {_vm.StartDate?.ToString("yyyy-MM-dd") ?? "-"} \u2013 {_vm.EndDate?.ToString("yyyy-MM-dd") ?? "-"}").FontSize(10);
                col.Item().LineHorizontal(1).Spacing(5);

                if (_vm.ReportType == "products")
                {
                    col.Item().Element(ComposeProducts);
                }
                else
                {
                    col.Item().Element(ComposeOrders);
                }
            });
        });
    }

    void ComposeProducts(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text("Id").Bold();
                row.RelativeItem().Text("Nombre").Bold();
                row.RelativeItem().Text("Categoría").Bold();
                row.RelativeItem().Text("Precio").Bold();
            });

            foreach (var p in _vm.Products ?? new())
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Text(p.Id);
                    row.RelativeItem().Text(p.Name);
                    row.RelativeItem().Text(p.Category);
                    row.RelativeItem().Text(p.Price.ToString("F2"));
                });
            }
        });
    }

    void ComposeOrders(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text("Orden").Bold();
                row.RelativeItem().Text("Fecha").Bold();
                row.RelativeItem().Text("Usuario").Bold();
                row.RelativeItem().Text("Total").Bold();
            });

            foreach (var o in _vm.Orders ?? new())
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Text(o.OrderNumber);
                    row.RelativeItem().Text(o.CreatedAt.ToString("yyyy-MM-dd"));
                    row.RelativeItem().Text(o.UserId);
                    row.RelativeItem().Text(o.Total.ToString("F2"));
                });
            }
        });
    }
}
