using Domain.Entities;
using MediatR;

namespace SistemaPOS.Views;

public partial class CartPage : ContentPage
{
    private readonly CartPageViewModel viewModel;
    public CartPage(IMediator mediator)
    {
        InitializeComponent();
        viewModel = new CartPageViewModel(mediator);
        BindingContext = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }

    private void OnClientSelected(object sender, SelectionChangedEventArgs e)
    {
        if (BindingContext is CartPageViewModel vm && e.CurrentSelection.FirstOrDefault() is Client client)
        {
            vm.SelectedClient = client;
        }
    }

}