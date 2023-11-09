using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.CardActions
{
    internal class AHeliosFireLaser : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            var parts = s.ship.parts;
            foreach (Part part in parts)
            {
                if (part.type == PType.cannon) part.type = PType.special;
                else if (part.type == PType.special) part.type = PType.cannon;
            }
            Audio.Play(new GUID?(FSPRO.Event.Plink));
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
