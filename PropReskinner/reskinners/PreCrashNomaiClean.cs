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

        public void ReskinProp(GameObject prop)
        {
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
            if (material.name.Contains("Structure_NOM_Whiteboard_mat")
                || material.name.Contains("Structure_NOM_WhiteboardSmall_mat")
                || material.name.Contains("Structure_NOM_SandStone_mat")
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
                || material.name.Contains("Props_NOM_MaskPainted_mat") // Texture has quarters red, white, turquoise-green, yellow
                || material.name.Contains("Structure_NOM_Spiral_Red_mat") // Toy ship
                || material.name.Contains("ObservatoryInterior_HEA_VillagePlanks_mat")
                )
            {
                return PropReskinner.Instance.replacementMaterialManager.porcelain;
            }
            else if (material.name.Contains("Structure_NOM_WhiteBoardTile_mat") // Nomai staff keypad
                || material.name.Contains("Structure_NOM_PropTile_Color_mat") // Bench/SimpleChair
                || material.name.Contains("Structure_NOM_TrimPattern_mat") // details and colour on furnature, small box
                || material.name.Contains("Structure_NOM_HexagonTile_mat")
                || material.name.Contains("Structure_NOM_TrimPatternLines_mat")
                || material.name.Contains("Props_NOM_Computer_mat")
                )
            {
                material.mainTexture = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                material.SetTexture("_MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);
            }
            else if (material.name.Contains("Props_NOM_Scroll_mat"))
            {
                material.mainTexture = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                material.SetTexture("_Detail2MainTex", PropReskinner.Instance.replacementMaterialManager.silver.mainTexture);
            }
            else if (material.name.Contains("Structure_NOM_Airlock_mat"))
            {
                material.mainTexture = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                material.SetTexture("_MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);
                material.SetTexture("_Detail4MainTex", PropReskinner.Instance.replacementMaterialManager.silver.mainTexture);
            }
            else if (material.name.Contains("Structure_NOM_WarpReceiver_mat"))
            {
                material.mainTexture = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                material.SetTexture("_MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);
                material.SetTexture("_BumpMap", null);

                // Structure_NOM_Spiral_Red
                material.SetTexture("_Detail1MainTex", null);
                material.SetTexture("_Detail1MetallicGlossMap", null);
                material.SetTexture("_Detail1BumpMap", null);

                // Structure_NOM_Grooves
                //material.SetTexture("_Detail2MainTex", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_albedo);
                //material.SetTexture("_Detail2MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_metallicGloss);
                //material.SetTexture("_Detail2BumpMap", PropReskinner.Instance.replacementMaterialManager.silverPorcelain_bump);

                material.SetTexture("_Detail4MainTex", PropReskinner.Instance.replacementMaterialManager.silver.mainTexture);

                material.shader = UnityEngine.Shader.Find("Standard"); // Otherwise it will still be inexplicably sandstone-coloured
            }
            else if (material.name.Contains("Structure_NOM_GravityCannon_mat"))
            {
                material.mainTexture = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                material.SetTexture("_MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);

                // Structure_NOM_Spiral_Red_d
                material.SetTexture("_Detail1MainTex", PropReskinner.Instance.replacementMaterialManager.porcelain_albedo);
                material.SetTexture("_Detail1MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);
                material.SetTexture("_Detail1BumpMap", null);

                // Structure_NOM_Spiral_Yellow_d
                material.SetTexture("_Detail2MainTex", PropReskinner.Instance.replacementMaterialManager.porcelain_albedo);
                material.SetTexture("_Detail2MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);
                material.SetTexture("_Detail2BumpMap", null);
            }
            else if (material.name.Contains("Structure_NOM_OrbTrack_mat")
                || material.name.Contains("Structure_NOM_ProbeWindow_mat")
                )
            {
                material.SetTexture("_DetailAlbedoMap", PropReskinner.Instance.replacementMaterialManager.porcelain_albedo);
            }
            else if (material.name.Contains("Structure_NOM_Copper_mat")
                || material.name.Contains("Structure_NOM_CopperOld_mat")
                || material.name.Contains("Structure_NOM_CopperOld_Dark_mat")
                || material.name.Contains("ObservatoryInterior_HEA_VillageMetal_mat")
                )
            {
                return PropReskinner.Instance.replacementMaterialManager.silver;
            }
            else if (material.name.Contains("Structure_NOM_SandStone_Darker_mat")
                || material.name.Contains("Structure_NOM_Grooves_Red_mat") // SmallBowl
                || material.name.Contains("Props_NOM_Mask_Trim_mat") // Post-crash guys have lines connected with circles. Pre-crash guys just have SilverPorcelain material.
                )
            {
                return PropReskinner.Instance.replacementMaterialManager.silverPorcelain;
            }
            else if (material.name.Contains("Structure_NOM_StarHexagon_Glow_mat")) // Gravity floors
            {
                //return PropReskinner.Instance.replacementMaterialManager.silverGlow;
                
                material.mainTexture = PropReskinner.Instance.replacementMaterialManager.porcelain_albedo;
                material.SetTexture("_MetallicGlossMap", PropReskinner.Instance.replacementMaterialManager.porcelain_metallicGloss);
                material.SetTexture("_BumpMap", null);

                //material.SetTexture("_DetailMainTex", null);
                //material.SetTexture("_DetailMetallicGlossMap", null);
                //material.SetTexture("_DetailBumpMap", null);
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
