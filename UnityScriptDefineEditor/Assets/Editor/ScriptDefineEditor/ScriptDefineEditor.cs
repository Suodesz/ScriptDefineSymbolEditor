using UnityEditor;

namespace Suodesz.ScriptDefineEditor
{
    public class ScriptDefineEditor : EditorWindow
    {
        [MenuItem("Window/Script Define Editor %#w")]
        static void CreatMenu()
        {
            var editor = EditorWindow.GetWindow(typeof(ScriptDefineEditor), true, "Define Editor");
            editor.Show();
        }
    }
}