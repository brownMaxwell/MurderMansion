using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Unlock : Command
    {
        public Unlock() : base()
        {
            this.Name = "unlock";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Unlock(this.SecondWord);
            }
            else
            {
                player.OutputMessage("\nOpen what?");
            }
            return false;
        }
    }
}
