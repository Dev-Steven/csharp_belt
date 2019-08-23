using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;   // Add this for [NotMapped]

namespace csharp_belt.Models
{
    public class Rsvp
    {
        [Key]
        [Column("id")]
        public int RsvpId {get;set;}

        [Column("user_id")]
        public int UserId {get;set;}

        [Column("activity_id")]
        public int ActivityId {get;set;}

        // Navigation Properties
        public User Rsvped {get;set;}
        public Activity Rsvps {get;set;}
    }
}