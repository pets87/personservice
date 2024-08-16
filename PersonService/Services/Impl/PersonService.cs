using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using PersonService.Dtos.Person;
using PersonService.Exceptions;
using PersonService.Models;
using PersonService.Utilities;

namespace PersonService.Services.Impl
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public PersonService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<PersonDto> GetPerson(string personCode)
        {
            var result = await context.Persons.FirstOrDefaultAsync(x => x.PersonalCode == personCode);
            if (result == null)
            {
                throw new EntityNotFoundException($"Entity Not found with Personcode: {personCode}");
            }
            return mapper.Map<PersonDto>(result);
        }

        public async Task<PersonDto> CreatePerson(PersonDto personDto)
        {
            var person = mapper.Map<Person>(personDto);
            var result = await context.Persons.AddAsync(person);
            await context.SaveChangesAsync();
            return mapper.Map<PersonDto>(result.Entity);
        }

        public async Task<PersonDto> UpdatePerson(PersonDto personDto)
        {
            var person = await context.Persons.FindAsync(personDto.Id);
            if (person == null)
            {
                throw new EntityNotFoundException($"Entity Not found with Id: {personDto.Id}");
            }
            var updatedPerson = mapper.Map<Person>(personDto);
            updatedPerson.CreatedAt = person.CreatedAt; //PersonDto does not have BaseEntity fields, so assign separately
            updatedPerson.UpdatedAt = person.UpdatedAt;
            updatedPerson.UpdatedBy = person.UpdatedBy;
            context.Persons.Entry(person).CurrentValues.SetValues(updatedPerson);
            await context.SaveChangesAsync();
            return mapper.Map<PersonDto>(person);
        }

        public async Task<bool> DeletePerson(PersonDto personDto)
        {
            var person = await context.Persons.FindAsync(personDto.Id);
            if (person == null)
            {
                throw new EntityNotFoundException($"Entity Not found with Id: {personDto.Id}");
            }
            context.Persons.Entry(person).State = EntityState.Deleted;
            return await context.SaveChangesAsync() == 1; // one row affected            
        }

        public async Task<PersonChangesResponseDto> GetPersonChanges(PersonChangesRequestDto personChangesRequest)
        {
            var personChanges = await context.PersonChangeLogs.Where(x => x.ChangeTime > personChangesRequest.StartTime && x.ChangeTime <= personChangesRequest.EndTime).ToListAsync();
            var changedPersonIds = personChanges.Select(x => x.PersonId);
            var changedPersons = await context.Persons.Where(x => changedPersonIds.Contains(x.Id)).ToListAsync();

            var response = new PersonChangesResponseDto() { Changes = new List<PersonChangeListDto>() };
            foreach (var person in changedPersons.OrderByDescending(x=>x.CreatedAt))
            {
                var change = new PersonChangeListDto();
                change.PersonChanges = new List<PersonChangeDto>();
                change.PersonCode = person.PersonalCode;
                var currentPersonChangeLogs = personChanges.Where(x => x.PersonId == person.Id).ToList();

                foreach (var currentPersonChangeLog in currentPersonChangeLogs)
                {
                    //not using automapper, because need to build changes string based on requested input
                    var personChangeDto = new PersonChangeDto();
                    personChangeDto.ChangeType = currentPersonChangeLog.ChangeType;
                    personChangeDto.ChangedBy = currentPersonChangeLog.ChangedBy;
                    personChangeDto.ChangedTime = currentPersonChangeLog.ChangeTime;
                    personChangeDto.ChangedValues = PersonUtils.GetChangedValues(currentPersonChangeLog.OldValue, currentPersonChangeLog.NewValue, personChangesRequest.ObservableParameters);
                    if(personChangeDto.ChangedValues.Any())
                        change.PersonChanges.Add(personChangeDto);
                }
                if(change.PersonChanges.Any())
                    response.Changes.Add(change);
            }
            return response;
        }
    }
}
