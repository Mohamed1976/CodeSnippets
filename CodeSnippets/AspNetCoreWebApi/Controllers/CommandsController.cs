using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApi.Data;
using AspNetCoreWebApi.Dtos;
using AspNetCoreWebApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

//https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1
//https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-3.1

namespace AspNetCoreWebApi.Controllers
{
    //  api/Commands
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public CommandsController(ICommanderRepo commanderRepo, 
            IMapper mapper, IMemoryCache cache)
        {
            _repository = commanderRepo;
            _mapper = mapper;
            _cache = cache;
        }

        //GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommmands()
        {
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET api/commands/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            return NotFound();
        }

        //POST api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();
            //After SaveChanges, we can retrieve the unique id created by the database in commandModel.Id 
            //https://stackoverflow.com/questions/5212751/how-can-i-retrieve-id-of-inserted-entity-using-entity-framework

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdatroute?view=aspnetcore-3.1
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        //DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        //PUT api/commands/{id}
        //Added id to the CommandUpdateDto 
        //[HttpPut("{id}")]
        //public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)

        //PUT api/commands
        [HttpPut]
        public ActionResult UpdateCommand(CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(commandUpdateDto.Id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //Install-Package Microsoft.AspNetCore.JsonPatch
        //Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson
        //https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
        //https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-3.1
        /* Call example 
            [
                {
                    "op": "replace",
                    "path": "HowTo",
                    "value": "#HowTo replaced"
                },
                {
                    "op": "replace",
                    "path": "Line",
                    "value": "#Line replaced"
                }
            ]
        */
        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, [FromBody]JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id); // Get original command object from the database.
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo); //Use Automapper to map to DTO object

            patchDoc.ApplyTo(commandToPatch, ModelState); //Apply the patch to that DTO. 

            if (!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        private readonly MemoryCacheEntryOptions options = new MemoryCacheEntryOptions()
        {
            AbsoluteExpiration = DateTime.Now.AddMinutes(1),
            Priority = CacheItemPriority.Normal
            //SlidingExpiration = TimeSpan.FromMinutes(1)
        };

        //api/commands/GetDate
        [HttpGet]
        [Route("GetDate")]
        public ActionResult GetDate()
        {
            string greeting = _cache.Get<string>("greeting");
            if(string.IsNullOrEmpty(greeting))
            {
                greeting = $"Greeting, the time is: {DateTime.Now.ToString()}";
                _cache.Set<string>("greeting", greeting, options);
            }

            return Ok(greeting);
        }

        //api/Commands/GetCountries/Africa
        [HttpGet]
        [Route("GetCountries/{name}")]
        public IActionResult GetCountries(string name)
        {
            Dictionary<string, long> lookup = new Dictionary<string, long>()
            {
                { "Africa",11189090 },
                { "Asia",233432300 },
                { "Europe",5000780},
                { "America",795292579 },
            };

            if(lookup.ContainsKey(name))
            {
                return Ok(lookup[name]);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
