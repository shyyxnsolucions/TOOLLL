using System;
using System.Collections.Generic;

namespace HuaweiUnlocker.Modules.Models
{
    [Serializable]
    public class LoaderManifest
    {
        public List<LoaderDefinition> Loaders { get; set; } = new List<LoaderDefinition>();
    }
}