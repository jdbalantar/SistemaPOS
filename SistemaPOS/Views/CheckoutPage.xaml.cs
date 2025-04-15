using Domain.Entities;
using MediatR;
using SistemaPOS.ViewModels;

namespace SistemaPOS.Views;

public partial class CheckoutPage : ContentPage
{
    private readonly CheckoutPageViewModel viewModel;

    public CheckoutPage(IMediator mediator)
	{
		InitializeComponent();
        viewModel = new CheckoutPageViewModel(mediator);
        BindingContext = viewModel;
    }

    private void OnPaymentMethodSelected(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton rb && e.Value && BindingContext is CheckoutPageViewModel vm)
        {
            vm.PaymentMethod = rb?.Value?.ToString()!;
        }
    }

}