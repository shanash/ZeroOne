#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FluffyDuck.EditorUtil
{
    [InitializeOnLoad]
    public class CreateVersionTxt
    {
        static CreateVersionTxt()
        {
            BuildLauncher.CreateVersionText();
        }
    }
}
#endif
