using System.Linq;
using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using UnityEngine;

namespace PropReskinner
{
    public class PropReskinner : ModBehaviour
    {
        public static PropReskinner Instance;
        public INewHorizons NewHorizons;
        public ReplacementAssetManager RepMan;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"Thank you for using {nameof(PropReskinner)}", MessageType.Success);
            // Mod {nameof(PropReskinner)} loaded!
            // I will reskin your children. // Rare

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            new Harmony("LeeSpork.PropReskinner").PatchAll(Assembly.GetExecutingAssembly());

            //OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            //LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;

            // Add extention to New Horizons planet config
            NewHorizons.GetBodyLoadedEvent().AddListener((name) =>
            {
                //ModHelper.Console.WriteLine($"Body {name} loaded!", MessageType.Info);
                var infos = NewHorizons.QueryBody<PropReskinnerInfo[]>(name, "$.extras.PropReskinner");

                if (infos == null) return;

                var planet = NewHorizons.GetPlanet(name);
                ModHelper.Console.WriteLine($"Reskinning stuff on {name}", MessageType.Info);

                foreach (PropReskinnerInfo info in infos)
                {
                    foreach (string path in info.props)
                    {
                        var prop = planet.transform.Find(path).gameObject;
                        ReskinObject(prop, info);
                    }
                }
            });
        }

        //public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        //{
        //    if (newScene != OWScene.SolarSystem) return;
        //    //ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
        //
        //    replacementMaterialManager = new(); // Probably not nessecary to do every time the scene is loaded?
        //}

        public void ReskinObject(GameObject prop, PropReskinnerInfo info)
        {
            RepMan ??= new();
            foreach (var renderer in prop.GetComponentsInChildren<Renderer>())
            {
                renderer.materials = [.. renderer.materials.Select(material => GetReplacementMaterial(ref material, info))];
            }
        }

        private Material GetReplacementMaterial(ref Material material, PropReskinnerInfo info)
        {
            if (info.nomaiCharacterSuit != null)
            { 
                if (material.name.Contains("Character_NOM_Nomai_v2_mat")
                || material.name.Contains("Character_EYE_Nomai_v2_d_mat")
                || material.name.Contains("Character_NOM_NomaiDirty_v2_mat")
                || material.name.Contains("Character_NOM_NomaiDirty_R_v2_mat")
                || material.name.Contains("Character_NOM_NomaiDirty_Advanced_mat")
                || material.name.Contains("Character_NOM_NomaiDirty_Advanced_R_mat")
                )
                {
                    material.mainTexture = RepMan.CustomTexture(info.nomaiCharacterSuit);

                    // Make sure these aren't the LOD versions
                    material.SetTexture("_MetallicGlossMap", RepMan.OWTex("Character_NOM_Nomai_v2_s"));
                    material.SetTexture("_BumpMap", RepMan.OWTex("Character_NOM_Nomai_v2_n"));
                    return material;
                }
            }

            if (info.nomaiMaskTrim != null && material.name.Contains("Props_NOM_Mask_Trim_mat"))
            {
                if (info.nomaiMaskTrim == "Default")
                    return material;
                else if (info.nomaiMaskTrim == "Vessel")
                    return RepMan.OWMat("Structure_NOM_SilverPorcelain_mat");
                else
                {
                    material.mainTexture = RepMan.CustomTexture(info.nomaiMaskTrim);
                    material.SetTexture("_MetallicGlossMap", null);
                    material.SetTexture("_BumpMap", null);
                    return material;
                }
            }

            if (info.style == PropReskinnerStyles.Default) return material;

            string baseMat, metalMat, detailedMat,
                baseTex;

            switch (info.style)
            {
                case PropReskinnerStyles.PreCrashNomai:
                    baseMat = "Structure_NOM_PorcelainClean_mat";
                    baseTex = "Structure_NOM_PorcelainClean_d";
                    metalMat = "Structure_NOM_Silver_mat";
                    detailedMat = "Structure_NOM_SilverPorcelain_mat";
                    break;

                case PropReskinnerStyles.BrittleHollow:
                    baseMat = "Terrain_BH_RockPlain_v2_mat";
                    baseTex = "Terrain_BH_RockSide_d";
                    metalMat = "Terrain_BH_RockGeode_v2_mat";
                    detailedMat = "Terrain_BH_RockGeode_v2_mat"; // Is this the best you got?
                    break;

                default:
                    return material;
            }

            static void ReplaceTexturesFrom(Material dest, Material source, string subtexture = "")
            {
                dest.SetTexture($"_{subtexture}MainTex", source.mainTexture);
                dest.SetTexture($"_{subtexture}MetallicGlossMap", source.GetTexture("_MetallicGlossMap"));
                dest.SetTexture($"_{subtexture}BumpMap", source.GetTexture("_BumpMap"));
            }


            if (material.name.Contains("Structure_NOM_SandStone_mat")
                || material.name.Contains("Structure_NOM_SandStone_Dark_mat")
                || material.name.Contains("Structure_NOM_WallInside_mat") // Detail: stained red, very worn.
                || material.name.Contains("Structure_NOM_Ceiling_mat")
                || material.name.Contains("Structure_NOM_Shuttle_mat") // TODO
                || material.name.Contains("Props_NOM_SmallTractorBeam_mat") // Detail1: TrimPattern. Detail2: Grooves_Red. Detail3: also Grooves_Red.
                || material.name.Contains("Props_NOM_LargeTractorBeam_mat") // TODO
                || material.name.Contains("Structure_NOM_PorcelainBroken_mat") // Masks of Nomai Grave guys
                || material.name.Contains("Structure_NOM_Spiral_Red_mat") // Toy ship
                || material.name.Contains("Structure_NOM_Spiral_Green_mat") // Seen on Ash Twin
                || material.name.Contains("Structure_NOM_Spiral_Yellow_mat") // Seen in Sun Station. Actually more of a tangerine orange.
                || material.name.Contains("ObservatoryInterior_HEA_VillagePlanks_mat")
                )
            {
                return RepMan.OWMat(baseMat);
            }
            else if (material.name.Contains("Structure_NOM_TrimPatternLines_mat")
                || material.name.Contains("Structure_NOM_Grooves_mat") // Detail: zigzag/diamond. Seen on stairs.
                || material.name.Contains("Props_NOM_Computer_mat")
                || material.name.Contains("Structure_NOM_WhiteBoardTile_mat") // Nomai staff keypad
                || material.name.Contains("Structure_NOM_Zigzag_mat") // sandstone but with glass texture as detail ? Rare, seen on Ash Twin, e.g. Ember Twin tower's tractor beam.
                || material.name.Contains("Structure_NOM_Spiral_mat") // 
                )
            {
                // Replace main texture only
                ReplaceTexturesFrom(material, RepMan.OWMat(baseMat));
            }
            if (material.name.Contains("Props_NOM_MaskPainted_mat") // Texture has quarters red, white, turquoise-green, yellow
                )
            {
                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        return material;
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat);
                    case PaintedDetailsMode.AltMaterial:
                        return RepMan.OWMat(detailedMat);
                }
            }
            else if (material.name.Contains("Structure_NOM_PropTile_Color_mat") // Diamonds pattern, yellow and blueish. Used for: SimpleChair (aka bench); Container (aka box with spout).
                || material.name.Contains("Structure_NOM_HexagonTile_mat") // teal and yellow diamonds with space. Used on bed.
                || material.name.Contains("Structure_NOM_WallOutside_mat") // Detail: diagonal square carvings, worn.
                || material.name.Contains("Structure_NOM_Zigzag_Color_mat")
                || material.name.Contains("Structure_NOM_RotatingDoor_mat") // _DetailAlbedoMap _DetailNormalMap
                )
            {
                //_DetailMainTex _DetailMetallicGlossMap _DetailBumpMap

                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        // Replace main texture only
                        material.SetTexture("_MainTex", RepMan.OWTex(baseTex));
                        return material;
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat);
                    case PaintedDetailsMode.AltMaterial:
                        return RepMan.OWMat(detailedMat);
                }
            }
            else if (material.name.Contains("Structure_NOM_TrimPattern_mat") // Atlas of horizontal strips. Details and colour on furnature, Solanum's mask, small box, detailed warp reciever...
                || material.name.Contains("Structure_NOM_Whiteboard_mat") // Whiteboard (very helpful comment I know)
                || material.name.Contains("Structure_NOM_WhiteboardSmall_mat") // Not sure?
                )
            {
                //_DetailMainTex _DetailMetallicGlossMap _DetailBumpMap

                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        // Replace main texture only
                        material.SetTexture("_MainTex", RepMan.OWTex(baseTex));
                        return material;
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat);
                    case PaintedDetailsMode.AltMaterial:
                        return RepMan.OWMat(metalMat); // Keep it simple
                }
            }
            else if (material.name.Contains("Props_NOM_Scroll_mat"))
            {
                ReplaceTexturesFrom(material, RepMan.OWMat(baseMat));
                material.SetTexture("_Detail2MainTex", RepMan.OWMat(metalMat).mainTexture);
            }
            else if (material.name.Contains("Structure_NOM_Airlock_mat"))
            {
                ReplaceTexturesFrom(material, RepMan.OWMat(baseMat));
                ReplaceTexturesFrom(material, RepMan.OWMat(metalMat), "Detail4");
                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        return material;
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat); // TODO
                    case PaintedDetailsMode.AltMaterial:
                        return RepMan.OWMat(detailedMat); // TODO
                }
            }
            else if (material.name.Contains("Structure_NOM_WarpReceiver_mat"))
            {
                ReplaceTexturesFrom(material, RepMan.OWMat(baseMat));

                // Structure_NOM_Spiral_Red
                material.SetTexture("_Detail1MainTex", null);
                material.SetTexture("_Detail1MetallicGlossMap", null);
                material.SetTexture("_Detail1BumpMap", null);

                // Structure_NOM_Grooves
                //material.SetTexture("_Detail2MainTex", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_albedo);
                //material.SetTexture("_Detail2MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_metallicGloss);
                //material.SetTexture("_Detail2BumpMap", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_bump);

                ReplaceTexturesFrom(material, RepMan.OWMat(metalMat), "Detail4");

                material.shader = UnityEngine.Shader.Find("Standard"); // Otherwise it will still be inexplicably sandstone-coloured
            }
            else if (material.name.Contains("Structure_NOM_GravityCannon_mat"))
            {
                ReplaceTexturesFrom(material, RepMan.OWMat(baseMat));

                // Structure_NOM_Spiral_Red_d - used on inside of gravity cannon tube
                material.SetTexture("_Detail1MainTex", null);
                material.SetTexture("_Detail1MetallicGlossMap", null);
                material.SetTexture("_Detail1BumpMap", null);

                // Structure_NOM_Spiral_Yellow_d - used on inside of gravity cannon tube
                material.SetTexture("_Detail2MainTex", null);
                material.SetTexture("_Detail2MetallicGlossMap", null);
                material.SetTexture("_Detail2BumpMap", null);

                // Structure_NOM_WovenGrooves_d - floor tiles where some are painted (used for gravity cannon's path bit)
                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        break;
                    case PaintedDetailsMode.Clean:
                        ReplaceTexturesFrom(material, RepMan.OWMat(baseMat), "Detail3");
                        break;
                    case PaintedDetailsMode.AltMaterial:
                        ReplaceTexturesFrom(material, RepMan.OWMat(detailedMat), "Detail3");
                        break;
                }

                // _Detail4MainTex _Detail4MetallicGlossMap _Detail4BumpMap : OrbitalProbeCannon_NOM_Diamonds_d // TODO
            }
            else if (material.name.Contains("Structure_NOM_Floor_mat")  // floor tiles where some are painted.
                || material.name.Contains("Structure_NOM_WovenGrooves_mat") // Version Seen on Big bridges (BH, TT, ATP)
                )
            {
                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        // Replace main texture only
                        material.SetTexture("_MainTex", RepMan.OWTex(baseTex));
                        return material;
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat);
                    case PaintedDetailsMode.AltMaterial:
                        ReplaceTexturesFrom(material, RepMan.OWMat(baseMat));
                        ReplaceTexturesFrom(material, RepMan.OWMat(detailedMat), "Detail");
                        return material;
                }
            }
            else if (material.name.Contains("Structure_NOM_StarHexagon_Glow_mat") // Gravity floors ON
                || material.name.Contains("IntactModule_NOM_RemoteViewerFloor_mat") // very similar if not the same as above
                || material.name.Contains("IntactModule_NOM_HologramFloor_mat") // Gravity floor but different
                )
            {
                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        // Replace main texture only
                        material.SetTexture("_MainTex", RepMan.OWTex(baseTex));
                        return material;
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat);
                    case PaintedDetailsMode.AltMaterial:
                        return RepMan.OWMat("Structure_NOM_SilverPorcelainGlow_mat");
                }
                //material.SetTexture("_DetailMainTex", null);
                //material.SetTexture("_DetailMetallicGlossMap", null);
                //material.SetTexture("_DetailBumpMap", null);
            }
            else if (material.name.Contains("Structure_NOM_StarHexagon_mat") // gravity floors OFF
                || material.name.Contains("IntactModule_NOM_HologramFloorBroken_mat") // gravity floor but different off
                )
            {
                switch (info.details)
                {
                    case PaintedDetailsMode.Keep:
                        // Replace main texture only
                        material.SetTexture("_MainTex", RepMan.OWTex(baseTex));
                        return material;
                    case PaintedDetailsMode.AltMaterial:
                        return RepMan.OWMat(detailedMat); // Assuming it looks like Structure_NOM_SilverPorcelainGlow_mat but not glowing
                    case PaintedDetailsMode.Clean:
                        return RepMan.OWMat(baseMat);
                }
            }
            else if (material.name.Contains("Structure_NOM_OrbTrack_mat")
                || material.name.Contains("Structure_NOM_ProbeWindow_mat")
                )
            {
                // TODO isn't there a circle version of the orb track material?
                material.SetTexture("_DetailAlbedoMap", RepMan.OWTex(baseTex));
            }
            else if (material.name.Contains("Structure_NOM_Copper_mat")
                || material.name.Contains("Structure_NOM_CopperOld_mat")
                || material.name.Contains("Structure_NOM_CopperOld_Dark_mat")
                || material.name.Contains("ObservatoryInterior_HEA_VillageMetal_mat")
                )
            {
                return RepMan.OWMat(metalMat);
            }
            else if (material.name.Contains("Structure_NOM_SandStone_Darker_mat") // ???? TODO
                || material.name.Contains("Structure_NOM_Grooves_Red_mat") // Stairs found on StatueIsland, SmallBowl
                || material.name.Contains("Props_NOM_Mask_Trim_mat") // Post-crash guys have lines connected with circles. Pre-crash guys just have SilverPorcelain material.
                )
            {
                return RepMan.OWMat(detailedMat);
            }
            else if (material.name.Contains("Props_NOM_WarpCore_mat")) // Black & White Warp Cores
            {
                var mat = RepMan.OWMat(baseMat);
                ReplaceTexturesFrom(material, mat);
                ReplaceTexturesFrom(material, RepMan.OWMat(metalMat), "Detail1");

                // _Detail2 = Structure_NOM_Zigzag (which looks like glass's texture)

                // _Detail3 = Structure_NOM_Grooves_Green
                material.SetTexture("_Detail3MainTex", mat.mainTexture);
                //material.SetTexture("_Detail3MetallicGlossMap", base_metallicGloss);
                //material.SetTexture("_Detail3BumpMap", base_bump);

                // _Detail4 = Structure_NOM_Grooves
            }
            else if (material.name.Contains("Props_NOM_Lamp_mat"))
            {
                return RepMan.OWMat("Props_NOM_VesselLamp_mat");
            }
            else if (material.name.Contains("Character_NOM_NomaiDirty_v2_mat"))
            {
                return RepMan.OWMat("Character_NOM_NomaiDirty_Advanced_mat");
            }
            else if (material.name.Contains("Character_NOM_NomaiDirty_R_v2_mat"))
            {
                return RepMan.OWMat("Character_NOM_NomaiDirty_Advanced_R_mat");
            }
            //else if (material.name.Contains("Structure_NOM_OrbTrack_mat"))
            //{
            //    material.color = new Color(999f, 999f, 999f, 1f); // Turns anything white
            //}
            else if (material.name.Contains("Props_HEA_Lightbulb_mat"))
            {
                material.SetColor("_EmissionColor", new Color(0.6f, 0.7f, 0.8f));
            }

            return material;
        }
    }
}
