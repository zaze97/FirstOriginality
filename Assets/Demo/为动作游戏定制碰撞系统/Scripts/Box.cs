using UnityEngine;

namespace ACTBook
{
    public class Box : MonoBehaviour
    {
        public Vector3 center;//中心
        public Vector3 size;//大小
        public Vector3 Extents { get { return size * 0.5f; } }//一半的大小
        public Vector3 P0 { get { return transform.TransformPoint(center + new Vector3(-Extents.x, -Extents.y, -Extents.z)); } }//点0
        public Vector3 P1 { get { return transform.TransformPoint(center + new Vector3(Extents.x, -Extents.y, -Extents.z)); } }//点1
        public Vector3 P2 { get { return transform.TransformPoint(center + new Vector3(Extents.x, Extents.y, -Extents.z)); } }//点2
        public Vector3 P3 { get { return transform.TransformPoint(center + new Vector3(-Extents.x, Extents.y, -Extents.z)); } }//点3
        public Vector3 P4 { get { return transform.TransformPoint(center + new Vector3(-Extents.x, -Extents.y, Extents.z)); } }//点4
        public Vector3 P5 { get { return transform.TransformPoint(center + new Vector3(Extents.x, -Extents.y, Extents.z)); } }//点5
        public Vector3 P6 { get { return transform.TransformPoint(center + new Vector3(Extents.x, Extents.y, Extents.z)); } }//点6
        public Vector3 P7 { get { return transform.TransformPoint(center + new Vector3(-Extents.x, Extents.y, Extents.z)); } }//点7
        public Vector3 XAxis { get { return transform.rotation * Vector3.right; } }//变换后的right方向
        public Vector3 YAxis { get { return transform.rotation * Vector3.up; } }//变换后的up方向
        public Vector3 ZAxis { get { return transform.rotation * Vector3.forward; } }//变换后的forward方向

        void OnDrawGizmos()
        {
            var cacheMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(center, size);
            Gizmos.matrix = cacheMatrix;
        }//由于需要节省书中代码长度，Gizmos的逻辑在书中并未出现
    }
}
