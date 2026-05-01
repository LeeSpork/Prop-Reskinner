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
        public const string DREAM_WOOD = "Structure_DW_Mangrove_Wood_mat";
        public const string DREAM_WOOD_LIGHT = "Structure_DW_Mangrove_Wood_Light_mat";

        private readonly Dictionary<string, Material> outerWildsMaterials = [];

        public Material OWMat(string name) // Get a material that is already in the game
        {
            try
            {
                return outerWildsMaterials[name];
            }
            catch (KeyNotFoundException)
            {
                Material mat = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains(name));
                outerWildsMaterials[name] = mat;
                return mat;
            }
        }
    }
}
