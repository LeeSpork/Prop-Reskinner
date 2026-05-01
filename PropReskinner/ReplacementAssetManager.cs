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
        private readonly Dictionary<string, Texture> outerWildsTextures = [];
        private readonly Dictionary<string, Texture> customTextures = [];

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

        public Texture OWTex(string name) // Get a material that is already in the game
        {
            try
            {
                return outerWildsTextures[name];
            }
            catch (KeyNotFoundException)
            {
                Texture mat = Resources.FindObjectsOfTypeAll<Texture>().First(x => x.name.Contains(name));
                outerWildsTextures[name] = mat;
                return mat;
            }
        }

        public Texture CustomTexture(string path)
        {
            try
            {
                return customTextures[path];
            }
            catch (KeyNotFoundException)
            {
                Texture tex;
                try
                {
                    tex = PropReskinner.Instance.ModHelper.Assets.GetTexture(path);
                } catch (System.IO.DirectoryNotFoundException)
                {
                    PropReskinner.Instance.ModHelper.Console.WriteLine($"Could not find part of path: {path}", MessageType.Error);
                    return null;
                }
                customTextures[path] = tex;
                return tex;
            }
        }
    }
}
