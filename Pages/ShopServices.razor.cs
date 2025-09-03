using Microsoft.AspNetCore.Components;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.Pages
{
    public partial class ShopServices : ComponentBase
    {
        [Inject] private NavigationManager _navManager { get; set; } = default!;
        [Inject] private StylistService _stylistService { get; set; } = default!;

        [Parameter] public Stylist? Stylist { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var uri = _navManager.ToAbsoluteUri(_navManager.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("stylist", out var stylistVal))
            {
                int stylistId = 0;
                if (Int32.TryParse(stylistVal, out stylistId))
                {
                    Stylist = await _stylistService.GetStylistAsync(stylistId);
                }
            }
        }
    }
}