using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRSMediatr.Entities.DTOs.Requests
{
    public class UpdateDriverRequest
    {
        [Required]
        public Guid DriverId { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public int DriverNumber { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}
