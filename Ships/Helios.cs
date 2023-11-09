using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using parchmentArmada.Artifacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using FMOD;

namespace parchmentArmada.Ships
{
    internal class Helios : ISpriteManifest, IShipPartManifest, IShipManifest, IStartershipManifest, IStatusManifest, IArtifactManifest
    {
        public DirectoryInfo? ModRootFolder { get; set; }
        public string Name => "parchmentEngineer.parchmentArmada.HeliosManifest";
        public IEnumerable<string> Dependencies => new string[0];
        public DirectoryInfo? GameRootFolder { get; set; }

        public static Dictionary<string, ExternalSprite> sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalPart> parts = new Dictionary<string, ExternalPart>();
        ExternalShip? helios;
        public static ExternalStatus? solarCharge;
        public static ExternalArtifact? HeliosArtifact;

        public static Vec?[] toDraw = new Vec?[20];
        public static int center = -1;
        public static Card centerCard;
        public static int statusCount = 0;


        private void addSprite(string name, IArtRegistry artRegistry)
        {
            if (ModRootFolder == null) throw new Exception("Root Folder not set");
            var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName(name + ".png"));
            sprites.Add(name, new ExternalSprite("parchment.armada.helios." + name, new FileInfo(path)));
            artRegistry.RegisterArt(sprites[name]);
        }

        private void addPart(string name, string sprite, PType type, bool flip, IShipPartRegistry registry)
        {
            parts.Add(name, new ExternalPart(
            "parchment.armada.helios." + name,
            new Part()
            {
                active = true,
                damageModifier = PDamMod.none,
                type = type,
                flip = flip
            },
            sprites[sprite] ?? throw new Exception()));
            registry.RegisterPart(parts[name]);
        }

        public void LoadManifest(IArtRegistry artRegistry)
        {
            addSprite("helios_cannon", artRegistry);
            addSprite("helios_cockpit", artRegistry);
            addSprite("helios_scaffold", artRegistry);
            addSprite("helios_missiles", artRegistry);
            addSprite("helios_chassis", artRegistry);
            addSprite("helios_solar_0", artRegistry);
            addSprite("helios_solar_1", artRegistry);
            addSprite("helios_solar_2", artRegistry);
            addSprite("helios_solar_3", artRegistry);
            addSprite("helios_solar_4", artRegistry);
            addSprite("helios_solar_5", artRegistry);
            addSprite("helios_solar_6", artRegistry);
            addSprite("helios_status", artRegistry);
            addSprite("helios_artifact", artRegistry);
            addSprite("helios_warning1", artRegistry);
            addSprite("helios_warning2", artRegistry);
        }

        public void LoadManifest(IShipPartRegistry registry)
        {
            addPart("cannon","helios_cannon", PType.cannon, false, registry);
            addPart("cockpit","helios_cockpit", PType.cockpit, false, registry);
            addPart("scaffold","helios_scaffold", PType.empty, false, registry);
            addPart("scaffoldf","helios_scaffold", PType.empty, true, registry);
            addPart("missiles","helios_missiles", PType.missiles, false, registry);
            addPart("solar","helios_solar_0", PType.special, false, registry);
            addPart("solarf","helios_solar_0", PType.special, true, registry);
        }

        public void LoadManifest(IShipRegistry shipRegistry)
        {
            helios = new ExternalShip("parchment.armada.Helios",
                new Ship()
                {
                    baseDraw = 5,
                    baseEnergy = 3,
                    heatTrigger = 3,
                    heatMin = 0,
                    hull = 7,
                    hullMax = 7,
                    shieldMaxBase = 4
                },
                new ExternalPart[] { 
                    parts["cockpit"],
                    parts["scaffold"],
                    parts["scaffoldf"],
                    parts["cannon"],
                    parts["solar"],
                    parts["solarf"],
                    parts["missiles"]
                },
                sprites["helios_chassis"] ?? throw new Exception(),
                null
                );
            shipRegistry.RegisterShip(helios);
        }
        public void LoadManifest(IStartershipRegistry registry)
        {
            if (helios == null)
                return;
            var heliosShip = new ExternalStarterShip("parchment.armada.Helios",
                helios.GlobalName,
                new ExternalCard[0],
                new ExternalArtifact[] {HeliosArtifact ?? throw new Exception()},
                new Type[0],
                new Type[0]);

            heliosShip.AddLocalisation("Helios", "An experimental ship built around a titanic solar cannon. Charge it by playing your centermost card.");
            //registry.RegisterStartership(heliosShip);
        }

        public void LoadManifest(IStatusRegistry statusRegistry)
        {
            solarCharge = new ExternalStatus("parchment.armada.solarCharge", true, System.Drawing.Color.Red, null, sprites["helios_status"] ?? throw new Exception("missing sprite"), false);
            statusRegistry.RegisterStatus(solarCharge);
            solarCharge.AddLocalisation("Solar Charge", "Your solar cannon is charging. At 10 charges, it fires a burst of six shots.");
        }

        public void LoadManifest(IArtifactRegistry registry)
        {
            var harmony = new Harmony("parchment.armada.Helios");
            HeliosIconLogic(harmony);

            var spr = sprites["helios_artifact"];
            HeliosArtifact = new ExternalArtifact(typeof(Artifacts.HeliosArtifact), "parchment.armada.HeliosArtifact", spr, null, new ExternalGlossary[0]);
            HeliosArtifact.AddLocalisation("en", "SOLAR CANNON", "When you play the centermost card of your hand, gain a Solar Charge. At 10 charges, your solar cannon fires a burst of six 1-damage shots.");
            registry.RegisterArtifact(HeliosArtifact);
        }

        private void HeliosIconLogic(Harmony harmony)
        {
            var patch_method = typeof(Card).GetMethod("Render") ?? throw new Exception("Couldnt find method");
            var patch_target = typeof(Helios).GetMethod("HeliosCardDrawPatch", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic) ?? throw new Exception("Couldnt find TrinityManifest.TrivengeNormalDamagePatch method");
            harmony.Patch(patch_method, prefix: new HarmonyMethod(patch_target));

        }

        private static bool HeliosCardDrawPatch(Card __instance, G g, Vec? posOverride = null, State? fakeState = null, bool ignoreAnim = false, bool ignoreHover = false, bool hideFace = false, bool hilight = false, bool showRarity = false, bool autoFocus = false, UIKey? keyOverride = null, OnMouseDown? onMouseDown = null, OnMouseDownRight? onMouseDownRight = null, OnInputPhase? onInputPhase = null, double? overrideWidth = null, UIKey? leftHint = null, UIKey? rightHint = null, UIKey? upHint = null, UIKey? downHint = null, int? renderAutopilot = null, bool? forceIsInteractible = null, bool reportTextBoxesForLocTest = false)
        {
            /*foreach (Vec vec in toDraw)
            {
                if(vec != null) {
                    double x = vec.x + vec.x;
                    double y = vec.y + vec.y - 1.0;
                    Draw.Rect(x, y, x + 21, y + 21, new Color(255, 0, 0));
                }
            }
            if(center > -1 && !(centerCard is null)) {
                //Rect rect = centerCard.GetScreenRect();
                double x = rect.x + centerCard.pos.x;
                double y = rect.y + centerCard.pos.y;
                double w = rect.w;
                double h = rect.h;
                //Draw.Rect(x,y,w,h, new Color(255, 0, 0));
            }*/

            Vec vec = posOverride ?? __instance.pos;
            Rect rect = (__instance.GetScreenRect() + vec + new Vec(0.0, __instance.hoverAnim * -2.0 + Mutil.Parabola(__instance.flipAnim) * -10.0 + Mutil.Parabola(Math.Abs(__instance.flopAnim)) * -10.0 * (double)Math.Sign(__instance.flopAnim))).round();
            //double yoff = Math.Min(Math.Max(Math.Abs(__instance.targetPos.x - __instance.pos.x)-10,0)*6, 10);
            double yoff = 0;
            //Draw.Rect(vec.x, vec.y, 300, 3, new Color(255,0,0));
            foreach (Artifact artifact in g.state.artifacts) { 
                if (artifact.Name() == "SOLAR CANNON") {
                    Vec tPos = __instance.targetPos;
                    if (tPos.x > 160 && tPos.x < 240 && tPos.y > 120)
                    {
                        Spr spr = statusCount >= 9 ? (Spr)sprites["helios_warning2"].Id : (Spr)sprites["helios_warning1"].Id;
                        Draw.Sprite(spr, rect.x + 27, rect.y + 18 + yoff);
                    }
                }
            }

            //Draw.Rect(0, 0, 20, 20, new Color(255, 0, 0));
            return true;
        }
    }
}
