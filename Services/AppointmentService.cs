using System.Threading.Tasks;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalonReservations.DAL;
using SalonReservations.DAL.Interfaces;
using SalonReservations.Data;
using SalonReservations.Models;

namespace SalonReservations.Services
{
    public class AppointmentService
    {
        private AvailabilityService _availabilityService { get; set; }
        private UnitOfWork _unitOfWork { get; set; }
        private IRepository<AppointmentEntity> _appointmentsRepo { get; set; }
        private readonly SalonServices _shopServices = default!;
        private readonly StylistService _stylistService = default!;
        public Appointment? NewAppointment { get; private set; }

        public AppointmentService(UnitOfWork unitOfWork, AvailabilityService availabilityService, SalonServices shopServices,
        StylistService stylistService)
        {
            _unitOfWork = unitOfWork;
            _availabilityService = availabilityService;
            _shopServices = shopServices;
            _stylistService = stylistService;
            _appointmentsRepo = unitOfWork.Repository<AppointmentEntity>();
        }

        public async Task MakeAppointment(int clientId, int stylistId, int serviceId, DateTime appointmentStart)
        {
            ServiceEntity? service = await _shopServices.GetById(serviceId);
            if (service == null) throw new Exception($"Salon service [{serviceId}] doesn't exist");

            //var available_times = _availabilityService.GetAvailability(stylistId);
            bool isAvailable = await _availabilityService.IsTimeSlotAvailable(stylistId, appointmentStart, service.Duration);
            if (!isAvailable) new Exception("Appointment time not available"); //NOTE: could use custom exception type

            await SaveAppointment(clientId, stylistId, service, appointmentStart);
        }

        private async Task SaveAppointment(int clientId, int stylistId, ServiceEntity service, DateTime appointmentStart)
        {
            var nAppointment = new AppointmentEntity
            {
                ClientId = clientId,
                StylistId = stylistId,
                ServiceId = service.ServiceId,
                StartTime = appointmentStart,
                EndTime = appointmentStart.AddMinutes(service.Duration)
            };

            //save appointment
            await _appointmentsRepo.AddAsync(nAppointment);

            //update stylist availability
            await _availabilityService.UpdateAvailability(stylistId, appointmentStart, service.Duration);
            await _unitOfWork.SaveAsync();

            NewAppointment = nAppointment.Adapt<Appointment>();
            NewAppointment.Service = service.Adapt<Service>();
            NewAppointment.Stylist = await _stylistService.GetStylistAsync(stylistId);
        }

        public async Task CancelAppointment(int appointmentId)
        {
            var appointment = await _appointmentsRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return; //TODO: return error

            //TODO: add timeslots back to stylist availability

            _appointmentsRepo.Remove(appointment);
            await _unitOfWork.SaveAsync();
        }

        public async Task ReshceduleAppointment(int appointmentId, int stylistId, DateTime datetime)
        {
            var appointment = await _appointmentsRepo.GetByIdAsync(appointmentId);
            if (appointment == null) return; //TODO: return error

            if (appointment.StylistId != stylistId)
            {
                //check new stylist availability
                var appService = await _shopServices.GetById(appointment.ServiceId);
                if (appService == null) return; //TODO: return error

                bool isAvailable = await _availabilityService.IsTimeSlotAvailable(stylistId, appointment.StartTime, appService.Duration);
                if (isAvailable)
                    await SaveAppointment(appointment.ClientId, stylistId, appService, appointment.StartTime);
            }
            else
            {

                //TODO: add timeslots back to stylist availability
                _appointmentsRepo.Remove(appointment);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<List<Appointment>> GetAll()
        {
            var apps = await _appointmentsRepo.Query().Include(x => x.Service).ToListAsync();
            return apps.OrderBy(x => x.StartTime).Adapt<List<Appointment>>();
        }
    }
}