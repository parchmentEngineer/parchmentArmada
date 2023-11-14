using CobaltCoreModding.Definitions.ModManifests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    internal class TrinityStarterCard : Card
    {
        internal static Spr card_sprite = Spr.cards_Shield;
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var list = new List<CardAction>();
            switch (this.upgrade)
            {
                case Upgrade.None:
                    list.Add(new CardActions.ATrinityActivateNext() { amount = 1, disabled = flipped });
                    list.Add(new ADummyAction());
                    list.Add(new AStatus() { targetPlayer = true, status = Status.shield, statusAmount = 1, disabled = !flipped });
                    break;

                case Upgrade.A:
                    list.Add(new CardActions.ATrinityActivateNext() { amount = 1, disabled = flipped });
                    list.Add(new ADummyAction());
                    list.Add(new AStatus() { targetPlayer = true, status = Status.tempShield, statusAmount = 2, disabled = !flipped });
                    break;

                case Upgrade.B:
                    list.Add(new CardActions.ATrinityActivateNext() { amount = 2 });
                    list.Add(new AStatus() { targetPlayer = true, status = Status.shield, statusAmount = 1 });
                    break;
            }

            return list;
        }

        public override CardData GetData(State state) => new CardData
        {
            cost = upgrade == Upgrade.B ? 2 : 1,
            floppable = upgrade == Upgrade.B ? false : true,
            retain = upgrade == Upgrade.A ? true : false,
            art = upgrade == Upgrade.B ? Spr.cards_BigShield : (flipped ? Spr.cards_Adaptability_Bottom : Spr.cards_Adaptability_Top)
        };

        public override string Name() => "Assault Bracing";
    }
}
