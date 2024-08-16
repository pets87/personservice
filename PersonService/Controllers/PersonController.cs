using Microsoft.AspNetCore.Mvc;
using PersonService.Dtos;
using PersonService.Dtos.Person;
using PersonService.Exceptions;
using PersonService.Models;
using PersonService.Services;
using PersonService.Validators;
using PersonService.Validators.Person;

namespace PersonService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService personService;
        private readonly ILogger<PersonController> logger;

        public PersonController(IPersonService personService, ILogger<PersonController> logger)
        {
            this.personService = personService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PersonDto>> GetPerson([PersonCodeValidation][FromQuery] string personCode, [XroadHeaderValidation][FromHeader] XroadHeaders xroadHeaders)
        {
            try
            {
                return Ok(await personService.GetPerson(personCode));
            }
            catch (EntityNotFoundException e) 
            {
                logger.LogError(e, "GetPerson.Error");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.LogError(e, "GetPerson.Error");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PersonDto>> CreatePerson([PersonCreateValidation][FromBody] PersonDto person, [XroadHeaderValidation][FromHeader] XroadHeaders xroadHeaders)
        {
            try
            {
                return Created($"api/{nameof(Person)}", await personService.CreatePerson(person));
            }
            catch (Exception e)
            {
                logger.LogError(e, "CreatePerson.Error");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult<PersonDto>> UpdatePerson([PersonUpdateValidation][FromBody]PersonDto person, [XroadHeaderValidation][FromHeader] XroadHeaders xroadHeaders)
        {
            try
            {
                return Ok(await personService.UpdatePerson(person));
            }
            catch (EntityNotFoundException e)
            {
                logger.LogError(e, "UpdatePerson.Error");
                return NotFound();
            }
            catch (Exception e)
            {
                logger.LogError(e, "UpdatePerson.Error");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeletePerson([PersonDeleteValidation][FromBody] PersonDto person, [XroadHeaderValidation][FromHeader] XroadHeaders xroadHeaders)
        {
            try
            {
                return Ok(await personService.DeletePerson(person));
            }
            catch (Exception e)
            {
                logger.LogError(e, "DeletePerson.Error");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("/changes")]
        public async Task<ActionResult<PersonChangesResponseDto>> GetPersonChanges([FromBody] PersonChangesRequestDto requestDto) 
        {
            try
            {
                return Ok(await personService.GetPersonChanges(requestDto));
            }
            catch (Exception e)
            {
                logger.LogError(e, "DeletePerson.Error");
                return StatusCode(500, "Internal server error");
            }
        }



    }
}
