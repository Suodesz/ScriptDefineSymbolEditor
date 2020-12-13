using System.Collections.Generic;
using System.IO;
using Suodesz.ScriptDefineEditor.Entities;
using UnityEditor;
using UnityEngine;

namespace Suodesz.ScriptDefineEditor
{
    public class ScriptDefineSymbolEditor : EditorWindow
    {  
        private List<DefineSymbol> defines;
        private bool waitCompile;
        private ScriptDefineSymbol targetDefineSymbol;
        private BuildTarget currentBuildTarget;
        [MenuItem("Window/Script Define Editor %#w")]
        static void CreatMenu()
        {
            string windowName = "Scripting Define Editor";
            var editor = EditorWindow.GetWindow(typeof(ScriptDefineSymbolEditor), true, windowName) as ScriptDefineSymbolEditor;
            editor.Show();
        }

        private void OnGUI()
        {
            LoadTargetDefine();
            LoadCustomDefine();

            if(defines.Count == 0)
            {
                EditorGUILayout.LabelField("Custom Defines is empty");
                //if (GUILayout.Button("Show Custom Define Asset Position"))
                //{
                    
                //}
                return;
            }

            if (waitCompile && EditorApplication.isCompiling == false)
            {
                CompileCompletely();
            }

            foreach (var group in defines)
            {
                BuildContents(group);
            }

            if(waitCompile == false)
            { 
                if (GUILayout.Button("Apply"))
                {
                    targetDefineSymbol.Apply(defines);
                    RefreshDefineStatus();
                }
            }
        }

        void LoadCustomDefine()
        {
            if (defines == null)
            {
                string defineAssetPath = GetDefineAssetPath();
                var assets = (DefineSymbolAsset)AssetDatabase.LoadAssetAtPath(defineAssetPath, typeof(DefineSymbolAsset));
                defines = assets.defines;
                RefreshDefineStatus();
            }
        }

        string GetDefineAssetPath()
        {
            var script = MonoScript.FromScriptableObject(this);
            var scriptPath = AssetDatabase.GetAssetPath(script);
            var editorRootPath = Path.GetDirectoryName(scriptPath);
            var dir = Path.Combine(editorRootPath, "Entities");
            var defineAssetPath = Path.Combine(dir, "CustomDefine.asset");
            return defineAssetPath;
        }

        void LoadTargetDefine()
        {
            currentBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (targetDefineSymbol == null)
            {
                targetDefineSymbol = new ScriptDefineSymbol(currentBuildTarget);
                targetDefineSymbol.OnNotifyChanged += OnDefineChanged;
            }
        }

        void BuildContents(DefineSymbol group)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Name");
                    EditorGUILayout.TextField(group.Name);
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Description");
                    EditorGUILayout.TextField(group.Description);
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Enable");
                    group.IsEnable = EditorGUILayout.ToggleLeft("", group.IsEnable);
                }

                EditorGUILayout.Space();
            }
        }

        void CompileCompletely()
        {
            waitCompile = false;
            RemoveNotification();
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("Complete", string.Format("Change to \n{0}\n Completely", GetDefineHint()), "Close");
        }

        void RefreshDefineStatus()
        {
            var currentDeines = targetDefineSymbol.Defines;
            foreach (var define in defines)
            {
                define.IsEnable = currentDeines.Contains(define.Name);
            }
        }

        void OnDefineChanged(bool isChanged)
        {
            if(isChanged)
            {
                if (EditorUtility.DisplayDialog("Change Define?", "Define : \n" + GetDefineHint(), "OK", "Cancel"))
                {
                    targetDefineSymbol.Save();
                    waitCompile = true;
                    ShowNotification(new GUIContent("Compiling"));
                }
                else
                {
                    targetDefineSymbol.Load(currentBuildTarget);
                }
            }
        }

        string GetDefineHint()
        {
            return string.Join(", ", targetDefineSymbol.Defines.ToArray());
        }
    }
}