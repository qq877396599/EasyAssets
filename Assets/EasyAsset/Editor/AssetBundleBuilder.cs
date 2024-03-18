using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
namespace EasyAsset
{
    /*
     * version 1.0
     * ��ʱֻ�ܰ����ļ��������Դ
     * 
     * 
     * */
    public static class AssetBundleBuilder
    {
        [MenuItem("EasyAsset/CreateAssetBundle")]
        public static void CreateAssetBundle()
        {
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            //��ȡ������Ҫ�������Դ·��
            var allSourcePath = LoadAllFilesPath();

            //Ϊ������Դ��ab���
            SetAssetsBundleName(allSourcePath);
            //��ȡab�����·��
            string outputPath = GetOutputPath();
            //ɾ�����е�ab��
            ClearAssetsBundle(outputPath);
            //���·��������Ӧ���ļ���
            CreateDirectory(outputPath);
            //�洢������ϵ
            GenerateManifest(allSourcePath);
            //���
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
        }

        public static void GenerateManifest(List<string> allSourcePath)
        {
            var assetsManifest = AssetDatabase.LoadAssetAtPath<AssetsManifest>("Assets/EasyAsset/Manifest/AssetsManifest.asset");
            /*
             * Test
             *var ab = AssetBundle.LoadFromFile(Utility.GetAboluteBundlePath() + "/Assets/EasyAsset/Manifest/AssetsManifest.asset".ToLower());
             *var assetsManifest = ab.LoadAsset<AssetsManifest>("Assets/EasyAsset/Manifest/AssetsManifest.asset");
            */
            assetsManifest.Manifest.Clear();
            assetsManifest.IsFinish = false;
            assetsManifest.Count = 0;
            for (int i = 0; i < allSourcePath.Count; i++)
            {
                var asset = new AssetData();
                asset.Path = allSourcePath[i];
                assetsManifest.Manifest.Add(asset);
                var abName = AssetDatabase.GetImplicitAssetBundleName(allSourcePath[i]);
                asset.RequireBundle.Add(abName); ;
                var Dependencies = AssetDatabase.GetAssetBundleDependencies(abName, true);
                asset.RequireBundle.AddRange(Dependencies);
            }
            assetsManifest.IsFinish = true;
            assetsManifest.Count = allSourcePath.Count;
        }

        public static List<string> LoadAllFilesPath()
        {
            List<string> AllSourcePath = new List<string>();
            AllSourcePath.Clear();
            string projectPath = Application.dataPath;
            string[] filePaths = Directory.GetFiles(projectPath, "*", SearchOption.AllDirectories);
            foreach (var filePath in filePaths)
            {
                if (Array.Exists(Parameter.Ignor_Suffix, item => filePath.EndsWith(item)))
                {
                    continue;
                }
                string relativePath = "Assets" + filePath.Replace(projectPath, "").Replace("\\", "/");


                if (Array.Exists(Parameter.Ignor_Prefix, item => relativePath.StartsWith(item)))
                {
                    continue;
                }
                AllSourcePath.Add(relativePath);
                Debug.Log($"Find path:{relativePath}");

            }
            return AllSourcePath;
        }
        public static void SetAssetsBundleName(List<string> allSourcePath)
        {
            foreach (var item in allSourcePath)
            {
                Debug.Log(item);
                AssetImporter.GetAtPath(item).assetBundleName = item;
            }
        }

        public static string GetOutputPath()
        {
            return Parameter.OutputPath + "/" + Utility.GetPlatformeName();
        }
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        static void ClearAssetsBundle(string folderPath)
        {
            Utility.DeleteFilesAndSubDirectories(folderPath);
        }
        [MenuItem("EasyAsset/TestClearAssetsBundle")]
        public static void TestClearAssetsBundle()
        {
            string outputPath = GetOutputPath();
            //ɾ�����е�ab��
            ClearAssetsBundle(outputPath);
        }
    }

}
