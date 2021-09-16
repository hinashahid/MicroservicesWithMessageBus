using System;
using Microsoft.AspNetCore.Mvc;
using CommandsService.Data;
using AutoMapper;
using System.Collections.Generic;
using CommandsService.Dtos;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepo _repository;
        public PlatformsController(ICommandRepo repository, 
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");

            var platformItems = _repository.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }
        [HttpPost]
        public ActionResult TestInbound()
        {
            Console.WriteLine("--> Inbound call");
            return Ok(" --> Inbound Test success");
        }
    }

}