using AspNetCoreWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        private static List<Command> commands = new List<Command>()
        {
            new Command() { Id=1, HowTo="List directories", Line="dir/w", Platform="Windows"},
            new Command() { Id=2, HowTo="Remove directories", Line="rm", Platform="Windows" },
            new Command() { Id=3, HowTo="Copy directories", Line="Copy", Platform="Windows"},
            new Command() { Id=4, HowTo="List files", Line="ls", Platform="Windows"},
            new Command() { Id=5, HowTo="Copy file", Line="xcopy", Platform="Windows"},
            new Command() { Id=6, HowTo="Ping website", Line="ping", Platform="Windows"},
        };

        public void CreateCommand(Command cmd)
        {
            int lastId = commands.Max(x => x.Id);
            cmd.Id = lastId + 1;
            commands.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            commands.RemoveAll(x => x.Id == cmd.Id);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return commands;
        }

        public Command GetCommandById(int id)
        {
            return commands.FirstOrDefault(x => x.Id == id);
        }

        public bool SaveChanges()
        {
            return true;
        }

        public void UpdateCommand(Command cmd)
        {
            int index = commands.FindIndex(x => x.Id == cmd.Id);
            if (index != -1)
                commands[index] = cmd;
        }
    }
}
