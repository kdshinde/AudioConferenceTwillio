using System.ComponentModel.DataAnnotations;

namespace ClickToCallAPI.Models
{
    public class SMSRequestModel
    {
        [Required(ErrorMessage = "The user number is required"), Phone]
        public string UserNumber { get; set; }
    }
}
