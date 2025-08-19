using HuaweiUnlocker.Modules.Abstractions;
using HuaweiUnlocker.Modules.Models;
using System.Threading.Tasks;
using static HuaweiUnlocker.FlashTool.FlashToolQClegacy;

namespace HuaweiUnlocker.Modules.Vendors
{
    public class QualcommModule : IDeviceModule
    {
        public string Name => "Qualcomm";
        public ChipsetType Chipset => ChipsetType.Qualcomm;

        public bool IsAvailable() => true;

        public async Task<bool> LoadLoaderAsync(LoaderDefinition def)
        {
            return await Task.Run(() => LoadLoader(def.LoaderPath));
        }

        public bool ErasePartition(string partitionName, LoaderDefinition def)
        {
            return Erase(partitionName, def.LoaderPath);
        }

        public bool WritePartition(string partitionName, string imagePath, LoaderDefinition def)
        {
            return Write(partitionName, def.LoaderPath, imagePath);
        }
    }
}