using ApplicationLayer.DTOs.Cart;
using ApplicationLayer.Features.Cart.Commands;
using ApplicationLayer.Features.Cart.Queries;
using ApplicationLayer.Features.Customers.Queries;
using Domain.Entities;
using MediatR;
using SistemaPOS.ViewModels;
using SistemaPOS.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class CartPageViewModel : INotifyPropertyChanged
{
    private readonly IMediator _mediator;

    public ObservableCollection<CartItem> CartItems { get; set; } = new();
    public ObservableCollection<Client> Clients { get; set; } = new();
    public ObservableCollection<Client> FilteredClients { get; set; } = new();

    private Client? _selectedClient;
    public Client? SelectedClient
    {
        get => _selectedClient;
        set { _selectedClient = value; OnPropertyChanged(); }
    }

    private string _clientSearch = string.Empty;
    public string ClientSearch
    {
        get => _clientSearch;
        set
        {
            _clientSearch = value;
            OnPropertyChanged();
            ApplyClientFilter();
        }
    }

    private decimal _subtotalTotal;
    public decimal SubtotalTotal
    {
        get => _subtotalTotal;
        set { _subtotalTotal = value; OnPropertyChanged(); }
    }

    private int _earnedPoints;
    public int EarnedPoints
    {
        get => _earnedPoints;
        set { _earnedPoints = value; OnPropertyChanged(); }
    }

    private int _pointsToRedeem;
    public int PointsToRedeem
    {
        get => _pointsToRedeem;
        set
        {
            _pointsToRedeem = value;
            OnPropertyChanged();
            UpdateFinalTotal();
        }
    }

    private decimal _finalTotal;
    public decimal FinalTotal
    {
        get => _finalTotal;
        set { _finalTotal = value; OnPropertyChanged(); }
    }

    public ICommand RemoveItemCommand { get; }
    public ICommand RedeemPointsCommand { get; }
    public ICommand CheckoutCommand { get; }

    public CartPageViewModel(IMediator mediator)
    {
        _mediator = mediator;

        RemoveItemCommand = new Command<CartItem>(async item => await RemoveItemAsync(item));
        RedeemPointsCommand = new Command(UpdateFinalTotal);
        CheckoutCommand = new Command(async () => await CompleteShop());
    }

    public async Task InitializeAsync()
    {
        var cartResult = await _mediator.Send(new GetCartQuery());
        if (cartResult.IsSuccess && cartResult.Data is not null)
        {
            CartItems = new ObservableCollection<CartItem>(cartResult.Data);
            OnPropertyChanged(nameof(CartItems));
            RecalculateTotals();
        }

        var clientResult = await _mediator.Send(new GetCustomersQuery());
        if (clientResult.IsSuccess && clientResult.Data is not null)
        {
            Clients = new ObservableCollection<Client>(clientResult.Data is not null ? clientResult.Data!.Select(x => new Client()
            {
                Email = x.Email,
                Id = x.Id,
                Name = x.Name,
                LoyaltyPoints = x.LoyaltyPoints
            }) : []);
            FilteredClients = new ObservableCollection<Client>(Clients);
            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(FilteredClients));
        }
    }

    private void ApplyClientFilter()
    {
        var query = ClientSearch.ToLower();
        FilteredClients = new ObservableCollection<Client>(
            string.IsNullOrWhiteSpace(query)
            ? Clients
            : Clients.Where(c => c.Name.ToLower().Contains(query) || c.Email.ToLower().Contains(query))
        );
        OnPropertyChanged(nameof(FilteredClients));
    }

    private async Task RemoveItemAsync(CartItem item)
    {
        var result = await _mediator.Send(new RemoveProductFromCartCommand(item.Product.Id));
        if (result.IsSuccess)
        {
            await InitializeAsync();
        }
    }

    private void RecalculateTotals()
    {
        SubtotalTotal = CartItems.Sum(i => i.Quantity * i.Product.Price);
        EarnedPoints = (int)Math.Floor(SubtotalTotal / 10);
        UpdateFinalTotal();
    }

    private void UpdateFinalTotal()
    {
        if (SelectedClient is null)
        {
            FinalTotal = SubtotalTotal;
            return;
        }

        var maxPointsBySubtotal = (int)Math.Floor(SubtotalTotal / 0.5m);
        var maxRedeemable = Math.Min(maxPointsBySubtotal, SelectedClient.LoyaltyPoints);

        if (PointsToRedeem > maxRedeemable)
            PointsToRedeem = maxRedeemable;

        if (PointsToRedeem < 0)
            PointsToRedeem = 0;

        var discount = PointsToRedeem * 0.5m;
        FinalTotal = SubtotalTotal - discount;
    }


    private async Task CompleteShop()
    {
        if (CartItems.Count == 0)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "No hay productos en el carrito.", "OK");
            return;
        }

        if (SelectedClient is null)
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Seleccione un cliente.", "OK");
            return;
        }

        var checkoutPage = new CheckoutPage(_mediator)
        {
            BindingContext = new CheckoutPageViewModel(_mediator)
            {
                CartItems = new ObservableCollection<CartItem>(CartItems),
                SelectedClient = SelectedClient,
                PointsToRedeem = PointsToRedeem
            }
        };

        await Application.Current!.MainPage!.Navigation.PushAsync(checkoutPage);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
