using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ApplicationLayer.DTOs.Cart;
using ApplicationLayer.Features.Cart.Commands;
using ApplicationLayer.Features.Customers.Commands;
using ApplicationLayer.Features.Sale.Commands;
using ApplicationLayer.Interfaces.Infrastructure;
using Domain.Entities;
using MediatR;

namespace SistemaPOS.ViewModels;

public partial class CheckoutPageViewModel : INotifyPropertyChanged
{
    private readonly IMediator mediator;
    public ObservableCollection<CartItem> CartItems { get; set; } = [];
    public Client? SelectedClient { get; set; }
    public string PaymentMethod { get; set; } = "Efectivo";
    public int PointsToRedeem { get; set; }

    public decimal SubtotalTotal => CartItems.Sum(i => i.Product.Price * i.Quantity);
    public decimal FinalTotal => Math.Max(0, SubtotalTotal - (PointsToRedeem * 0.5m));
    public int PointsEarned => (int)Math.Floor(SubtotalTotal / 10);

    public ICommand CheckoutCommand { get; }

    public CheckoutPageViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        CheckoutCommand = new Command(async () => await Checkout());
    }

    private async Task Checkout()
    {
        if (CartItems.Count == 0)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "No hay productos en el carrito", "OK");
            return;
        }

        if (SelectedClient is null)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "No ha seleccionado un cliente", "OK");
            return;
        }

        var sale = new Sale
        {
            ClientId = SelectedClient.Id,
            Date = DateTime.Now,
            PaymentMethod = PaymentMethod,
            PointsUsed = PointsToRedeem,
            PointsEarned = PointsEarned,
            Total = FinalTotal,
            SaleDetails = CartItems.Select(ci => new SaleDetail
            {
                ProductId = ci.Product.Id,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        var result = await mediator.Send(new CreateSaleCommand(sale));
        if (!result.IsSuccess)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", result.Message, "OK");
            return;
        }

        await mediator.Send(new UpdateCustomerLoyaltyPointsCommand(SelectedClient.Id, PointsEarned, PointsToRedeem));

        var outputDirectory = FileSystem.Current.CacheDirectory;
        var generatePdfCommand = new GenerateInvoicePdfCommand(result.Data, outputDirectory);
        var pdfPath = await mediator.Send(generatePdfCommand);

        await Application.Current!.MainPage!.DisplayAlert("Compra finalizada", $"La factura ha sido registrada correctamente. La factura se guardó en:\n{pdfPath}", "OK");

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

        var clearCartCommandResult = await mediator.Send(new ClearCartCommand());
        if (!clearCartCommandResult.IsSuccess)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", clearCartCommandResult.Message, "OK");
            return;
        }

        CartItems.Clear();
        SelectedClient = null;
        PointsToRedeem = 0;
        PaymentMethod = "Efectivo";
        OnPropertyChanged(nameof(SelectedClient));
        OnPropertyChanged(nameof(PointsToRedeem));
        OnPropertyChanged(nameof(PaymentMethod));
        OnPropertyChanged(nameof(SubtotalTotal));
        OnPropertyChanged(nameof(FinalTotal));
        OnPropertyChanged(nameof(PointsEarned));

        Application.Current!.MainPage = new NavigationPage(new MainPage(App.Services.GetRequiredService<IAuthenticatedUserService>(), App.Services, mediator));

    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}