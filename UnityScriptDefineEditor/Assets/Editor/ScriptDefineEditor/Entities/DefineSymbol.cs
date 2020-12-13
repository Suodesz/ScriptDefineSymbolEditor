using System;

namespace Suodesz.ScriptDefineEditor.Entities
{
    [Serializable]
    public class DefineSymbol
    {
        public string Name;
        public string Description;
        public bool IsEnable { get; set; }

        public DefineSymbol()
        {
            Description = "DESCRIPTION";
            Name = "NAME";
            IsEnable = false;
        }

        public DefineSymbol(string name, string description)
        {
            Name = name;
            Description = description;
            IsEnable = false;
        }
    }
}