using UnityEngine;
using System.Collections.Generic;

namespace Suodesz.ScriptDefineEditor.Entities
{
    [CreateAssetMenu(menuName = "Tools/Define")]
    public class DefineSymbolAsset : ScriptableObject
    {
        public List<DefineSymbol> defines;
    }
}