using HuaweiUnlocker.Modules.Models;
using System.Threading.Tasks;

namespace HuaweiUnlocker.Modules.Abstractions
{
    public interface IDeviceModule
    {
        string Name { get; }
        ChipsetType Chipset { get; }
        bool IsAvailable();
        Task<bool> LoadLoaderAsync(LoaderDefinition def);
        bool ErasePartition(string partitionName, LoaderDefinition def);
        bool WritePartition(string partitionName, string imagePath, LoaderDefinition def);
    }
}