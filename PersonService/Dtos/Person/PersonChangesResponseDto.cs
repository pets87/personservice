using System.Diagnostics.CodeAnalysis;

namespace PersonService.Dtos.Person
{
    public class PersonChangesResponseDto
    {
        public List<PersonChangeListDto> Changes { get; set; }
    }
}
