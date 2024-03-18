using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasyAsset
{
    [Serializable]
    public class AssetData
    {
        public string Path;
        public List<string> RequireBundle = new List<string>();
    }

}
