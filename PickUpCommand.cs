using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace StarterGame
{
    public class PickUpCommand : Command
    {
        public PickUpCommand() : base()
        {
            this.Name = "pickup";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                    player.Pickup(this.SecondWord);
                   
            }
            else
            {
                player.NormalMessage("\nPick up what?");
            }
            return false;
        }
    }
}
