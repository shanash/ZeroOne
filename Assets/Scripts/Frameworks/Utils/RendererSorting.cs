using UnityEngine;
using UnityEngine.Rendering;


namespace FluffyDuck.Util
{
    public class RendererSorting : MonoBehaviour
    {

        SortingGroup Sort_Group;

        private void Start()
        {
            Sort_Group = GetComponent<SortingGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 pos = this.transform.position;
            pos.y *= -1;
            Sort_Group.sortingOrder = (int)pos.y;
        }
    }

}
