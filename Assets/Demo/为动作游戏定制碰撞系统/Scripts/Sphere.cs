using UnityEngine;

namespace ACTBook
{
    public class Sphere : MonoBehaviour
    {
        public Vector3 center;//中心偏移
        public float radius;//半径
        public Vector3 WorldCenter { get { return transform.TransformPoint(center); } }//中心世界坐标
        public float ConcertRadius
        {
            get
            {
                var lossyScale = transform.lossyScale;
                var maxScale = Mathf.Max(Mathf.Max(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y)), Mathf.Abs(lossyScale.z));

                return maxScale * radius;
            }
        }//注意，出于篇幅原有，此处与书中内容不一致。书中的版本可以理解为不含缩放的球对象。

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(WorldCenter, ConcertRadius);
        }
    }
}