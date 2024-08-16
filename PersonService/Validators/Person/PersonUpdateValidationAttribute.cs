using PersonService.Dtos.Person;
using System.ComponentModel.DataAnnotations;

namespace PersonService.Validators.Person
{
    public class PersonUpdateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var personDto = value as PersonDto;
                        
            if (personDto == null)
            {
                ErrorMessage = "Input cannot be null.";
                return false;
            }
            if (!personDto.Id.HasValue || personDto.Id == Guid.Empty) 
            {
                ErrorMessage = "Cannot update entity without Id";
                return false;
            }
            if (string.IsNullOrWhiteSpace(personDto.FirstName))
            {
                ErrorMessage = $"{nameof(PersonDto.FirstName)} cannot be empty";
                return false;
            }
            if (string.IsNullOrWhiteSpace(personDto.LastName))
            {
                ErrorMessage = $"{nameof(PersonDto.LastName)} cannot be empty";
                return false;
            }
            if (string.IsNullOrWhiteSpace(personDto.PersonalCode))
            {
                ErrorMessage = $"{nameof(PersonDto.PersonalCode)} cannot be empty";
                return false;
            }
            if (personDto.DateOfBirth == DateTime.MinValue || personDto.DateOfBirth == DateTime.MaxValue)
            {
                ErrorMessage = $"{nameof(PersonDto.DateOfBirth)} is invalid";
                return false;
            }
            if (personDto.DateOfBirth > DateTime.Now)
            {
                ErrorMessage = $"{nameof(PersonDto.DateOfBirth)} cannot be in the future";
                return false;
            }
            if (personDto.TimeOfDeath != null && (personDto.TimeOfDeath == DateTime.MinValue || personDto.TimeOfDeath == DateTime.MaxValue))
            {
                ErrorMessage = $"{nameof(PersonDto.TimeOfDeath)} is invalid";
                return false;
            }
            if (personDto.TimeOfDeath != null && personDto.TimeOfDeath > DateTime.Now)
            {
                ErrorMessage = $"{nameof(PersonDto.TimeOfDeath)} cannot be in the future";
                return false;
            }
            return true;
        }
    }
}
