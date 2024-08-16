using PersonService.Dtos.Person;

namespace PersonService.Services
{
    public interface IPersonService
    {
        Task<PersonDto> GetPerson(string personCode);
        Task<PersonDto> CreatePerson(PersonDto personDto);
        Task<PersonDto> UpdatePerson(PersonDto personDto);
        Task<bool> DeletePerson(PersonDto personDto);
        Task<PersonChangesResponseDto> GetPersonChanges(PersonChangesRequestDto personChangesRequest);
    }
}
