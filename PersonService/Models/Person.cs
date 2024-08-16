using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonService.Models
{
    [Table("person")]
    [Index(nameof(PersonalCode), IsUnique = true)]
    public class Person : BaseEntity
    {
        public Person() { }
        public Person(string personCode, string firstName, string lastName)
        {
            PersonalCode = personCode;
            FirstName = firstName;
            LastName = lastName;
        }

        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]

        [Column("personal_code")]
        public string PersonalCode { get; set; }
        [Required]
        [Column("first_name")]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }
        [Column("time_of_death")]
        public DateTime? TimeOfDeath { get; set; }

    }
}
