using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]
using System.Text.RegularExpressions;

namespace csharp_belt.Models
    {
        public class User
        {
            [Key]
            [Column("id")]            
            public int UserId {get;set;}

            [Column("first_name")]
            [Required]
            [MinLength(2)]
            public string FirstName {get;set;}

            [Column("last_name")]
            [Required]
            [MinLength(2)]
            public string LastName {get;set;}

            [Column("email")]
            [Required]
            [EmailAddress]
            public string Email {get;set;}

            [Column("password")]
            [Required]
            [MinLength(8)]
            [ValidatePassword]
            [DataType(DataType.Password)]
            public string Password {get;set;}

            [Column("created_at")]
            public DateTime CreatedAt {get;set;}

            [Column("updated_at")]
            public DateTime UpdatedAt {get;set;}

            [NotMapped]
            [Required]
            [Compare("Password")]
            [DataType(DataType.Password)]
            public string Confirm {get;set;}

            // Navigation Properties
            public List<Rsvp> AllActivities {get;set;}
        }

        public class ValidatePasswordAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[0-9])(?=.*\W).*$");
                
                // Match match = regex.Match((string)value);

                var hasLetter = new Regex(@"[a-z]+");
                var hasNumber = new Regex(@"[0-9]+");
                var hasSpecialChar = new Regex(@"[!@#$%^&*(),.?:{}|<>]");

                var isGood = hasLetter.IsMatch((string)value) && hasNumber.IsMatch((string)value) && hasSpecialChar.IsMatch((string)value);

                if(isGood == false)
                {
                    return new ValidationResult("Password must contain 1 letter, 1 number, and 1 special character");
                }
                
                return ValidationResult.Success;
            }
        }
    }