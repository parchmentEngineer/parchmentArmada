﻿using parchmentArmada.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.CardActions
{
    internal class AProteusDisplayPart : CardAction
    {
        public string skin;
        public override Icon? GetIcon(State s)
        {
            if (skin != null) { return new Icon((Spr)Proteus.sprites["proteus_type_" + skin].Id, null, Colors.textMain); }
            else { return new Icon((Spr)Proteus.sprites["proteus_type_" + "unknown"].Id, null, Colors.textMain); }
            
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>();
            TTGlossary glossary;
            if (skin != null) { glossary = new TTGlossary(Proteus.glossary["typex" + skin].Head); }
            else { glossary = new TTGlossary(Proteus.glossary["typex" + "unknown"].Head); }
            tooltips.Add(glossary);

            return tooltips;
        }
    }
}
