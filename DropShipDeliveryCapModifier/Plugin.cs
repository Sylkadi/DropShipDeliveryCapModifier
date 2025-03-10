using BepInEx;
using HarmonyLib;

namespace DropShipDeliveryCapModifier
{
    [HarmonyPatch]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.github.Sylkadi.DropShipDeliveryCapModifier";
        private const string NAME = "DropShipDeliveryCapModifier";
        private const string VERSION = "1.0.0";
        public static readonly Harmony harmony = new Harmony(GUID);

        public static Configuration config { get; private set; }

        void Awake()
        {
            Log.Initalize(Logger);

            config = new Configuration();

            harmony.PatchAll();

            Log.Info($"{NAME} {VERSION} is done patching.");
        }
    }
}
