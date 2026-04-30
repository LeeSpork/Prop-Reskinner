using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PropReskinner.reskinners
{
    internal class PreCrashNomaiClean
    {
        private PropReskinnerStyles style;
        private PaintedDetailsMode nomaiPaintDetails;
        private Material base_mat, metal_mat, fancy_mat, glowing_mat;
        private Texture base_albedo, base_metallicGloss, base_bump,
            metal_albedo, metal_metallicGloss, metal_bump,
            fancy_albedo, fancy_metallicGloss, fancy_bump;

        public void ReskinProp(GameObject prop, PropReskinnerStyles style, PaintedDetailsMode nomaiPaintDetails)
        {
            // Remember parameters in this
            this.style = style;
            this.nomaiPaintDetails = nomaiPaintDetails;
            switch (style)
            {
                case PropReskinnerStyles.PreCrashNomai:
                    base_mat = PropReskinner.Instance.replacementMaterialManager.porcelain;
                    base_albedo = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                    base_metallicGloss = PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss;
                    base_bump = null;

                    metal_mat = PropReskinner.Instance.replacementMaterialManager.silver;
                    metal_albedo = PropReskinner.Instance.replacementMaterialManager.silver_albedo;
                    metal_metallicGloss = PropReskinner.Instance.replacementMaterialManager.silver_metallicGloss;
                    metal_bump = PropReskinner.Instance.replacementMaterialManager.silver_bump;
                    
                    fancy_mat = PropReskinner.Instance.replacementMaterialManager.silverPorcelain;
                    fancy_albedo = PropReskinner.Instance.replacementMaterialManager.silverPorcelain_albedo;
                    fancy_metallicGloss = PropReskinner.Instance.replacementMaterialManager.silverPorcelain_metallicGloss;
                    fancy_bump = PropReskinner.Instance.replacementMaterialManager.silverPorcelain_bump;

                    glowing_mat = PropReskinner.Instance.replacementMaterialManager.silverGlow;
                    
                    break;
            }

            foreach (var renderer in prop.GetComponentsInChildren<Renderer>())
            {
                renderer.materials = [.. renderer.materials.Select(GetReplacementMaterial)];
            }

            // idk what this is
            //foreach (var bml in prop.GetComponentsInChildren<BatchedMaterialLookup>())
            //{
            //    bml.materials = [.. bml.materials.Select(GetReplacementMaterial)];
            //}
        }

        private Material GetReplacementMaterial(Material material)
        {
            if (material.name.Contains("Structure_NOM_SandStone_mat")
                || material.name.Contains("Structure_NOM_SandStone_Dark_mat")
                || material.name.Contains("Structure_NOM_Grooves_mat")
                || material.name.Contains("Structure_NOM_Floor_mat")
                || material.name.Contains("Structure_NOM_WallInside_mat")
                || material.name.Contains("Structure_NOM_Ceiling_mat")
                || material.name.Contains("Structure_NOM_WallOutside_mat")
                || material.name.Contains("Structure_NOM_Shuttle_mat")
                || material.name.Contains("Props_NOM_SmallTractorBeam_mat")
                || material.name.Contains("Props_NOM_LargeTractorBeam_mat")
                || material.name.Contains("Structure_NOM_PorcelainBroken_mat")
                || material.name.Contains("Structure_NOM_Spiral_Red_mat") // Toy ship
                || material.name.Contains("Structure_NOM_Spiral_Green_mat") // Seen on Ash Twin
                || material.name.Contains("ObservatoryInterior_HEA_VillagePlanks_mat")
                )
            {
                return base_mat;
            }
            if (material.name.Contains("Props_NOM_MaskPainted_mat") // Texture has quarters red, white, turquoise-green, yellow
                )
            {
                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        return material;
                    case PaintedDetailsMode.Removed:
                        return base_mat;
                    case PaintedDetailsMode.AltTexture:
                        return fancy_mat;
                }
            }
            else if (material.name.Contains("Structure_NOM_TrimPatternLines_mat")
                || material.name.Contains("Props_NOM_Computer_mat")
                || material.name.Contains("Structure_NOM_WhiteBoardTile_mat") // Nomai staff keypad
                || material.name.Contains("Structure_NOM_Zigzag_mat") // sandstone but with glass texture as detail ? Rare, seen on Ash Twin, e.g. Ember Twin tower's tractor beam.
                || material.name.Contains("Structure_NOM_Spiral_mat") // 
                )
            {
                // Replace main texture only
                material.mainTexture = base_albedo;
                material.SetTexture("_MetallicGlossMap", base_metallicGloss);
            }
            else if (material.name.Contains("Structure_NOM_PropTile_Color_mat") // Diamonds pattern, yellow and blueish. Used for: SimpleChair (aka bench); Container (aka box with spout).
                || material.name.Contains("Structure_NOM_HexagonTile_mat") // teal and yellow diamonds with space. Used on bed.
                )
            {
                //_DetailMainTex _DetailMetallicGlossMap _DetailBumpMap

                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        // Replace main texture only
                        material.mainTexture = base_albedo;
                        material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                        material.SetTexture("_BumpMap", base_bump);
                        return material;
                    case PaintedDetailsMode.Removed:
                        return base_mat;
                    case PaintedDetailsMode.AltTexture:
                        return fancy_mat;
                }
            }
            else if (material.name.Contains("Structure_NOM_TrimPattern_mat") // Atlas of horizontal strips. Details and colour on furnature, Solanum's mask, small box, detailed warp reciever...
                || material.name.Contains("Structure_NOM_Whiteboard_mat") // Whiteboard (very helpful comment I know)
                || material.name.Contains("Structure_NOM_WhiteboardSmall_mat") // Not sure?
                )
            {
                //_DetailMainTex _DetailMetallicGlossMap _DetailBumpMap

                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        // Replace main texture only
                        material.mainTexture = base_albedo;
                        material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                        material.SetTexture("_BumpMap", base_bump);
                        return material;
                    case PaintedDetailsMode.Removed:
                        return base_mat;
                    case PaintedDetailsMode.AltTexture:
                        return metal_mat;
                }
            }
            else if (material.name.Contains("Props_NOM_Scroll_mat"))
            {
                material.mainTexture = base_albedo;
                material.SetTexture("_Detail2MainTex", metal_mat.mainTexture);
            }
            else if (material.name.Contains("Structure_NOM_Airlock_mat"))
            {
                material.mainTexture = base_albedo;
                material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                material.SetTexture("_Detail4MainTex", metal_mat.mainTexture);
            }
            else if (material.name.Contains("Structure_NOM_WarpReceiver_mat"))
            {
                material.mainTexture = base_albedo;
                material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                material.SetTexture("_BumpMap", base_bump);

                // Structure_NOM_Spiral_Red
                material.SetTexture("_Detail1MainTex", null);
                material.SetTexture("_Detail1MetallicGlossMap", null);
                material.SetTexture("_Detail1BumpMap", null);

                // Structure_NOM_Grooves
                //material.SetTexture("_Detail2MainTex", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_albedo);
                //material.SetTexture("_Detail2MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_metallicGloss);
                //material.SetTexture("_Detail2BumpMap", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_bump);

                material.SetTexture("_Detail4MainTex", metal_mat.mainTexture);

                material.shader = UnityEngine.Shader.Find("Standard"); // Otherwise it will still be inexplicably sandstone-coloured
            }
            else if (material.name.Contains("Structure_NOM_GravityCannon_mat"))
            {
                material.mainTexture = base_albedo;
                material.SetTexture("_MetallicGlossMap", base_metallicGloss);

                // Structure_NOM_Spiral_Red_d - used on inside of gravity cannon tube
                material.SetTexture("_Detail1MainTex", null);
                material.SetTexture("_Detail1MetallicGlossMap", null);
                material.SetTexture("_Detail1BumpMap", null);

                // Structure_NOM_Spiral_Yellow_d - used on inside of gravity cannon tube
                material.SetTexture("_Detail2MainTex", null);
                material.SetTexture("_Detail2MetallicGlossMap", null);
                material.SetTexture("_Detail2BumpMap", null);

                // Structure_NOM_WovenGrooves_d - floor tiles where some are painted (used for gravity cannon's path bit)
                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        break;
                    case PaintedDetailsMode.Removed:
                        material.SetTexture("_Detail3MainTex", base_albedo);
                        material.SetTexture("_Detail3MetallicGlossMap", base_metallicGloss);
                        material.SetTexture("_Detail3BumpMap", base_bump);
                        break;
                    case PaintedDetailsMode.AltTexture:
                        material.SetTexture("_Detail3MainTex", fancy_albedo);
                        material.SetTexture("_Detail3MetallicGlossMap", fancy_metallicGloss);
                        material.SetTexture("_Detail3BumpMap", fancy_bump);
                        break;
                }

                // _Detail4MainTex _Detail4MetallicGlossMap _Detail4BumpMap : OrbitalProbeCannon_NOM_Diamonds_d
            }
            else if (material.name.Contains("Structure_NOM_WovenGrooves_mat") // floor tiles where some are painted. Seen on Big bridges (BH, TT, ATP)
                || material.name.Contains("Structure_NOM_StarHexagon_mat") // non-gravity floors
                )
            {
                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        // Replace main texture only
                        material.mainTexture = base_albedo;
                        material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                        material.SetTexture("_BumpMap", base_bump);
                        return material;
                    case PaintedDetailsMode.AltTexture:
                        material.SetTexture("_DetailMainTex", fancy_albedo);
                        material.SetTexture("_DetailMetallicGlossMap", fancy_metallicGloss);
                        material.SetTexture("_DetailBumpMap", fancy_bump);
                        goto case PaintedDetailsMode.Faded; // also replace main tex
                    case PaintedDetailsMode.Removed:
                        return base_mat;
                }
            }
            else if (material.name.Contains("Structure_NOM_StarHexagon_Glow_mat")) // Gravity floors
            {
                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        // Replace main texture only
                        material.mainTexture = base_albedo;
                        material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                        material.SetTexture("_BumpMap", base_bump);
                        return material;
                    case PaintedDetailsMode.Removed:
                        return base_mat;
                    case PaintedDetailsMode.AltTexture:
                        //return glowing_mat;
                        material.mainTexture = base_albedo;
                        material.SetTexture("_MetallicGlossMap", glowing_mat.GetTexture("_MetallicGlossMap"));
                        material.SetTexture("_BumpMap", glowing_mat.GetTexture("_BumpMap"));
                        return material;
                }
                //material.SetTexture("_DetailMainTex", null);
                //material.SetTexture("_DetailMetallicGlossMap", null);
                //material.SetTexture("_DetailBumpMap", null);
            }
            else if (material.name.Contains("Structure_NOM_OrbTrack_mat")
                || material.name.Contains("Structure_NOM_ProbeWindow_mat")
                )
            {
                material.SetTexture("_DetailAlbedoMap", base_albedo);
            }
            if (material.name.Contains("Structure_NOM_RotatingDoor_mat")
                )
            {
                switch (nomaiPaintDetails)
                {
                    case PaintedDetailsMode.Faded:
                        // Replace main texture only
                        material.mainTexture = base_albedo;
                        material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                        material.SetTexture("_BumpMap", base_bump);
                        return material;
                    case PaintedDetailsMode.Removed:
                        return base_mat;
                    case PaintedDetailsMode.AltTexture:
                        return fancy_mat;
                }
                // _DetailAlbedoMap _DetailNormalMap
            }
            else if (material.name.Contains("Structure_NOM_Copper_mat")
                || material.name.Contains("Structure_NOM_CopperOld_mat")
                || material.name.Contains("Structure_NOM_CopperOld_Dark_mat")
                || material.name.Contains("ObservatoryInterior_HEA_VillageMetal_mat")
                )
            {
                return metal_mat;
            }
            else if (material.name.Contains("Structure_NOM_SandStone_Darker_mat")
                || material.name.Contains("Structure_NOM_Grooves_Red_mat") // SmallBowl
                || material.name.Contains("Props_NOM_Mask_Trim_mat") // Post-crash guys have lines connected with circles. Pre-crash guys just have SilverPorcelain material.
                )
            {
                return fancy_mat;
            }
            else if (material.name.Contains("Props_NOM_WarpCore_mat")) // Black & White Warp Cores
            {
                material.mainTexture = base_albedo;
                material.SetTexture("_MetallicGlossMap", base_metallicGloss);
                material.SetTexture("_BumpMap", base_bump);

                material.SetTexture("_Detail1MainTex", metal_albedo);
                material.SetTexture("_Detail1MetallicGlossMap", metal_metallicGloss);
                material.SetTexture("_Detail1BumpMap", metal_bump);

                // _Detail2 = Structure_NOM_Zigzag (which looks like glass's texture)

                // _Detail3 = Structure_NOM_Grooves_Green
                material.SetTexture("_Detail3MainTex", base_albedo);
                //material.SetTexture("_Detail3MetallicGlossMap", base_metallicGloss);
                //material.SetTexture("_Detail3BumpMap", base_bump);

                // _Detail4 = Structure_NOM_Grooves
            }
            else if (material.name.Contains("Props_NOM_Lamp_mat"))
            {
                return PropReskinner.Instance.replacementMaterialManager.vesselLamp;
            }
            else if (material.name.Contains("Character_NOM_NomaiDirty_v2_mat"))
            {
                return PropReskinner.Instance.replacementMaterialManager.nomaiAdvancedSuitDirty; // Not actually clean but its a corpse so eh
            }
            else if (material.name.Contains("Character_NOM_NomaiDirty_R_v2_mat"))
            {
                return PropReskinner.Instance.replacementMaterialManager.nomaiAdvancedSuitDirtyR; // ditto
            }
            else if (material.name.Contains("Character_NOM_Nomai_v2_mat"))
            {
                //material.mainTexture = nomaiSuit;
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
