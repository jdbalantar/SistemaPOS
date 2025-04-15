using SistemaPOS.Views;

namespace SistemaPOS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("CartPage", typeof(CartPage));
            Routing.RegisterRoute("CheckoutPage", typeof(CheckoutPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));

        }
    }
}
