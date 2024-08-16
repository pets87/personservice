using PersonService.Dtos.Person;
using System.ComponentModel.DataAnnotations;

namespace PersonService.Validators.Person
{
    public class PersonDeleteValidationAttribute : ValidationAttribute
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
                ErrorMessage = "Cannot delete entity without Id";
                return false;
            }          
            return true;
        }
    }
}
