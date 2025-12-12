using UnityEngine;
using UnityEditor;
using System.IO;

namespace DancingLineFanmade.Editor
{
    public class MaterialCreationProcessor : AssetModificationProcessor
    {
        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(".meta", "");
            if (Path.GetExtension(path) != ".mat")
                return;

            EditorApplication.delayCall += () =>
            {
                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material == null)
                {
                    Debug.LogError($"Target material is not exist. Path: {path}");
                    return;
                }

                const string shaderName = "Dancing Line Fanmade/Standard/Color";
                var shader = Shader.Find(shaderName);
                if (shader != null)
                {
                    material.shader = shader;
                    EditorUtility.SetDirty(material);
                    AssetDatabase.SaveAssets();
                }
                else Debug.LogError($"Target shader \"{shaderName}\" is not exist.");
            };
        }
    }
}