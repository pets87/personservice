using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos.Person
{
    public class PersonChangesRequestDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<string>? ObservableParameters { get; set; }
        
    }
}
