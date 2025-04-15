using SistemaPOS.ViewModels;
using MediatR;

namespace SistemaPOS.Views;

public partial class CustomerListPage : ContentPage
{
    private readonly CustomerListViewModel viewModel;
    public CustomerListPage(IMediator mediator)
    {
        InitializeComponent();
        viewModel = new CustomerListViewModel(mediator);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }
}