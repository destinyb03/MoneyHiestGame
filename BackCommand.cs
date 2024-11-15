using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarterGame
{
    public class BackCommand : Command
    {
        public BackCommand() : base()
        {
            this.Name = "back";

        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.Back();
                return true;
            }
           
            return false; 

        }
    }
}