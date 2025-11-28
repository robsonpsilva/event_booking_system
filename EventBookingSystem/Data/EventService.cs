
using Microsoft.EntityFrameworkCore;
using EventBookingSystem.Models;

namespace EventBookingSystem.Data
{
    /// <summary>
    /// The EventService handles all data access logic for Events and Ticket Types.
    /// </summary>
    public class EventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new event and saves it to the database.
        /// </summary>
        public async Task<Event> CreateEventAsync(Event eventData)
        {
            _context.Events.Add(eventData);
            await _context.SaveChangesAsync();
            return eventData;
        }

        /// <summary>
        /// Retrieves all events with their ticket types.
        /// </summary>
        public async Task<List<Event>> GetEventsAsync()
        {
            return await _context.Events
                                 .Include(e => e.TicketTypes)
                                 .OrderBy(e => e.StartDate)
                                 .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific event by ID, including ticket types.
        /// </summary>
        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events
                                 .Include(e => e.TicketTypes)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Counts confirmed tickets sold for a specific ticket type.
        /// </summary>
        public async Task<int> GetTicketsSoldByTicketTypeIdAsync(int ticketTypeId)
        {
            return await _context.Registrations
                                 .Where(r => r.TicketTypeId == ticketTypeId && r.Status == "Confirmed")
                                 .SumAsync(r => r.Quantity);
        }

        /// <summary>
        /// Creates a single registration after validating availability.
        /// </summary>
        public async Task<Registration> CreateRegistrationAsync(Registration registration)
        {
            var ticketType = await _context.TicketTypes
                                           .Include(t => t.Event)
                                           .FirstOrDefaultAsync(t => t.Id == registration.TicketTypeId);

            if (ticketType == null)
                throw new InvalidOperationException("Ticket type not found.");

            var ticketsSold = await GetTicketsSoldByTicketTypeIdAsync(registration.TicketTypeId);

            if (ticketsSold >= ticketType.Quota)
                throw new InvalidOperationException($"The ticket '{ticketType.Name}' is sold out.");

            registration.EventId = ticketType.EventId;

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return registration;
        }

        /// <summary>
        /// Adds multiple registrations in a single transaction with quota validation.
        /// </summary>
        public async Task AddRegistrationsAsync(List<Registration> registrations)
        {
            if (registrations == null || registrations.Count == 0)
                throw new ArgumentException("No registrations provided.");

            foreach (var reg in registrations)
            {
                var ticketType = await _context.TicketTypes
                                               .Include(t => t.Event)
                                               .FirstOrDefaultAsync(t => t.Id == reg.TicketTypeId);

                if (ticketType == null)
                    throw new InvalidOperationException($"Ticket type with ID {reg.TicketTypeId} not found.");

                var ticketsSold = await _context.Registrations
                                                .Where(r => r.TicketTypeId == reg.TicketTypeId && r.Status == "Confirmed")
                                                .SumAsync(r => r.Quantity);

                if (ticketsSold + reg.Quantity > ticketType.Quota)
                    throw new InvalidOperationException($"Quota exceeded for ticket type '{ticketType.Name}'.");
            }

            _context.Registrations.AddRange(registrations);
            await _context.SaveChangesAsync();
        }
    }
}
