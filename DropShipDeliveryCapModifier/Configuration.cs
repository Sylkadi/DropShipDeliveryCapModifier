using BepInEx;
using BepInEx.Configuration;

namespace DropShipDeliveryCapModifier
{
    public class Configuration
    {
        public ConfigFile configFile;

        public ConfigEntry<int> deliveryCap, buyCap;

        public Configuration()
        {
            configFile = new ConfigFile($@"{Paths.ConfigPath}\DropShipDeliveryCapModifier.cfg", true);

            deliveryCap = configFile.Bind("Settings", "Delivery Cap", 9999, "Drop ship wont be able to hold more than this.");
            buyCap = configFile.Bind("Settings", "Buy Cap", 999, "You wont be able to buy more than this in a single purchase.");
        }
    }
}
