using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using PropReskinner.reskinners;
using UnityEngine;

namespace PropReskinner
{
    public class PropReskinnerInfo
    {
        /// <summary>
        /// Paths to props that you want to get reskinned to the porcelain and silver metal materals of pre-crash Nomai structures (the vessel and escape pods).
        /// Will reskin any children as well.
        /// </summary>
        public string[] preCrashNomaiClean;

        /// <summary>
        /// Dusty wood of the Stranger (aka RingWorld) (Requires Echoes of the Eye DLC)
        /// </summary>
        //public string[] stranger;//TODO: and metal?

        /// <summary>
        /// Clean treated wood of the DreamWorld (Requires Echoes of the Eye DLC)
        /// </summary>
        public string[] dreamWorld;
    }

    public class PropReskinner : ModBehaviour
    {
        public static PropReskinner Instance;
        public INewHorizons NewHorizons;
        public ReplacementAssetManager replacementMaterialManager;

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
            ModHelper.Console.WriteLine($"Thank you for using {nameof(PropReskinner)}.", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            new Harmony("LeeSpork.PropReskinner").PatchAll(Assembly.GetExecutingAssembly());

            //OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;

            PreCrashNomaiClean preCrashNomaiClean = new();

            // Add extention to New Horizons planet config
            NewHorizons.GetBodyLoadedEvent().AddListener((name) =>
            {
                ModHelper.Console.WriteLine($"Body {name} loaded!", MessageType.Info);
                var info = NewHorizons.QueryBody<PropReskinnerInfo>(name, "$.extras.PropReskinner");
                if (info != null)
                {
                    var planet = NewHorizons.GetPlanet(name);
                    
                    ModHelper.Console.WriteLine("Reskinning stuff!", MessageType.Info);

                    foreach (string path in info.preCrashNomaiClean)
                    {
                        var prop = planet.transform.Find(path).gameObject;
                        preCrashNomaiClean.ReskinProp(prop);
                    }

                    //foreach (string path in info.dreamWorld)
                    //{
                    //    var prop = planet.transform.Find(path).gameObject;
                    //    dreamWorld.ReskinProp(prop);
                    //}
                }
            });
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            //ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

            replacementMaterialManager = new(); // Probably not nessecary to do every time the scene is loaded?
        }
    }

}
