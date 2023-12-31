﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class ProteusStartingArtifact : Artifact
    {
        public override void OnTurnStart(State state, Combat combat)
        {
            Random rnd = new Random();
            PType[] parts = { PType.cannon, PType.cockpit, PType.missiles, PType.wing, PType.empty, PType.wing, PType.wing, PType.cannon, PType.missiles, PType.wing, PType.wing, PType.cannon };
            string[] sprites = { "cannon", "cockpit", "missiles", "armor", "scaffold", "thrusters", "fuel", "cannon2", "missiles2", "thrusters2", "reactor", "cannon2" };
            int index = rnd.Next(3, parts.Length);
            bool hasCannon = false;
            bool hasMissiles = false;
            bool hasCockpit = false;
            foreach (Part part in state.ship.parts)
            {
                if (part.type == PType.cannon) { hasCannon = true; }
                if (part.type == PType.missiles) { hasMissiles = true; }
                if (part.type == PType.cockpit) { hasCockpit = true; }
                if (part.skin == "@mod_part:parchment.armada.proteus.fuel") { combat.Queue(new ADrawCard { count = 1 }); }
                if (part.skin == "@mod_part:parchment.armada.proteus.reactor") { combat.Queue(new AEnergy { changeAmount = 1 }); }
                if (part.skin == "@mod_part:parchment.armada.proteus.thrusters") { combat.Queue(new AMove { dir = 1, targetPlayer = true }); }
                if (part.skin == "@mod_part:parchment.armada.proteus.thrusters2") { combat.Queue(new AMove { dir = -1, targetPlayer = true }); }
                if (part.skin == "@mod_part:parchment.armada.proteus.armor") { combat.Queue(new AStatus { targetPlayer = true, status = Status.tempShield, statusAmount = 1, }); }
            }
            if (!hasCockpit) { index = 1; }
            if (!hasMissiles) { index = 2; }
            if (!hasCannon) { index = 0; }
            combat.Queue(new AAddCard
            {
                card = new Cards.ProteusMutator() { part = parts[index], sprite = sprites[index] },
                destination = CardDestination.Hand,
                amount = 1
            });
        }

        public override List<Tooltip>? GetExtraTooltips()
        {
            return new List<Tooltip>
            {
                new TTCard
                {
                    card = new Cards.ProteusMutator() { part = PType.cannon, sprite="unknown" }
                },
                //new TTGlossary("cardtrait.retain"),
            };
        }
    }
}
