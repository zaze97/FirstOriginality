using UnityEngine;

namespace ACTBook
{
    public class BoxVSBox_Test : MonoBehaviour
    {
        public Box a, b;//测试用box


        void Start()
        {
            Debug.Log("[Box vs Box]Is Instect: " + BoxVSBox(a, b));//打印是否相交
        }
        bool BoxVSBox(Box xBox, Box yBox)
        {
            var isNotIntersect = false;
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, xBox.XAxis);//xBox的x轴上是否未相交
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, xBox.YAxis);//xBox的y轴上是否未相交
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, xBox.ZAxis);//xBox的z轴上是否未相交
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, yBox.XAxis);//yBox的x轴上是否未相交
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, yBox.YAxis);//yBox的y轴上是否未相交
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, yBox.ZAxis);//yBox的z轴上是否未相交
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.XAxis, yBox.XAxis));//叉乘轴的检测
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.XAxis, yBox.YAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.XAxis, yBox.ZAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.YAxis, yBox.XAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.YAxis, yBox.YAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.YAxis, yBox.ZAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.ZAxis, yBox.XAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.ZAxis, yBox.YAxis));
            isNotIntersect |= ProjectionIsNotIntersect(xBox, yBox, Vector3.Cross(xBox.ZAxis, yBox.ZAxis));
            return isNotIntersect ? false : true;
        }
        //投影点是否没有相交
        bool ProjectionIsNotIntersect(Box xBox, Box yBox, Vector3 axis)
        {
            var x_p0 = GetProject_Fast(xBox.P0, axis);//xBox点0的投影点float值
            var x_p1 = GetProject_Fast(xBox.P1, axis);//xBox点1的投影点float值
            var x_p2 = GetProject_Fast(xBox.P2, axis);//xBox点2的投影点float值
            var x_p3 = GetProject_Fast(xBox.P3, axis);//xBox点3的投影点float值
            var x_p4 = GetProject_Fast(xBox.P4, axis);//xBox点4的投影点float值
            var x_p5 = GetProject_Fast(xBox.P5, axis);//xBox点5的投影点float值
            var x_p6 = GetProject_Fast(xBox.P6, axis);//xBox点6的投影点float值
            var x_p7 = GetProject_Fast(xBox.P7, axis);//xBox点7的投影点float值
            var y_p0 = GetProject_Fast(yBox.P0, axis);//yBox点0的投影点float值
            var y_p1 = GetProject_Fast(yBox.P1, axis);//yBox点1的投影点float值
            var y_p2 = GetProject_Fast(yBox.P2, axis);//yBox点2的投影点float值
            var y_p3 = GetProject_Fast(yBox.P3, axis);//yBox点3的投影点float值
            var y_p4 = GetProject_Fast(yBox.P4, axis);//yBox点4的投影点float值
            var y_p5 = GetProject_Fast(yBox.P5, axis);//yBox点5的投影点float值
            var y_p6 = GetProject_Fast(yBox.P6, axis);//yBox点6的投影点float值
            var y_p7 = GetProject_Fast(yBox.P7, axis);//yBox点7的投影点float值
            var xMin = Mathf.Min(x_p0, Mathf.Min(x_p1, Mathf.Min(x_p2, Mathf.Min(x_p3, Mathf.Min(x_p4, Mathf.Min(x_p5, Mathf.Min(x_p6, x_p7)))))));//xBox的最小投影值
            var xMax = Mathf.Max(x_p0, Mathf.Max(x_p1, Mathf.Max(x_p2, Mathf.Max(x_p3, Mathf.Max(x_p4, Mathf.Max(x_p5, Mathf.Max(x_p6, x_p7)))))));//xBox的最大投影值
            var yMin = Mathf.Min(y_p0, Mathf.Min(y_p1, Mathf.Min(y_p2, Mathf.Min(y_p3, Mathf.Min(y_p4, Mathf.Min(y_p5, Mathf.Min(y_p6, y_p7)))))));//yBox的最小投影值
            var yMax = Mathf.Max(y_p0, Mathf.Max(y_p1, Mathf.Max(y_p2, Mathf.Max(y_p3, Mathf.Max(y_p4, Mathf.Max(y_p5, Mathf.Max(y_p6, y_p7)))))));//yBox的最大投影值
            if (yMin >= xMin && yMin <= xMax) return false;
            if (yMax >= xMin && yMax <= xMax) return false;
            if (xMin >= yMin && xMin <= yMax) return false;
            if (xMax >= yMin && xMax <= yMax) return false;
            //有无交错
            return true;
            //在投影轴上的float值获取
            float GetProject_Fast(Vector3 point, Vector3 onNormal)
            {
                return Vector3.Dot(point, onNormal);
            }
        }
    }
}