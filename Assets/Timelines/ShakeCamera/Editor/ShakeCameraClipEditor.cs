using UnityEditor;
using UnityEngine;

/// <summary>
/// ShakeCameraClip 인스펙터를 편집하기 위한 에디터
/// </summary>
[CustomEditor(typeof(ShakeCameraClip))]
public class ShakeCameraClipEditor : Editor
{
    // 인스펙터에서 보여주기위한 패턴이름
    // 0 : Vertical == true && Horizontal == true
    // 1 : Vertical == true && Horizontal == false
    // 2 : Vertical == false && Horizontal == true
    static readonly string[] s_ShakePattern = { "All Directions", "Vertical", "Horizontal" };

    // ShakeCameraClip에 SerializeField로 선언되어 있는 값을 갖고 있기 위한 Fields
    SerializedProperty _Magnitude = null;
    SerializedProperty _Roughness = null;
    SerializedProperty _Vertical = null;
    SerializedProperty _Horizontal = null;

    #region Editor Methods
    /// <summary>
    /// 클립 인스펙터가 화면에 떴을때 호출
    /// </summary>
    void OnEnable()
    {
        // 사용할 값들을 저장합니다.
        SerializedProperty _Template = serializedObject.FindProperty(nameof(_Template));
        _Magnitude = _Template.FindPropertyRelative(nameof(_Magnitude));
        _Roughness = _Template.FindPropertyRelative(nameof(_Roughness));
        _Vertical = _Template.FindPropertyRelative(nameof(_Vertical));
        _Horizontal = _Template.FindPropertyRelative(nameof(_Horizontal));

        // 어느방향으로도 움직이지 않는 이상한 값은 수정펀치다.
        if (!_Vertical.boolValue && !_Horizontal.boolValue)
        {
            _Vertical.boolValue = true;
            _Horizontal.boolValue = true;
            serializedObject.ApplyModifiedProperties();
        }
    }

    /// <summary>
    /// 클립 인스펙터 뷰가 업데이트 될때마다 호출됩니다.
    /// 여기에 인스펙터에 표시되어야 할 컴포넌트들을 배치하면 됩니다.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // serializedObject에 있는 실제 값을 가져옵니다.
        serializedObject.Update();

        EditorGUILayout.Slider(_Magnitude, 1, 100);
        EditorGUILayout.Slider(_Roughness, 1, 100);

        int patternIndex = GetPatternIndex(_Vertical.boolValue, _Horizontal.boolValue);
        patternIndex = EditorGUILayout.Popup(new GUIContent("Pattern", "흔들리는 방향"), patternIndex, s_ShakePattern);
        SetPatternFromIndex(_Vertical, _Horizontal, patternIndex);

        // SerializedProperty들에 넣어준 값을 실제로 적용합니다. 
        serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Shake Pattern 인덱스 값으로 _Vertical과 _Horizontal에 bool 값을 입력
    /// </summary>
    /// <param name="_Vertical"></param>
    /// <param name="_Horizontal"></param>
    /// <param name="index"></param>
    void SetPatternFromIndex(SerializedProperty _Vertical, SerializedProperty _Horizontal, int index)
    {
        _Vertical.boolValue = (index < 2) ? true : false;
        _Horizontal.boolValue = (index == 1) ? false : true;
    }

    /// <summary>
    /// 세로 가로의 두개의 방향 bool 값으로 Shake Pattern 인덱스 값을 가져온다
    /// </summary>
    /// <param name="isVertical"></param>
    /// <param name="isHorizontal"></param>
    /// <returns></returns>
    int GetPatternIndex(bool isVertical, bool isHorizontal)
    {
        int index = 0;

        if (!(isVertical && isHorizontal)) // 둘다 false인 경우가 없으니 둘다 true인 케이스(== All Directions)만 걸러내면
        {
            index++; // 세로로만 흔들리거나 == Vertical만 true == 1
            if (isHorizontal)
            {
                index++; // 가로로만 흔들리거나 == Horizontal만 true == 2
            }
        }

        return index;
    }
    #endregion
}
