using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalonReservations.Data;
using SalonReservations.Models;
using SalonReservations.Services;

namespace SalonReservations.Pages
{
    public partial class MakeAppointment
    {
        [Inject] private ILogger<MakeAppointment> _logger { get; set; } = default!;
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;
        [Inject] public NavigationManager NavManager { get; set; } = default!;
        [Inject] private SalonServices _shopServices { get; set; } = default!;
        [Inject] private StylistService _stylistService { get; set; } = default!;
        [Inject] private AvailabilityService _availabilityService { get; set; } = default!;
        [Inject] private AppointmentService _appointmentService { get; set; } = default!;

        public Service? ShopService { get; set; }
        public Stylist? Stylist { get; set; }
        public List<TimeSlot> _timeSlots { get; set; } = [];
        public TimeSlot? _selectedTimeSlot { get; set; }
        public string StylistImgSrc { get; set; } = string.Empty;
        private bool _render { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
            int stylistId = 0;
            string? shopServiceName = null;

            if (queryParams.TryGetValue("service", out var serviceVal))
                shopServiceName = Uri.UnescapeDataString(serviceVal[0] ?? "");

            if (queryParams.TryGetValue("stylist", out var stylistVal))
                stylistId = Int32.Parse(stylistVal[0] ?? "0");

            if (string.IsNullOrEmpty(shopServiceName) || stylistId <= 0)
            {
                _render = false;
                return;
            }

            ShopService = await _shopServices.Find(x => x.Name == shopServiceName).FirstOrDefaultAsync();
            Stylist = await _stylistService.GetStylistAsync(stylistId);
            if (Stylist == null) return; //TODO: display message

            StylistImgSrc = $"images/stylist{Stylist.UserId}.jpeg";
            _timeSlots = await _availabilityService.GetAvailability(stylistId, DateTime.Now) ?? [];
        }

        private async Task OnDateChanged(DateTime date)
        {
            if (Stylist == null) return;

            //NOTE: quick way to generate slots for all stylists
            await _availabilityService.GenerateStylistAvailability(date.AddHours(8), date.AddHours(16));
            _timeSlots = await _availabilityService.GetAvailability(Stylist.UserId, date) ?? [];
        }

        public void SelectTimeSlot(int slotId)
        {
            _selectedTimeSlot = _timeSlots.Find(x => x.TimeSlotId == slotId);
        }

        public bool IsSelectedSlot(TimeSlot slot)
        {
            return _selectedTimeSlot == slot;
        }

        private async Task ScheduleAppointment()
        {
            if (Stylist == null || _selectedTimeSlot == null || ShopService == null) return;

            try
            {
                await _appointmentService.MakeAppointment(1, Stylist.UserId, ShopService.ServiceId, _selectedTimeSlot.Start);
                _navigationManager.NavigateTo("/appointment/confirmation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                //TODO: display message
            }
        }
    }
}