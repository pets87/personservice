using PersonService.Models;

namespace PersonService.Data
{
    public static class DataSeed
    {
        public static void SeedPersons(ApplicationDbContext context)
        {
            if (!context.Persons.Any())
            {
                var persons = new List<Person>
                {
                    new Person
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Edgar",
                        LastName = "Savisaar",
                        PersonalCode = "38605043778",
                        DateOfBirth = new DateTime(1986, 05, 04, 12, 23, 21, DateTimeKind.Utc)
                    },
                    new Person
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Roberto",
                        LastName = "Dinamite",
                        PersonalCode = "46806164715",
                        DateOfBirth = new DateTime(1968, 06, 16, 10, 12, 52, DateTimeKind.Utc)
                    },
                    new Person
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Marek",
                        LastName = "Plura",
                        PersonalCode = "37412180181",
                        DateOfBirth = new DateTime(1974, 12, 18, 18, 52, 13, DateTimeKind.Utc)
                    }
                };
                context.AddRange(persons);
                context.SaveChanges();
            }
        }
    }
}
