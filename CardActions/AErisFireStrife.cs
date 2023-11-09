using FMOD;
using parchmentArmada.Drones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.CardActions
{
    internal class AErisFireStrife : CardAction
    {
        public bool start;
        public bool targetPlayer;
        public ErisStrifeEngine drone;

        public override void Begin(G g, State s, Combat c)
        {
            drone.targetPlayer = false;
            //drone.isHitting = start ? true : false;
            if(start) { 
                if (targetPlayer) { drone.fakeOffset = -16; }
                if (!targetPlayer) { drone.fakeOffset = 16; }
            }
            if (!start)
            {
                if (targetPlayer && drone.eCharge > 0) { drone.eCharge -= 1; }
                if (!targetPlayer && drone.charge > 0) { drone.charge -= 1; }
            }
            timer = 0;
        }
    }
}
