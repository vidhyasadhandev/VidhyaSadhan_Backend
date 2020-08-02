using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VidyaSadhan_API.Entities
{
    public class Account: IdentityUser
	{

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(6)]
        public string Sex { get; set; }
        //public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public string VerificationToken { get; set; }

        public ICollection<Instructor> Instructors { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Address> Addresses { get; set; }

        [JsonIgnore]
        public List<RefreshTokenSet> RefreshTokens { get; set; }

    }
}
