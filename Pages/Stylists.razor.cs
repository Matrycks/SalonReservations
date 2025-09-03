

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace SalonReservations.Pages
{
    public partial class Stylists
    {
        public string? ServiceName { get; set; }

        [Inject] public NavigationManager NavManager { get; set; } = default!;

        protected override void OnInitialized()
        {
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("service", out var value))
            {
                ServiceName = value;
            }
        }
    }
}