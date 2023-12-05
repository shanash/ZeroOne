using JetBrains.Annotations;
using Spine;
using Spine.Unity;
using Spine.Unity.Editor;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

[CustomEditor(typeof(ActressBase), true)]
[CanEditMultipleObjects]
public class ActressBaseInspector : Editor
{
    readonly GUIContent SpawnHierarchyButtonLabel = new GUIContent("Add BoundingBoxes", Spine.Unity.Editor.SpineEditorUtilities.Icons.skeleton);

    ActressBase _Actress;
    SkeletonUtility _Skeleton_Utility;
    Dictionary<SkeletonUtilityBone, Dictionary<Slot, List<Attachment>>> _Bone_Bounding_Box_Table = new Dictionary<SkeletonUtilityBone, Dictionary<Slot, List<Attachment>>>();

    void OnEnable()
    {
        _Actress = (ActressBase)target;
        _Skeleton_Utility = _Actress.gameObject.GetComponent<SkeletonUtility>();

        var bones = _Actress.GetComponentsInChildren<SkeletonUtilityBone>();
        _Bone_Bounding_Box_Table.Clear();

        foreach (var utilityBone in bones)
        {
            var skeletonUtility = utilityBone.hierarchy;

            if (!utilityBone.valid && skeletonUtility != null)
            {
                if (skeletonUtility.skeletonRenderer != null)
                    skeletonUtility.skeletonRenderer.Initialize(false);
                if (skeletonUtility.skeletonGraphic != null)
                    skeletonUtility.skeletonGraphic.Initialize(false);
            }

            if (utilityBone.bone == null) continue;

            Skeleton skeleton = utilityBone.bone.Skeleton;
            int slotCount = skeleton.Slots.Count;
            Skin skin = skeleton.Skin;
            if (skeleton.Skin == null)
                skin = skeleton.Data.DefaultSkin;

            Dictionary<Slot, List<Attachment>> boundingBoxTable = new Dictionary<Slot, List<Attachment>>();

            for (int i = 0; i < slotCount; i++)
            {
                Slot slot = skeletonUtility.Skeleton.Slots.Items[i];
                if (slot.Bone == utilityBone.bone)
                {
                    List<Skin.SkinEntry> slotAttachments = new List<Skin.SkinEntry>();
                    int slotIndex = skeleton.Data.FindSlot(slot.Data.Name).Index;
                    skin.GetAttachments(slotIndex, slotAttachments);

                    List<Attachment> boundingBoxes = new List<Attachment>();
                    foreach (Skin.SkinEntry entry in slotAttachments)
                    {
                        switch (entry.Attachment)
                        {
                            case BoundingBoxAttachment boundingBoxAttachment:
                                boundingBoxes.Add(boundingBoxAttachment);
                                break;
                            case PointAttachment pointAttachment:
                                boundingBoxes.Add(pointAttachment);
                                break;
                            default:
                                break;
                        }
                    }

                    if (boundingBoxes.Count > 0)
                        boundingBoxTable.Add(slot, boundingBoxes);
                }
            }
            _Bone_Bounding_Box_Table.Add(utilityBone, boundingBoxTable);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        if (SpineInspectorUtility.LargeCenteredButton(SpawnHierarchyButtonLabel))
        {
            SkeletonAnimation skel = _Actress.GetComponent<SkeletonAnimation>();

            foreach (var bounding_box_table in _Bone_Bounding_Box_Table)
            {
                var utilityBone = bounding_box_table.Key;
                foreach (KeyValuePair<Slot, List<Attachment>> entry in bounding_box_table.Value)
                {
                    Slot slot = entry.Key;
                    List<Attachment> boundingBoxes = entry.Value;

                    foreach (Attachment attachment in boundingBoxes)
                    {
                        utilityBone.bone.Skeleton.UpdateWorldTransform(Skeleton.Physics.Update);

                        switch (attachment)
                        {
                            case BoundingBoxAttachment box:
                                Transform bbTransform = utilityBone.transform.Find("[BoundingBox]" + box.Name); // Use FindChild in older versions of Unity.
                                if (bbTransform != null)
                                {
                                    PolygonCollider2D originalCollider = bbTransform.GetComponent<PolygonCollider2D>();
                                    if (originalCollider != null)
                                        SkeletonUtility.SetColliderPointsLocal(originalCollider, slot, box);
                                    else
                                        SkeletonUtility.AddBoundingBoxAsComponent(box, slot, bbTransform.gameObject);
                                }
                                else
                                {
                                    PolygonCollider2D newPolygonCollider = SkeletonUtility.AddBoundingBoxGameObject(null, box, slot, utilityBone.transform);
                                    bbTransform = newPolygonCollider.transform;
                                }
                                
                                var spine_bounding_box = bbTransform.GetComponent<SpineBoundingBox>();
                                if (spine_bounding_box == null)
                                {
                                    spine_bounding_box = bbTransform.AddComponent<SpineBoundingBox>();
                                }

                                string name = box.Name.Replace("bd_", "");

                                spine_bounding_box.Set(_Actress, name);

                                int length = name.Length;
                                TOUCH_BODY_DIRECTION dir = TOUCH_BODY_DIRECTION.NONE;

                                if (name[length - 2] == '_')
                                {
                                    if (name[length - 1].Equals('R'))
                                    {
                                        dir = TOUCH_BODY_DIRECTION.R;
                                    }
                                    else if (name[length - 1].Equals('L'))
                                    {
                                        dir = TOUCH_BODY_DIRECTION.L;
                                    }
                                    Debug.Assert(dir != TOUCH_BODY_DIRECTION.NONE, $"뼈의 방향 입력이 잘못되었습니다 : box.Name : {box.Name}");

                                    name = name.Substring(0, length - 2);
                                }
                                else
                                {
                                    dir = TOUCH_BODY_DIRECTION.NONE;
                                }

                                string enum_name = name.ToUpper();
                                if (Enum.TryParse(enum_name, out TOUCH_BODY_TYPE result))
                                {
                                    spine_bounding_box.SetTouchBodyType(result, dir);
                                }
                                break;
                            case PointAttachment pt:
                                Transform ptTransform = utilityBone.transform.Find("[Point]" + pt.Name);

                                if (ptTransform != null)
                                {
                                    PointFollower pf = ptTransform.GetComponent<PointFollower>();
                                    pf.slotName = pt.Name;
                                    pf.pointAttachmentName = pt.Name;
                                    pf.skeletonRenderer = _Actress.GetComponent<SkeletonRenderer>();
                                }
                                else
                                {
                                    GameObject go = new GameObject("[Point]" + pt.Name);
                                    Transform tf = go.transform;
                                    tf.parent = utilityBone.transform;
                                    tf.localPosition = Vector3.zero;
                                    tf.localRotation = Quaternion.identity;
                                    tf.localScale = Vector3.one;

                                    PointFollower pf = go.AddComponent<PointFollower>();
                                    pf.slotName = pt.Name;
                                    pf.pointAttachmentName = pt.Name;
                                    pf.skeletonRenderer = skel;
                                }
                                break;
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
