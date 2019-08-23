using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace csharp_belt.Models
{
    public class Activity
    {
        [Key]
        [Column("id")]
        public int ActivityId {get;set;}

        [Column("user_id")]
        public int UserId {get;set;}

        [Required]
        [Column("title")]
        public string Title {get;set;}

        [Column("date")]
        [Required]
        [DataType(DataType.Date)]
        [ValidateDate]
        public string Date {get;set;}

        [Column("time")]
        [DataType(DataType.Time)]
        public string Time {get;set;}

        [Column("duration_time")]
        public int? DurationTime {get;set;}

        [Column("duration_unit")]
        public string DurationUnit {get;set;}

        [Column("description")]
        public string Description {get;set;}

        // Navigatin Properties
        public User Creator {get;set;}
        public List<Rsvp> AllParticipants {get;set;}
    }

    public class ValidateDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime wedDate = Convert.ToDateTime(value);
            DateTime currDate = DateTime.Now;
            if(wedDate < currDate)
            {
                return new ValidationResult("Not a Valid Date");
            }
            return ValidationResult.Success;
        }
    }
}