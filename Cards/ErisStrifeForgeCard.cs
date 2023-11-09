using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { })]
    internal class ErisStrifeForgeCard : Card
    {
        internal static Spr card_sprite = Spr.cards_goat;
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new ASpawn { thing = new Drones.ErisStrifeEngine { } });
                    //list.Add(new CardActions.ATrinityActivateNext() { amount = 1 });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = 0,
            art = new Spr?(card_sprite),
            temporary = true,
            recycle = true,
            retain = true,
        };
        public override string Name() => "Strife Forge";

    }
}
