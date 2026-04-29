using OWML.Common;
using OWML.ModHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PropReskinner
{
    public class ReplacementAssetManager
    {
        public Material porcelain, silver, silverPorcelain, silverGlow, vesselLamp,
            dreamWood, dreamWoodLight,
            nomaiAdvancedSuitDirty, nomaiAdvancedSuitDirtyR;

        public Texture porcelain_albedo, porcelain_metallicGloss, silverPorcelain_albedo, silverPorcelain_metallicGloss, silverPorcelain_bump, silverGlow_emission,
            dreamWood_albedo, dreamWood_metallicGloss, dreamWood_bump, dreamWoodLight_albedo, dreamWoodLight_metallicGloss, dreamWoodLight_bump;

        public ReplacementAssetManager() {
            porcelain               = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_PorcelainClean_mat"));
            silver                  = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_Silver_mat"));
            silverPorcelain         = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_SilverPorcelain_mat"));
            silverGlow              = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_SilverPorcelainGlow_mat"));
            vesselLamp              = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Props_NOM_VesselLamp_mat"));

            dreamWood               = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_DW_Mangrove_Wood_mat"));
            dreamWoodLight          = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_DW_Mangrove_Wood_Light_mat"));

            nomaiAdvancedSuitDirtyR = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Character_NOM_NomaiDirty_Advanced_R_mat"));
            nomaiAdvancedSuitDirty  = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Character_NOM_NomaiDirty_Advanced_mat"));

            porcelain_albedo              = porcelain.GetTexture("_DetailAlbedoMap");
            porcelain_metallicGloss       = porcelain.GetTexture("_MetallicGlossMap");
            silverPorcelain_albedo        = silverPorcelain.GetTexture("_DetailAlbedoMap");
            silverPorcelain_metallicGloss = silverPorcelain.GetTexture("_MetallicGlossMap");
            silverPorcelain_bump          = silverPorcelain.GetTexture("_BumpMap");
            silverGlow_emission           = silverGlow.GetTexture("_EmissionMap");

            dreamWoodLight_albedo           = dreamWood.mainTexture;
            dreamWoodLight_metallicGloss    = dreamWood.GetTexture("_MetallicGlossMap");
            dreamWoodLight_bump             = dreamWood.GetTexture("_BumpMap");
            dreamWoodLight_albedo           = dreamWoodLight.mainTexture;
            dreamWoodLight_metallicGloss    = dreamWoodLight.GetTexture("_MetallicGlossMap");
            dreamWoodLight_bump             = dreamWoodLight.GetTexture("_BumpMap");

            PropReskinner.Instance.ModHelper.Console.WriteLine("Got replacement materials!", MessageType.Success);
        }
    }
}
