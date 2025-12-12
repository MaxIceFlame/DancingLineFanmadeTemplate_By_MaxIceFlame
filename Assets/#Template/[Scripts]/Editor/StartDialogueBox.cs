using UnityEngine;
using UnityEditor;

namespace DancingLineFanmade.Editor
{
    public static class StartDialogueBox
    {
        private const string Key = "TEMPLATE_PROJECT_INTIALIZED";
        private const string Website = "https://maxiceflame.github.io/tutorial";

        [InitializeOnLoadMethod]
        private static void OnProjectLoaded()
        {
            if (EditorPrefs.HasKey(Key))
                return;
            EditorPrefs.SetBool(Key, true);
            EditorApplication.delayCall += ShowConfirmationDialog;
        }

        private static void ShowConfirmationDialog()
        {
            EditorApplication.delayCall -= ShowConfirmationDialog;
            if (!EditorUtility.DisplayDialog("模板使用说明", "初次启动模板，是否打开模板使用手册？\n（如后续需查看模板使用手册，可点击菜单栏Template/Tutorial以打开。）",
                    "是", "否"))
                return;
            Application.OpenURL(Website);
        }
    }
}