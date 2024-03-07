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

        public static Rect GetScreenRect(RectTransform rectTransform, Vector2 fixed_resolution = default(Vector2))
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                Debug.Assert(false, "Transform이 소속된 캔버스를 찾을 수 없습니다");
                return Rect.zero;
            }

            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Camera camera = null; // ScreenSpaceOverlay의 경우 null을 그대로 사용
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                camera = canvas.worldCamera; // ScreenSpaceCamera의 경우 캔버스에 설정된 카메라 사용
            }

            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = RectTransformUtility.WorldToScreenPoint(camera, corners[i]);
            }

            // Corners를 사용해 스크린에서의 Rect 정보 계산
            Vector2 rectPositionOnScreen = corners[0]; // 하단 왼쪽 코너
            Vector2 rectSizeOnScreen = corners[2] - corners[0]; // 오른쪽 상단 코너에서 하단 왼쪽 코너를 뺀 값

            if (!fixed_resolution.Equals(default))
            {
                float multiple_width = fixed_resolution.x / Screen.width;
                float modify_height = Screen.height / multiple_width;

                rectPositionOnScreen = new Vector2(
                    rectPositionOnScreen.x / Screen.width * fixed_resolution.x,
                    (rectPositionOnScreen.y - (Screen.height - modify_height) / 2) / Screen.height * fixed_resolution.y
                    );

                rectSizeOnScreen = new Vector2(
                    rectSizeOnScreen.x / Screen.width * fixed_resolution.x,
                    rectSizeOnScreen.y * multiple_width
                    );
            }

            return new Rect(rectPositionOnScreen.x, rectPositionOnScreen.y, rectSizeOnScreen.x, rectSizeOnScreen.y);
        }
    }
}
