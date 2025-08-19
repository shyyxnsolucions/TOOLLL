using HuaweiUnlocker.Modules.Abstractions;
using HuaweiUnlocker.Modules.Models;
using System.Threading.Tasks;
using HuaweiUnlocker.FlashTool;

namespace HuaweiUnlocker.Modules.Vendors
{
    public class MTKModule : IDeviceModule
    {
        public string Name => "MediaTek";
        public ChipsetType Chipset => ChipsetType.MediaTek;

        public bool IsAvailable() => true;

        public async Task<bool> LoadLoaderAsync(LoaderDefinition def)
        {
            return await Task.Run(() => MTKFlash.FlashScatter(def.RawprogramXmlPath, def.LoaderPath, "download", true));
        }

        public bool ErasePartition(string partitionName, LoaderDefinition def)
        {
            // Placeholder. Erase support depends on external tool route.
            return true;
        }

        public bool WritePartition(string partitionName, string imagePath, LoaderDefinition def)
        {
            // Placeholder.
            return true;
        }
    }
}