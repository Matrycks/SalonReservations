using Microsoft.AspNetCore.Components;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.View.Components
{
    public partial class StylistsComponent : ComponentBase
    {
        private bool IsLoading = false;
        private List<Stylist> Stylists { get; set; } = new();
        public bool IsCompactDesign { get; set; }

        [Inject] StylistService _stylistService { get; set; } = default!;
        [Inject] NavigationManager _navManager { get; set; } = default!;

        [Parameter] public string? ServiceName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            if (!string.IsNullOrEmpty(ServiceName))
                Stylists = await _stylistService.GetStylistsAsync(ServiceName);
            else
                Stylists = await _stylistService.GetStylistsAsync();

            IsLoading = false;
        }

        public void MakeAppointment(Stylist stylist)
        {
            if (!String.IsNullOrEmpty(ServiceName))
                _navManager.NavigateTo($"/appointment/schedule?service={Uri.EscapeDataString(ServiceName)}&stylist={stylist.UserId}");
            else
                _navManager.NavigateTo($"/services?stylist={stylist.UserId}");
        }
    }
}
