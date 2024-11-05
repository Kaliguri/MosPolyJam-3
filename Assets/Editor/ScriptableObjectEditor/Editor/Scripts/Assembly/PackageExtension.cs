using System.IO;
using UnityEditor;
using UnityEngine;

namespace Agent.Assembly
{
    public class PackageExtension
    {
        private const string packageName = "ScriptableObjectEditor";
        private static string packagePath = $"{Application.dataPath.Replace("Assets", "")}Packages/{packageName}";
        private static string installPath = $"{Application.dataPath.Replace("Assets", "")}Assets/Editor/{packageName}";

        [MenuItem("SOE/Relocate to Packages", true, priority = 1)]
        public static bool ValidateScriptablePackage()
        {
            var why = !Directory.Exists(packagePath);
            return why;
        }

        [MenuItem("SOE/Relocate to Packages", false, priority = 1)]
        public static void RelocateScriptablePackage()
        {
            try
            {
                FileUtil.MoveFileOrDirectory(installPath, packagePath);
                AssetDatabase.Refresh();

                Debug.Log("<color=green>Sucess!</color> Scriptable Object Editor converted to Package.");
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }
    }
}