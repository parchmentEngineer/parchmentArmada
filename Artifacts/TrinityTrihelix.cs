using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parchmentArmada.Artifacts
{
    [ArtifactMeta(owner = Deck.colorless, pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    internal class TrinityTrihelix : Artifact
    {
        public override void OnReceiveArtifact(State state)
        {
            state.ship.baseDraw++;
        }
    }
}
