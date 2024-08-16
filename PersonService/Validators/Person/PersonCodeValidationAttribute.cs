using System.ComponentModel.DataAnnotations;

namespace PersonService.Validators.Person
{
    public class PersonCodeValidationAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var personCode = value as string;

            if (string.IsNullOrWhiteSpace(personCode) || personCode.Length != 11)
            {
                ErrorMessage = "Invalid personcode. Personcode must be 11 characters long.";
                return false;
            }

            return true;
        }
    }
}
