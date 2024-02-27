using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectComponent))]
[CanEditMultipleObjects]
public class EffectComponent_Inspector : Editor
{
    EffectComponent EC;
    //  모든 프로퍼티 보기
    SerializedProperty Is_Show_All_Properties_Property;

    //  이펙트 타입
    SerializedProperty Effect_Type_Property;
    SerializedProperty Effect_Duration_Property;
    SerializedProperty Is_Loop_Property;
    SerializedProperty Delay_Time_Property;

    //  투사체 관련 정의
    SerializedProperty Projectile_Start_Pos_Type_Property;
    SerializedProperty Projectile_Reach_Pos_Type_Property;
    SerializedProperty Throwing_Type_Property;

    //  Curve
    SerializedProperty Curve_Property;
    SerializedProperty Start_Curve_Dist_Property;
    SerializedProperty End_Curve_Dist_Property;

    //  Parabola
    SerializedProperty Parabola_Property;
    SerializedProperty Parabola_Height_Property;

    //  Mover
    SerializedProperty Mover_Property;

    //  Velocity
    SerializedProperty Projectile_Velocity_Property;

    //  Hide Objects
    SerializedProperty Use_Hide_Transforms_Property;
    SerializedProperty Hide_Transforms_Property;
    SerializedProperty Hide_After_Delay_Time_Property;


    private void OnEnable()
    {
        EC = (EffectComponent)target;

        //  모든 프로퍼티 보기(Private)
        Is_Show_All_Properties_Property = serializedObject.FindProperty("Is_Show_All_Properties");

        //  스킬 이펙트 타입
        Effect_Type_Property = serializedObject.FindProperty("Effect_Type");
        Effect_Duration_Property = serializedObject.FindProperty("Effect_Duration");
        Is_Loop_Property = serializedObject.FindProperty("Is_Loop");
        Delay_Time_Property = serializedObject.FindProperty("Delay_Time");

        //  투사체 관련 정의
        Projectile_Start_Pos_Type_Property = serializedObject.FindProperty("Projectile_Start_Pos_Type");
        Projectile_Reach_Pos_Type_Property = serializedObject.FindProperty("Projectile_Reach_Pos_Type");
        Throwing_Type_Property = serializedObject.FindProperty("Throwing_Type");

        //  curve
        Curve_Property = serializedObject.FindProperty("Curve");
        Start_Curve_Dist_Property = serializedObject.FindProperty("Start_Curve_Dist");
        End_Curve_Dist_Property = serializedObject.FindProperty("End_Curve_Dist");

        //  포물선
        Parabola_Property = serializedObject.FindProperty("Parabola");
        Parabola_Height_Property = serializedObject.FindProperty("Parabola_Height");

        //  직선
        Mover_Property = serializedObject.FindProperty("Mover");

        //  이동 속도
        Projectile_Velocity_Property = serializedObject.FindProperty("Projectile_Velocity");

        //  투사체 이동 종료 후 오브젝트 감추기
        Use_Hide_Transforms_Property = serializedObject.FindProperty("Use_Hide_Transforms");
        Hide_Transforms_Property = serializedObject.FindProperty("Hide_Transforms");
        Hide_After_Delay_Time_Property = serializedObject.FindProperty("Hide_After_Delay_Time");
    }

    

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //base.OnInspectorGUI();      //  기본 프로퍼티를 보이지 않게 하기위해서는 구현하지 말아줘야 함(마지막에 처리)

        EditorGUILayout.BeginVertical("모든 프로퍼티 보기");
        EditorGUILayout.PropertyField(Is_Show_All_Properties_Property);
        EditorGUILayout.EndVertical();

        if (Is_Show_All_Properties_Property.boolValue)
        {
            LayoutAllPropertiesGUI();
        }
        else
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.BeginVertical("스킬 이펙트 타입 선택");

            EditorGUILayout.PropertyField(Effect_Type_Property);
            SKILL_EFFECT_TYPE skill_effect_type = (SKILL_EFFECT_TYPE)Effect_Type_Property.enumValueIndex;

            switch (skill_effect_type)
            {
                case SKILL_EFFECT_TYPE.NONE:
                    break;
                case SKILL_EFFECT_TYPE.CASTING:
                    EditorGUILayout.PropertyField(Effect_Duration_Property);
                    LayoutReachTargetPosTypeGUI();
                    break;
                case SKILL_EFFECT_TYPE.PROJECTILE:
                    LayoutProjectileGUI();
                    break;
                case SKILL_EFFECT_TYPE.IMMEDIATE:
                    EditorGUILayout.PropertyField(Is_Loop_Property);
                    EditorGUILayout.PropertyField(Delay_Time_Property);
                    if (!Is_Loop_Property.boolValue)
                    {
                        EditorGUILayout.PropertyField(Effect_Duration_Property);
                    }
                    LayoutReachTargetPosTypeGUI();
                    break;
            }

            LayoutResetButtonGUI();
            EditorGUILayout.EndVertical();
        }
        

        serializedObject.ApplyModifiedProperties();

    }

    void LayoutAllPropertiesGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.PropertyField(Effect_Type_Property);
        EditorGUILayout.PropertyField(Effect_Duration_Property);
        EditorGUILayout.PropertyField(Is_Loop_Property);
        EditorGUILayout.PropertyField(Delay_Time_Property);

        //  투사체 관련 정의
        EditorGUILayout.PropertyField(Projectile_Start_Pos_Type_Property);
        EditorGUILayout.PropertyField(Projectile_Reach_Pos_Type_Property);
        EditorGUILayout.PropertyField(Throwing_Type_Property);


        //  Curve
        EditorGUILayout.PropertyField(Curve_Property);
        EditorGUILayout.PropertyField(Start_Curve_Dist_Property);
        EditorGUILayout.PropertyField(End_Curve_Dist_Property);

        //  Parabola
        EditorGUILayout.PropertyField(Parabola_Property);
        EditorGUILayout.PropertyField(Parabola_Height_Property);

        //  Mover
        EditorGUILayout.PropertyField(Mover_Property);

        //  Velocity
        EditorGUILayout.PropertyField(Projectile_Velocity_Property);

        //  Hide Objects
        EditorGUILayout.PropertyField(Use_Hide_Transforms_Property);
        EditorGUILayout.PropertyField(Hide_Transforms_Property);
        EditorGUILayout.PropertyField(Hide_After_Delay_Time_Property);

        LayoutResetButtonGUI();
        EditorGUILayout.EndVertical();
    }

    void LayoutResetButtonGUI()
    {
        GUILayout.Space(30);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("데이터 초기화", GUILayout.Width(200), GUILayout.Height(40)))
        {
            Effect_Type_Property.enumValueIndex = 0;
            Effect_Duration_Property.floatValue = 0f;
            Is_Loop_Property.boolValue = false;
            Delay_Time_Property.floatValue = 0f;
            Projectile_Start_Pos_Type_Property.enumValueIndex = 0;
            Projectile_Reach_Pos_Type_Property.enumValueIndex = 0;
            Throwing_Type_Property.enumValueIndex = 0;
            
            Curve_Property.objectReferenceValue = null;
            Start_Curve_Dist_Property.floatValue = 0f;
            End_Curve_Dist_Property.floatValue = 0f;

            Parabola_Property.objectReferenceValue = null;
            Parabola_Height_Property.floatValue = 0f;

            Mover_Property.objectReferenceValue = null;

            Projectile_Velocity_Property.floatValue = 0f;

            Hide_Transforms_Property.ClearArray();
            Hide_After_Delay_Time_Property.floatValue = 0f;

        }
        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

    void LayoutReachTargetPosTypeGUI()
    {
        EditorGUILayout.PropertyField(Projectile_Reach_Pos_Type_Property);
    }

    /// <summary>
    /// 이동 종료 후 이펙트는 일부 사라지고<br/>
    /// 잠시 딜레이 후 실제 효과를 적용하기 위한 변수들
    /// </summary>
    void LayoutHideTransformsGUI()
    {
        GUILayout.Space(10);

        EditorGUILayout.BeginVertical("투사체 이동 후 오브젝트 감추기");
        {
            EditorGUILayout.PropertyField(Use_Hide_Transforms_Property);
            if (Use_Hide_Transforms_Property.boolValue)
            {
                EditorGUILayout.PropertyField(Hide_Transforms_Property);
                EditorGUILayout.PropertyField(Hide_After_Delay_Time_Property);
            }
        }
        EditorGUILayout.EndVertical();
    }

    /// <summary>
    /// 투사체 관련 상세 프로퍼티 
    /// </summary>
    void LayoutProjectileGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginVertical("투사체 관련 정의");
        
        THROWING_TYPE throwing_type = (THROWING_TYPE)Throwing_Type_Property.enumValueIndex;
        {
            EditorGUILayout.PropertyField(Projectile_Start_Pos_Type_Property);
            EditorGUILayout.PropertyField(Projectile_Reach_Pos_Type_Property);
            EditorGUILayout.PropertyField(Throwing_Type_Property);

            //  throwgin_type
            
            switch (throwing_type)
            {
                case THROWING_TYPE.NONE:
                    break;
                case THROWING_TYPE.LINEAR:
                    EditorGUILayout.PropertyField(Mover_Property);
                    EditorGUILayout.PropertyField(Projectile_Velocity_Property);
                    break;
                case THROWING_TYPE.PARABOLA:
                    EditorGUILayout.PropertyField(Parabola_Property);
                    EditorGUILayout.PropertyField(Parabola_Height_Property);
                    EditorGUILayout.PropertyField(Projectile_Velocity_Property);
                    break;
                case THROWING_TYPE.BEZIER:
                    EditorGUILayout.PropertyField(Curve_Property);
                    EditorGUILayout.PropertyField(Start_Curve_Dist_Property);
                    EditorGUILayout.PropertyField(End_Curve_Dist_Property);
                    EditorGUILayout.PropertyField(Projectile_Velocity_Property);
                    break;
            }
        }

        EditorGUILayout.EndVertical();

        if (throwing_type != THROWING_TYPE.NONE)
        {
            LayoutHideTransformsGUI();
        }
        
    }
}
