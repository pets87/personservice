using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos.Person
{
    public class PersonChangedValue
    {
        public PersonChangedValue(string field, string oldValue, string newValue)
        {
            Field = field;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string Field { get; set; }
        public string? OldValue{ get; set; }
        public string? NewValue{ get; set; }

    }
}
