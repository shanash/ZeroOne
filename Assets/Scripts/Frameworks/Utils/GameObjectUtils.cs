using UnityEngine;

namespace FluffyDuck.Util
{
    public static class GameObjectUtils
    {
        /// <summary>
        /// 현재 Transform 연결되어 있는 오브젝트 및 자식 오브젝트들의 레이어를 변경합니다
        /// </summary>
        /// <param name="parent">부모 Transform</param>
        /// <param name="layerName">레이어 이름</param>
        public static void ChangeLayersRecursively(Transform parent, string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);

            if (layer == -1)
            {
                Debug.LogError("Layer not found: " + layerName);
                return;
            }

            SetLayerRecursively(parent, layer);
        }

        static void SetLayerRecursively(Transform trans, int layer)
        {
            trans.gameObject.layer = layer;
            foreach (Transform child in trans)
            {
                SetLayerRecursively(child, layer);
            }
        }

        public static Rect GetScreenRect(RectTransform rectTransform)
        {
            // RectTransform의 스크린에서의 위치와 크기를 얻기 위한 Corners 계산
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
            }

            // Corners를 사용해 스크린에서의 Rect 정보 계산
            Vector2 rectPositionOnScreen = corners[0]; // 하단 왼쪽 코너
            Vector2 rectSizeOnScreen = corners[2] - corners[0]; // 오른쪽 상단 코너에서 하단 왼쪽 코너를 뺀 값

            return new Rect(rectPositionOnScreen.x, rectPositionOnScreen.y, rectSizeOnScreen.x, rectSizeOnScreen.y);
        }
    }
}
