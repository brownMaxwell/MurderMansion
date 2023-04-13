using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Unload : Command
    {
        public Unload() : base()
        {
            this.Name = "unload";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.OpenContainer(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nUnload what container?");
            }
            return false;
        }
    }
}
