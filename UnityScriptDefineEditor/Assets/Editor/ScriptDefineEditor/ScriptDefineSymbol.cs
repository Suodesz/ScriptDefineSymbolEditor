using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Linq;
using Suodesz.ScriptDefineEditor.Entities;
  
namespace Suodesz.ScriptDefineEditor
{
    public class ScriptDefineSymbol
    {
        public event Action<bool> OnNotifyChanged;
        public List<string> Defines { get { return defines; } }
        List<string> defines;
        BuildTarget buildTarget;
        string separator = ";";

        public ScriptDefineSymbol(BuildTarget buildTarget)
        {
            Load(buildTarget);
        }

        public void Load(BuildTarget buildTarget)
        {
            this.buildTarget = buildTarget;
            var deinfeStr = GetScriptingDefineSymbol(buildTarget);
            defines = deinfeStr.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Where(value => !string.IsNullOrEmpty(value))
                .ToList();
        }

        public void Save()
        {
            SetScriptingDefineSymbol();
        }

        public void Apply(IEnumerable<Define> addedDefines)
        {
            var currentDeines = defines;
            bool isChanged = false;
            foreach (var addDefine in addedDefines)
            {
                bool isExists = currentDeines.Contains(addDefine.Name);
                if (isExists == false && addDefine.IsEnable)
                {
                    currentDeines.Add(addDefine.Name);
                    isChanged = true;
                }
                else if (isExists && addDefine.IsEnable == false)
                {
                    currentDeines.Remove(addDefine.Name);
                    isChanged = true;
                }
            }

            if (OnNotifyChanged != null)
            {
                OnNotifyChanged(isChanged);
            }
        }

        void SetScriptingDefineSymbol()
        {
            string defineStr = string.Join(separator, defines.ToArray());
            var group = GetBuildTargetGroup(buildTarget);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, defineStr);
        }

        string GetScriptingDefineSymbol(BuildTarget target)
        {
            var group = GetBuildTargetGroup(target);
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        }

        BuildTargetGroup GetBuildTargetGroup(BuildTarget buildTarget)
        {
            return BuildPipeline.GetBuildTargetGroup(buildTarget);
        }
    }
}
