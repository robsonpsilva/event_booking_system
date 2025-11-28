using System.ComponentModel.DataAnnotations;

namespace EventBookingSystem.Models
{
    // Represents an event in the system.
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The event name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(7);
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7).AddHours(4);

        [StringLength(100)]
        public string? Location { get; set; }
        
        //Maximum capacity of participants (sum of all ticket types)
        public int MaxCapacity { get; set; }

        // Navigation property for related Ticket Types.
        public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();
    }
}