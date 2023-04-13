using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Say : Command
    {
        public Say() : base()
        {
            this.Name = "say";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Say(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nSay what?");
            }
            return false;
        }
    }
}
