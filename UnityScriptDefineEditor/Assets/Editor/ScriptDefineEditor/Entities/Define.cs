using System;

namespace Suodesz.ScriptDefineEditor.Entities
{
    [Serializable]
    public class Define
    {
        public string Name;
        public string Description;
        public bool IsEnable { get; set; }

        public Define()
        {
            Description = "DESCRIPTION";
            Name = "NAME";
            IsEnable = false;
        }

        public Define(string name, string description)
        {
            Name = name;
            Description = description;
            IsEnable = false;
        }
    }
}