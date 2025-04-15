using ApplicationLayer.Interfaces.Infrastructure;
using ApplicationLayer.Interfaces.Infrastructure.Repositories;
using MediatR;
using SistemaPOS.ViewModels;

namespace SistemaPOS.Views;

public partial class ProductListPage : ContentPage
{
    private readonly ProductListViewModel viewModel;

    public ProductListPage(IMediator mediator)
	{
		InitializeComponent();
        viewModel = new ProductListViewModel(mediator);
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.InitializeAsync();
    }

}