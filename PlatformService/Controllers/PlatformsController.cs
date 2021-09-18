using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using AutoMapper;
using System.Collections.Generic;
using PlatformService.Dtos;
using System;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using System.Threading.Tasks;
using PlatformService.AsyncDataService;

namespace PlatformSevice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController: ControllerBase{

        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;
        public PlatformsController(IPlatformRepo repository, 
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms(){

            Console.WriteLine("--> Getting Platforms"); 
            
            var platformItems = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}",Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id){

            Console.WriteLine("--> Getting Platform by Id"); 
            
            var platformItem = _repository.GetPlatformById(id);
            if(platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto){

            Console.WriteLine("--> Create Platform"); 
            
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();
            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            //Send Sync message
            try{
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex){
                Console.WriteLine($"--> Could not send synchromously: {ex.Message}");
            }

            //Send Async Message
            try
            {
                 var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
                 platformPublishedDto.Event = "Platform_Published";
                 _messageBusClient.PublishNewPlatfom(platformPublishedDto);
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"--> Could not send asynchromously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id},  platformReadDto);
        }


    }
    
}