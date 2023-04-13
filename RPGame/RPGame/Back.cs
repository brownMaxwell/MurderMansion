using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Back : Command
    {
        public Back() : base()
        {
            this.Name = "back";
        }

        override
        public bool Execute(Player player)
        {
           // if (this.HasSecondWord())
            //{
                player.BackTrack();
           // }
           // else
           // {
               // player.OutputMessage("\nCan't go back any further.");
           // }
            return false;
        }
    }
}
