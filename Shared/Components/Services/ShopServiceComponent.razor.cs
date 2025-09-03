using Mapster;
using Microsoft.AspNetCore.Components;
using SalonReservations.DAL;
using SalonReservations.Data;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.View.Components
{
    public partial class ShopServiceComponent : ComponentBase
    {
        [Parameter] public Stylist? Stylist { get; set; }
        [Parameter] public bool IsMinimalDesign { get; set; } = false;

        [Inject] private SalonServices _shopServices { get; set; } = default!;
        [Inject] private NavigationManager _navManager { get; set; } = default!;

        public List<Service> Services { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            IEnumerable<ServiceEntity> serviceEntities = [];
            if (Stylist == null)
                serviceEntities = await _shopServices.ListServices();
            else
                serviceEntities = await _shopServices.GetStylistServices(Stylist.UserId);

            Services = serviceEntities.Adapt<List<Service>>();
        }

        public void ServiceSelected(Service service)
        {
            if (Stylist == null)
                _navManager.NavigateTo($"/stylists?service={Uri.EscapeDataString(service.Name)}");
            else
                _navManager.NavigateTo($"/appointment/schedule?service={Uri.EscapeDataString(service.Name)}&stylist={Stylist.UserId}");
        }
    }
}