using ApplicationLayer.Interfaces.Identity;
using ApplicationLayer.Interfaces.Infrastructure;
using MediatR;
using SistemaPOS.Views;

namespace SistemaPOS
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public static IServiceProvider Services { get; set; } = default!;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            var identityService = serviceProvider.GetService<IIdentityService>();
            var iAuthenticatedUserService = serviceProvider.GetService<IAuthenticatedUserService>();
            var mediator = serviceProvider.GetService<IMediator>();
            MainPage = new NavigationPage(new LoginPage(identityService!, iAuthenticatedUserService!, mediator!));
        }
    }
}
