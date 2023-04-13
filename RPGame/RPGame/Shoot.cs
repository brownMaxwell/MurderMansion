using System;
using System.Collections.Generic;
using System.Text;

namespace RPGame
{
    class Shoot : Command
    {
        public Shoot() : base()
        {
            this.Name = "shoot";
        }

        override
        public bool Execute(Player player)
        {
            /*
            if (player.CurrentRoom.NPC == null)
            {
                player.OutputMessage("\nNo one is here.");
                
            }
            else
            {
                player.Shoot();
            }
            */
            bool theEnd = false;
            if (this.HasSecondWord())
            {
                theEnd = player.Shoot(SecondWord);
            }
            else
            {
                player.WarningMessage("Shoot who?");
            }
            return theEnd;
        }
    }
}
