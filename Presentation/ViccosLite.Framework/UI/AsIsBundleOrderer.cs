﻿using System.Collections.Generic;
using System.Web.Optimization;

namespace ViccosLite.Framework.UI
{
    public class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}