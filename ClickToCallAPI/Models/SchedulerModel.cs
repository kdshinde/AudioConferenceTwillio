using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClickToCallAPI.Models
{
    public class SchedulerModel
    {
        [Required(ErrorMessage = "The Appointment time is required")]
        public DateTime Appointment { get; set; }

        [Required(ErrorMessage = "The user number is required"), Phone]
        public string UserNumber { get; set; }

        [Required(ErrorMessage = "The sales number is required"), Phone]
        public string SalesNumber { get; set; }
    }
}