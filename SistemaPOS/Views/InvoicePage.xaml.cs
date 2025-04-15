using MediatR;
using SistemaPOS.ViewModels;

namespace SistemaPOS.Views;

public partial class InvoicePage : ContentPage
{
    private readonly InvoicePageViewModel viewModel;

    public InvoicePage(IMediator mediator)
    {
        InitializeComponent();
        viewModel = new InvoicePageViewModel(mediator);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.LoadInvoicesAsync();
    }
}
