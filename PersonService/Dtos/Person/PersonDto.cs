using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos.Person
{
    public class PersonDto
    {
        public Guid? Id { get; set; }
        public string PersonalCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? TimeOfDeath { get; set; }
    }
}
