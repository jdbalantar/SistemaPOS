using ApplicationLayer.DTOs.Invoice;
using ApplicationLayer.Features.Cart.Commands;
using ApplicationLayer.Features.Invoices.Queries;
using ApplicationLayer.Features.Sale.Commands;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SistemaPOS.ViewModels;
public partial class InvoicePageViewModel : INotifyPropertyChanged
{
    private readonly IMediator _mediator;

    public ObservableCollection<InvoiceDto> Invoices { get; set; } = [];

    public ICommand GeneratePdfCommand { get; }

    public InvoicePageViewModel(IMediator mediator)
    {
        _mediator = mediator;
        GeneratePdfCommand = new Command<InvoiceDto>(async invoice => await GeneratePdf(invoice));
    }

    public async Task LoadInvoicesAsync()
    {
        var result = await _mediator.Send(new GetInvoicesQuery());
        Invoices = new ObservableCollection<InvoiceDto>(result.Data!);
        OnPropertyChanged(nameof(Invoices));
    }

    private async Task GeneratePdf(InvoiceDto invoice)
    {
        var outputDirectory = FileSystem.Current.CacheDirectory;
        var generatePdfCommand = new GenerateInvoicePdfCommand(invoice.InvoiceId, outputDirectory);
        var pdfPath = await _mediator.Send(generatePdfCommand);

#if WINDOWS
        try
        {
            var processStartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = pdfPath,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", $"No se pudo abrir el PDF. Error: {ex.Message}", "OK");
        }
#endif
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
