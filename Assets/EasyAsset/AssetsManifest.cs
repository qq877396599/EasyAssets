using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EasyAsset
{
    [CreateAssetMenu(fileName = "AssetsManifest", order = 0)]
    public class AssetsManifest : ScriptableObject
    {
        public List<AssetData> Manifest = new List<AssetData>();
        public bool IsFinish = false;
        public int Count = 0;
        public void ClearAllData()
        {
            Count = 0;
            IsFinish = false;
            Manifest.Clear();
        }
    }
}
