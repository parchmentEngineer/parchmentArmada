using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { })]
    internal class ProteusMutator : Card
        {
        public int posInHand = 0;
        public PType part;
        public string sprite;
        private void getPosInHand(Combat c)
        {
            int tempPos = 0;
            bool flag = false;
            foreach (Card card in c.hand)
            {
                tempPos += 1;
                if (card.GetType() == typeof(Cards.ProteusMutator))
                {
                    flag = true;
                    break;
                }
            }
            if (flag) { posInHand = tempPos; }
            if (posInHand < 1) { posInHand = 0; }
            if (posInHand > 7) { posInHand = 0; }
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            getPosInHand(c);
            var list = new List<CardAction>();
            if (part != null && sprite != null) {
                list.Add(new CardActions.AProteusDisplayPart { skin = sprite });
                list.Add(new CardActions.AProteusMutate { pos = posInHand, skin = sprite, type = part });
            }
            //list.Add(new AAttack { damage = posInHand });
            return list;
        }

        public override void OnDiscard(State s, Combat c)
        {
            //c.Queue(new CardActions.AProteusMutate { pos = posInHand, skin = sprite, type = part });
            //c.SendCardToExhaust(s, this);
            //c.exhausted.Add(this);
        }
        
        public override CardData GetData(State state) => new CardData
        {
            cost = 0,
            art = new Spr?(Spr.cards_colorless),
            temporary = true,
            unplayable = true,
            exhaust = true
        };
        public override string Name() => "Reconfigure";
    }
}
