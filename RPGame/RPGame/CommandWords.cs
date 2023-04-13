using System.Collections;
using System.Collections.Generic;
using System;

namespace RPGame
{
    /*
     * Only changes I made were the new Commands.
     */
    public class CommandWords
    {
        Dictionary<string, Command> commands;
        private static Command[] commandArray = { new GoCommand(), new QuitCommand(), new Open(), new Close(), new Say(),
            new Unlock(), new Back(), new Pickup(), new DisplayInventory(), new Equip(), new Shoot(), new Inspect(),
            new InspectInventory(), new Drop(), new Unload(), new Modify(), new DropAll(), new Experience() };

        public CommandWords() : this(commandArray)
        {
        }

        public CommandWords(Command[] commandList)
        {
            commands = new Dictionary<string, Command>();
            foreach (Command command in commandList)
            {
                commands[command.Name] = command;
            }
            Command help = new HelpCommand(this);
            commands[help.Name] = help;
        }

        public Command Get(string word)
        {
            Command command = null;
            commands.TryGetValue(word, out command);
            return command;
        }

        public string Description()
        {
            string commandNames = "";
            Dictionary<string, Command>.KeyCollection keys = commands.Keys;
            foreach (string commandName in keys)
            {
                commandNames += " " + commandName;
            }
            return commandNames;


        }
    }
}
