using System.ComponentModel.DataAnnotations;
using EventBookingSystem.Models;

namespace EventBookingSystem.Models
{
    // Represents the registration (ticket sale/reservation) of a participant for an event.
    public class Registration
    {
        public int Id { get; set; }

        // Foreign key to the Ticket Type.
        public int TicketTypeId { get; set; }
        // Navigation property to the Ticket Type.
        public TicketType TicketType { get; set; } = default!;

        // Foreign key to the Event. (Added to maintain crucial relationship)
        public int EventId { get; set; }
        // Navigation property to the Event.
        public Event Event { get; set; } = default!;

        // Attendee Details
        [Required(ErrorMessage = "The full name is required.")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100)] // Added StringLength for consistency
        public string Email { get; set; } = string.Empty;

        // Booking Data
        [Range(1, int.MaxValue, ErrorMessage = "The quantity must be at least 1.")]
        public int Quantity { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Status: "Pending", "Confirmed", "Canceled" (Added for completeness, matching previous structure)
        public string Status { get; set; } = "Confirmed";
    }
}