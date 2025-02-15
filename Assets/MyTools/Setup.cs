using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

namespace MyTools
{
    public static class Setup 
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            Folders.CreateDefault("_Project", 
                "Animations",
                "Art",
                "Audio",
                "Materials",
                "Models",
                "Prefabs",
                "Resources",
                "Scenes",
                "Scripts",
                "Shaders",
                "Sprites",
                "Textures",
                "UI",
                "Videos");
            Refresh();
        }

        private static class Folders
        {
            public static void CreateDefault(string root, params string[] folders)
            {
                var fullPath = Combine(Application.dataPath, root);
                foreach (var folder in folders)
                {
                    var path = Combine(fullPath, folder);
                    if (!Exists(path))
                    {
                        CreateDirectory(path);
                    }
                }
            }
        }
    }
}
