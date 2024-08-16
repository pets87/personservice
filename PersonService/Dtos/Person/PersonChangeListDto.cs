using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos.Person
{
    public class PersonChangeListDto
    {
        public string PersonCode { get; set; }
        public List<PersonChangeDto> PersonChanges { get; set; }
    }
}
