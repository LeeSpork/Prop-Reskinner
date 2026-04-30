using System.Reflection;
using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using PropReskinner.reskinners;
using UnityEngine;

namespace PropReskinner
{
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
            ModHelper.Console.WriteLine($"Thank you for using {nameof(PropReskinner)}", MessageType.Success);
            // Mod {nameof(PropReskinner)} loaded!
            // I will reskin your children. // Rare

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
                        preCrashNomaiClean.ReskinProp(prop, info.style, info.paintedDetails);
                    }
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
