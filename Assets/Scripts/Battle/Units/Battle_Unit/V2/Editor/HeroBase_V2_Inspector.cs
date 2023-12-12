using Spine;
using Spine.Unity;
using Spine.Unity.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(HeroBase_V2), true)]
[CanEditMultipleObjects]
public class HeroBase_V2_Inspector : Editor
{
    readonly GUIContent FindBonePositionButtonLabel = new GUIContent("Find SD Pos Bones", Spine.Unity.Editor.SpineEditorUtilities.Icons.skeleton);

    HeroBase_V2 Hero;
    SerializedProperty Sd_Body_Transforms_Property;


    private void OnEnable()
    {
        Hero = (HeroBase_V2)target;
        Sd_Body_Transforms_Property = serializedObject.FindProperty("Sd_Body_Transforms");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();


        //  button click
        if (SpineInspectorUtility.LargeCenteredButton(FindBonePositionButtonLabel))
        {
            List<string> find_bone_name = new List<string>();
            find_bone_name.Add("root");
            find_bone_name.Add("center");
            find_bone_name.Add("face");

            List<SD_Body_Pos_Data> find_result = new List<SD_Body_Pos_Data>();

            var bones = Hero.GetComponentsInChildren<SkeletonUtilityBone>();

            foreach (var utility_bone in bones)
            {
                SkeletonUtility skeleton_utility = utility_bone.hierarchy;
                if (!utility_bone.valid && skeleton_utility != null)
                {
                    if (skeleton_utility.skeletonRenderer != null)
                        skeleton_utility.skeletonRenderer.Initialize(false);
                    if (skeleton_utility.skeletonGraphic != null)
                        skeleton_utility.skeletonGraphic.Initialize(false);
                }
                if (utility_bone.bone == null)
                {
                    continue;
                }

                if (find_bone_name.Exists(x => x.Equals(utility_bone.boneName)))
                {
                    var sd = new SD_Body_Pos_Data();
                    if (utility_bone.boneName.Equals("center"))
                    {
                        sd.Body_Type = SD_BODY_TYPE.BODY;
                    }
                    else if (utility_bone.boneName.Equals("root"))
                    {
                        sd.Body_Type = SD_BODY_TYPE.FOOT;
                    }
                    else if (utility_bone.boneName.Equals("face"))
                    {
                        sd.Body_Type = SD_BODY_TYPE.HEAD;
                    }
                    sd.Trans = utility_bone.transform;
                    find_result.Add(sd);
                }
            }


            Sd_Body_Transforms_Property.arraySize = find_result.Count;

            for (int i = 0; i < Sd_Body_Transforms_Property.arraySize; i++)
            {
                var element = Sd_Body_Transforms_Property.GetArrayElementAtIndex(i);

                //  struct 필드 접근
                SerializedProperty body_type_property = element.FindPropertyRelative("Body_Type");
                SerializedProperty trans_property = element.FindPropertyRelative("Trans");

                var sd = find_result[i];

                //  enum
                body_type_property.enumValueIndex = Array.IndexOf(Enum.GetNames(typeof(SD_BODY_TYPE)), sd.Body_Type.ToString());
                //  transform
                trans_property.objectReferenceValue = sd.Trans;
            }

            AssetDatabase.SaveAssets();

        }

        serializedObject.ApplyModifiedProperties();
    }

    
}
