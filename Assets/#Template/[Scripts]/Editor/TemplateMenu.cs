using UnityEngine;
using UnityEditor;

namespace DancingLineFanmade.Editor
{
    public static class TemplateMenu
    {
        [MenuItem("Template/Tutorial")]
        private static void OpenTutorial()
        {
            Application.OpenURL("https://maxiceflame.github.io/tutorial");
        }
    }
}