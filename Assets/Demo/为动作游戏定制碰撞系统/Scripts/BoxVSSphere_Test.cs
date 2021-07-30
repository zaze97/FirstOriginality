using UnityEngine;

namespace ACTBook
{
    public class BoxVSSphere_Test : MonoBehaviour
    {
        public Box a;//测试用box
        public Sphere b;//测试用球体


        void Start()
        {
            Debug.Log("[Box vs Sphere]Is Instect: " + BoxVSSphere(a, b));//打印是否相交
        }
        bool BoxVSSphere(Box box, Sphere sphere)
        {
            var localSpherePoint = box.transform.InverseTransformPoint(sphere.WorldCenter);//转换到Box本地空间
            var extents = box.Extents;
            var xMin = box.center.x + (-extents.x);//Box的x最小值
            var xMax = box.center.x + extents.x;//Box的x最大值
            var yMin = box.center.y + (-extents.y);//Box的y最小值
            var yMax = box.center.y + extents.y;//Box的y最大值
            var zMin = box.center.z + (-extents.z);//Box的z最小值
            var zMax = box.center.z + extents.z;//Box的z最大值
            var outside = false;//球的中心点是否在Box外
            if (localSpherePoint.x < xMin)//是否小于x最小值
            {
                outside = true;
                localSpherePoint.x = xMin;//把最小值赋予中心点
            }
            else if (localSpherePoint.x > xMax)//是否大于x最大值
            {
                outside = true;
                localSpherePoint.x = xMax;//把最大值赋予中心点
            }
            if (localSpherePoint.y < yMin)//是否小于y最小值
            {
                outside = true;
                localSpherePoint.y = yMin;//把最小值赋予中心点
            }
            else if (localSpherePoint.y > yMax)//是否大于y最大值
            {
                outside = true;
                localSpherePoint.y = yMax;//把最大值赋予中心点
            }
            if (localSpherePoint.z < zMin)//是否小于z最小值
            {
                outside = true;
                localSpherePoint.z = zMin;//把最小值赋予中心点
            }
            else if (localSpherePoint.z > zMax)//是否大于z最大值
            {
                outside = true;
                localSpherePoint.z = zMax;//把最大值赋予中心点
            }
            if (outside)//如果在Box外
            {
                var edgePoint = box.transform.TransformPoint(localSpherePoint);//转换回来就是边界点
                var distance = Vector3.Distance(sphere.WorldCenter, edgePoint);
                if (distance > sphere.radius)//边界点到中心距离是否大于球的半径
                    return false;
            }
            return true;
        }
    }
}
