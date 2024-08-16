using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos.Person
{
    public class PersonChangeDto
    {
        public string ChangeType { get; set; }
        public List<PersonChangedValue> ChangedValues { get; set; }
        public DateTime ChangedTime { get; set; }
        public string? ChangedBy { get; set; }
    }
}
