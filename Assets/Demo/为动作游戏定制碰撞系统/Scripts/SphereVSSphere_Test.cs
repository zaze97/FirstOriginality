using UnityEngine;

namespace ACTBook
{
    public class SphereVSSphere_Test : MonoBehaviour
    {
        public Sphere a;//测试用球体a
        public Sphere b;//测试用球体b


        void Start()
        {
            Debug.Log("[Sphere vs Sphere]Is Instect: " + SphereVSSphere(a, b));//打印是否相交
        }
        bool SphereVSSphere(Sphere aSphere, Sphere bSphere)
        {
            return Vector3.Distance(aSphere.WorldCenter, bSphere.WorldCenter) <= (aSphere.ConcertRadius + bSphere.ConcertRadius);//距离检测相交
        } //注意，出于篇幅原有，此处与书中内容不一致。书中的版本可以理解为不含缩放的球对象。
    }
}
