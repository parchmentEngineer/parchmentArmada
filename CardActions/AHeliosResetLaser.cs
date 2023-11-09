using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.CardActions
{
    internal class AHeliosResetLaser : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            var parts = s.ship.parts;
            foreach (Part part in parts)
            {
                if ((part.type == PType.cannon) || (part.type == PType.special))
                {
                    if (part.skin == "@mod_part:parchment.armada.helios.cannon") part.type = PType.cannon;
                    else part.type = PType.special;
                }
            }
        }

        public override Icon? GetIcon(State s) => new Icon(Spr.icons_ace, null, Colors.status);

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>();

            //tooltips.Add(new TTGlossary(glossary_item, Array.Empty<object>()));

            return tooltips;
        }
    }
}
