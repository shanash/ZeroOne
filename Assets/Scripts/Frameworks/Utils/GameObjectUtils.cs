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
    }
}
