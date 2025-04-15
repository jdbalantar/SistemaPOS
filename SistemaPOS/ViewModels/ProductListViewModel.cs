using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ApplicationLayer.Features.Cart.Commands;
using ApplicationLayer.Features.Products.Commands;
using ApplicationLayer.Features.Products.Queries;
using Domain.Entities;
using MediatR;

namespace SistemaPOS.ViewModels;

public partial class ProductListViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Product> Products { get; set; }
    public ObservableCollection<Product> Cart { get; set; }
    private readonly IMediator mediator;

    public ICommand AddToCartCommand { get; }

    public ProductListViewModel(IMediator mediator)
    {
        this.mediator = mediator;
        Products = [];
        Cart = [];
        AddToCartCommand = new Command<Product>(async (product) => await AddToCart(product));
    }

    public async Task InitializeAsync()
    {
        var saveProductsResult = await mediator.Send(new SaveProductsCommand());
        if (!saveProductsResult.IsSuccess)
            await Application.Current!.MainPage!.DisplayAlert("Error", saveProductsResult.Message, "OK");

        var getProductsResult = await mediator.Send(new GetProductsQuery());
        if (getProductsResult.IsSuccess)
        {
            var products = getProductsResult.Data;
            if (products is not null)
            {
                Products = new ObservableCollection<Product>(products.Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Title!,
                    Description = p.Description!,
                    Price = p.Price,
                    Image = p.Image!
                }));
                OnPropertyChanged(nameof(Products));
            }
        }
        else
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", getProductsResult.Message, "OK");
        }
    }

    private async Task AddToCart(Product product)
    {
        if (product != null)
        {
            var addProductToCartResult = await mediator.Send(new AddProductToCartCommand(product, 1));
            if (addProductToCartResult.IsSuccess)
            {
                Cart.Add(product);
                Application.Current?.MainPage?.DisplayAlert("Carrito", $"{product.Name} agregado al carrito.", "OK");
            }
            else
                await Application.Current!.MainPage!.DisplayAlert("Error", addProductToCartResult.Message, "OK");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
