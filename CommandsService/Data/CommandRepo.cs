using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.Data;
using System.Linq;
using System;

namespace CommandsService.Data
{

    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;
        public CommandRepo(AppDbContext context)
        {
            _context = context;

        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >=0 ? true : false);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
                throw new ArgumentNullException(nameof(plat));
            _context.Platforms.Add(plat);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(x=>x.Id == platformId);
        }

        public bool ExternalPlatformExists(int externalId){
            return _context.Platforms.Any(x=>x.ExternalID == externalId);
        }


        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(x => x.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }
        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .Where(x=>x.PlatformId == platformId && x.Id == commandId)
                .FirstOrDefault();
        }
        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }
    }

}