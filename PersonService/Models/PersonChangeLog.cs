using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonService.Models
{
    [Table("person_changeLog")]    
    [Index(nameof(ChangeTime))]
    public class PersonChangeLog
    {
        public PersonChangeLog() { }
        public PersonChangeLog(Guid personId, string changetype) 
        {
            PersonId = personId;
            ChangeType = changetype;
        }
        [Required]
        [Key]
        public Guid Id { get; set; }
        [Required]        
        [Column("person_id")]
        public Guid PersonId { get; set; }
        [Required]        
        [Column("change_type")]
        public string ChangeType { get; set; } //INSERT, UPDATE, DELETE
        [Column("change_time")]
        public DateTime ChangeTime { get; set; } = DateTime.UtcNow;
        [Column("changed_by")]
        public string? ChangedBy { get; set; } // Personcode from X-Road-UserId. Same as Person.UpdatedBy
        [Column("old_value")]
        public string? OldValue { get; set; }
        [Column("new_value")]
        public string? NewValue { get; set; }

    }
}
