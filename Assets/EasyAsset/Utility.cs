using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Utility
{

    public static string GetDirectoryPath(string path)
    {
        var directionries = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        string dirPath = "";
        for (int i = 0; i < directionries.Length; i++)
        {
            dirPath += directionries[i];
        }
        return dirPath;
    }
    public static string GetPlatformeName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "IOS";
            default:
                return "PC";
        }
    }
    public static string GetAboluteBundlePath()
    {
        string absolutePath = "";
#if UNITY_EDITOR
        absolutePath += System.Environment.CurrentDirectory;
#else
         absolutePath += Application.temporaryCachePath;
#endif
        absolutePath = absolutePath + "/AssetBundles/"+ GetPlatformeName();

        return absolutePath;
    }
    public static void DeleteFilesAndSubDirectories(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] files = directoryInfo.GetFiles();

        foreach (FileInfo file in files)
        {
            file.Delete();
        }
        var directoryInfos = directoryInfo.GetDirectories();
        foreach (var item in directoryInfos)
        {
            item.Delete(true);
        }
    }
}
