using FMOD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using parchmentArmada.CardActions;
using parchmentArmada.Ships;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HarmonyLib.Code;
using System.Runtime.CompilerServices;

namespace parchmentArmada.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class HeliosArtifact : Artifact
    {
        private int counter = 0;

        public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
        {
            CalculateIconPos(combat);
        }

        public override void OnCombatEnd(State state)
        {
            var parts = state.ship.parts;
            foreach (Part part in parts)
            {
                if ((part.type == PType.cannon) || (part.type == PType.special))
                {
                    if (part.skin == "@mod_part:parchment.armada.helios.cannon") part.type = PType.cannon;
                    else part.type = PType.special;
                }
            }
        }

        public override void OnTurnStart(State state, Combat combat)
        {
            CalculateIconPos(combat);
        }

        public override string Name()
        {
            return "SOLAR CANNON";
        }

        public override void OnTurnEnd(State state, Combat combat)
        {
            Helios.center = -1;
        }

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount)
        {
            Helios.center = -1;
            bool playedCenter = false;
            if (handCount % 2 == 1 && handPosition == Math.Floor((decimal)handCount / 2)) playedCenter = true;
            else if (handCount % 2 == 0 && (handPosition == handCount/2 || handPosition == (handCount/2)-1)) playedCenter = true;
            if (playedCenter)
            { 
                var status = (Status)(Helios.solarCharge.Id ?? throw new NullReferenceException());
                int amt = 0;
                int statusAmt = 0;
                if (state.ship.statusEffects.TryGetValue(status, out amt))
                { statusAmt = amt; }
                else { statusAmt = 0; }
                if (statusAmt >= 9)
                {
                    combat.Queue(new AStatus() { status = status, statusAmount = 1, targetPlayer = true });
                    combat.Queue(new AHeliosFireLaser() { });
                    combat.Queue(new AAttack() { damage = 1, fast = true });
                    combat.Queue(new AAttack() { damage = 1, fast = true });
                    combat.Queue(new AAttack() { damage = 1, fast = true });
                    combat.Queue(new AHeliosResetLaser() { });
                    combat.Queue(new AStatus() { status = status, statusAmount = -10, targetPlayer = true });
                    //combat.Queue(new AEndTurn());
                    Helios.statusCount = 0;
                }
                else
                {
                    combat.Queue(new AStatus() { status = status, statusAmount = 1, targetPlayer = true });
                    Helios.statusCount = statusAmt + 1;
                }
            }
            
        }
        private void CalculateIconPos(Combat combat)
        {
            int handSize = 0;
            Card tempC = new Card(); 
            foreach (Card c in combat.hand)
            {
                handSize += 1;
                //tempC = c;
            }
            if (handSize % 2 == 0)
            {
                int counter = 0;
                foreach (Card c in combat.hand)
                {
                    counter += 1;
                    if(counter==(handSize-1)/2) tempC = c;
                }
                Helios.center = -1;
                Helios.centerCard = tempC;
            }
            else
            {
                Helios.center = 1;
            }
        }
    }
}
