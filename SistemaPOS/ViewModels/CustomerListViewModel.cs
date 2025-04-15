using ApplicationLayer.Features.Customers.Commands;
using ApplicationLayer.Features.Customers.Queries;
using Domain.Entities;
using MediatR;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SistemaPOS.ViewModels
{
    public partial class CustomerListViewModel(IMediator mediator) : INotifyPropertyChanged
    {
        private readonly IMediator mediator = mediator;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<Client> Clients { get; set; } = [];

        public async Task InitializeAsync()
        {
            var saveCustomersResult = await mediator.Send(new SaveCustomersCommand());
            if (!saveCustomersResult.IsSuccess)
                await Application.Current!.MainPage!.DisplayAlert("Error", saveCustomersResult.Message, "OK");

            var getCustomersResult = await mediator.Send(new GetCustomersQuery());
            if (getCustomersResult.IsSuccess)
            {
                var customers = getCustomersResult.Data;
                if (customers is not null)
                {
                    Clients = new ObservableCollection<Client>(customers.Select(p => new Client
                    {
                        Id = p.Id,
                        Name = p.Name!,
                        Email = p.Email,
                        LoyaltyPoints = p.LoyaltyPoints,
                    }));
                    OnPropertyChanged(nameof(Clients));

                }
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", getCustomersResult.Message, "OK");
            }
        }

    }
}
