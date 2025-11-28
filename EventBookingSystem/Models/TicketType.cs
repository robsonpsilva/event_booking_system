using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EventBookingSystem.Models
{
    // Represents a specific type of ticket for an event (e.g., VIP, General Admission, Student)
    public class TicketType
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; } = default!;

        [Required(ErrorMessage = "The ticket type name is required.")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(0.0, double.MaxValue, ErrorMessage = "The price should be a positive value.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "The quota must be at least 1.")]
        public int Quota { get; set; }

        // Propriedades de Navegação
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}