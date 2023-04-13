using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Experience : Command
    {
        public Experience() : base()
        {
            this.Name = "experience";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.OutputMessage("\nDon't add another word after the experience command.");
            }
            else
            {
                player.OutputMessage(player.Experience.ToString());
            }
            return false;
        }
    }
}
