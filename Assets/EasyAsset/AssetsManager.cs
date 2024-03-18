using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EasyAsset
{
    public class AssetsManager : MonoBehaviour
    {

        public static AssetsManager Instance { get; private set; }

        [SerializeField]
        AssetsManifest assetManifest;
        List<string> AlreadyLoadedBundle;
        Dictionary<string, AssetData> AllAssetData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Init();
            }

        }
        private void Init()
        {

            assetManifest = LoadManifest("Assets/EasyAsset/Manifest/AssetsManifest.asset");
            if (assetManifest == null)
            {
                Debug.LogError($"AssetManifest is null");
                return;
            }
            AlreadyLoadedBundle = new List<string>();

            AllAssetData = new Dictionary<string, AssetData>();

            foreach (var item in assetManifest.Manifest)
            {
                if (!AllAssetData.ContainsKey(item.Path))
                {
                    AllAssetData.Add(item.Path, item);
                }
            }
            Debug.Log(assetManifest.Manifest.Count);
            foreach (var item in assetManifest.Manifest)
            {
                foreach (var bundle in item.RequireBundle)
                {
                    Debug.Log($"{item.Path}==>>{bundle}");
                }
            }
        }
        void Start()
        {

            var GO3 = LoadAsset<GameObject>("Assets/Prefabs/GO3.prefab");
            Instantiate<GameObject>(GO3);
            var cube = LoadAsset<GameObject>("Assets/Prefabs/CubeRed.prefab");
            Instantiate<GameObject>(cube);
        }

        void Update()
        {

        }
        AssetsManifest LoadManifest(string path)
        {
            string absolutePath = Utility.GetAboluteBundlePath() + "/" + path.ToLower();
            var assetBundle = AssetBundle.LoadFromFile(absolutePath);
            var asset = assetBundle.LoadAsset<AssetsManifest>(path);
            return asset;
        }
        public T LoadAsset<T>(string path)where T:Object
        {
            var assetBundle = LoadRequireBundle(path);

            var asset = assetBundle.LoadAsset<T>(path);
            return asset;
        }
        AssetBundle LoadRequireBundle(string assetPath)
        {
            AssetBundle assetBundle = null;

            //if (!AlreadyLoadedBundle.Contains(bundlePath))
            //{

            //    if (AllAssetData.ContainsKey(bundlePath))
            //    {
            //        string absolutePath = Utility.GetAboluteBundlePath() + "/" + bundlePath.ToLower();

            //        assetBundle = AssetBundle.LoadFromFile(absolutePath);
            //        AlreadyLoadedBundle.Add(bundlePath);
            //        for (int i = 0; i < AllAssetData[bundlePath].RequireBundle.Count; i++)
            //        {
            //            int index = i;
            //            LoadRequireBundle(AllAssetData[bundlePath].RequireBundle[index]);

            //        }
            //    }
            //}
            if (AllAssetData.ContainsKey(assetPath))
            {
                for (int i = 0; i < AllAssetData[assetPath].RequireBundle.Count; i++)
                {
                    if (!AlreadyLoadedBundle.Contains(AllAssetData[assetPath].RequireBundle[i]))
                    {
                        string absolutePath = Utility.GetAboluteBundlePath() + "/" + AllAssetData[assetPath].RequireBundle[i].ToLower();
                        if (i == 0)
                        {
                            assetBundle = AssetBundle.LoadFromFile(absolutePath);
                        }
                        else
                        {
                            AssetBundle.LoadFromFile(absolutePath);
                        }
                        AlreadyLoadedBundle.Add(AllAssetData[assetPath].RequireBundle[i]);
                    }

                }
            }
            return assetBundle;
        }
        public void UNloadAllAsset()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }
        private void OnDestroy()
        {
            UNloadAllAsset();
        }
    }
}


