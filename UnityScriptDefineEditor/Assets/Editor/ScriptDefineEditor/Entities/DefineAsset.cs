using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Suodesz.ScriptDefineEditor.Entities
{
    [CreateAssetMenu(menuName = "Tools/Define")]
    public class DefineAsset: ScriptableObject
    {
        public List<Define> defines;
    }
}