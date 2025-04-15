using ApplicationLayer.Interfaces.Identity;
using ApplicationLayer.Interfaces.Infrastructure;
using MediatR;
using SistemaPOS.Views;

namespace SistemaPOS
{
    public partial class MainPage : ContentPage
    {
        private readonly IMediator _mediator;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IServiceProvider _serviceProvider;
        public MainPage(IAuthenticatedUserService authenticatedUserService, IServiceProvider serviceProvider, IMediator mediator)
        {
            _mediator = mediator;
            _authenticatedUserService = authenticatedUserService;
            _serviceProvider = serviceProvider;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!await _authenticatedUserService.IsAuthenticatedAsync())
            {
                var identityService = _serviceProvider.GetService<IIdentityService>();
                Application.Current!.MainPage = new NavigationPage(
                    new LoginPage(identityService!, _authenticatedUserService, _mediator)
                );
            }
        }


        private async void GoToCustomerList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomerListPage(_mediator));
        }

        private async void GoToProductList(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductListPage(_mediator));
        }

        private async void GoToCart(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage(_mediator));
        }

        private async void GoToInvoice(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InvoicePage(_mediator));
        }
    }

}
