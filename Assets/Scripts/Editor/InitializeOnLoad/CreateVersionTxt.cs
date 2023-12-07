#if UNITY_EDITOR
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace FluffyDuck.EditorUtil
{
    [InitializeOnLoad]
    public class CreateVersionTxt
    {
        static CreateVersionTxt()
        {
            if (!BuildLauncher.ExistVersionText())
            {
                BuildLauncher.CreateVersionText();
                _ = BuildLauncher.ModifyVersionTextMeta();
            }
        }
    }
}
#endif
