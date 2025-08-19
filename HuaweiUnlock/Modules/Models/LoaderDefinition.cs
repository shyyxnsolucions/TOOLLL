using System;
using System.Collections.Generic;

namespace HuaweiUnlocker.Modules.Models
{
    [Serializable]
    public class LoaderDefinition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Vendor { get; set; }
        public string Model { get; set; }
        public ChipsetType Chipset { get; set; } = ChipsetType.Unknown;
        public string LoaderPath { get; set; }           // .mbn/.elf/.bin
        public string RawprogramXmlPath { get; set; }    // optional
        public string PatchXmlPath { get; set; }         // optional
        public bool AllowRead { get; set; } = true;
        public bool AllowWrite { get; set; } = true;
        public bool AllowErase { get; set; } = true;
    }
}