using System.IO;
using UnityEditor;

public class CreatBundles
{
    
    [MenuItem("Tool/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string dir = "AssetBundle";
        if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
        BuildPipeline.BuildAssetBundles(dir,BuildAssetBundleOptions.None,BuildTarget.StandaloneWindows64);
    }
}
