using UnityEngine;

namespace ACTBook
{
    public class FallingHead : MonoBehaviour
    {
        const int FIX_TRANSFORM_COUNT = 10;//碰撞缓存数量
        public Vector3 fixCenterOffsetPoint = new Vector3(0f, 1f, 0f);//中心偏移
        public Vector2 speed = new Vector2(0.3f, 0.5f);//下落速度(最小,最大)
        public LayerMask layerMask = ~0;//层遮罩，0取反则为所有层全选
        public float radius = 1f;//椎体半径
        public float gradient = 1f;//椎体高度
        public float areaBottomExtern;//底部延伸
        public float eps = 0.001f;//误差
        Bounds mCacheBounds;
        Collider[] mCacheOverlayBoxCollider;


        void Awake()
        {
            var center = fixCenterOffsetPoint - (-Physics.gravity.normalized) * gradient * 0.5f;
            var size = new Vector3(radius * 2f, gradient, radius * 2f);
            mCacheBounds = new Bounds(center, size);

            mCacheOverlayBoxCollider = new Collider[FIX_TRANSFORM_COUNT];
        }

        void Update()
        {
            var center = transform.localToWorldMatrix.MultiplyPoint3x4(mCacheBounds.center);
            var hitsCount = Physics.OverlapBoxNonAlloc(center, mCacheBounds.extents, mCacheOverlayBoxCollider, transform.rotation, layerMask);//覆盖到的碰撞器
            for (int i = 0, iMax = hitsCount; i < iMax; i++)
            {
                var item = mCacheOverlayBoxCollider[i];
                if (item.transform == transform) continue;
                var rigidbody = item.GetComponent<Rigidbody>();//获取刚体组件
                if (rigidbody != null)
                    ConeFix(rigidbody);//如果存在刚体进行坐标下滑修复
            }
        }

        bool ConeFix(Rigidbody target)
        {
            var topCenterPoint = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint);//顶部中心点
            var diff = (target.position - topCenterPoint).normalized;
            var yAxis = -Physics.gravity.normalized;
            var offset = Vector3.ProjectOnPlane(diff, yAxis);//和椎中心偏差投影在Y轴
            var bottomCenterPoint = topCenterPoint - yAxis * gradient;//底部中心点

            if (!IsConeContain(target.position))//如果不包含在椎中则跳出
                return false;

            if (Vector3.Distance(offset, Vector3.zero) < eps)//如果过于接近椎中心则随机一个值
                offset = Vector3.ProjectOnPlane(Random.onUnitSphere, yAxis).normalized;
            var expectBottomPoint = bottomCenterPoint + offset * radius;//期望的底部位置
            var a = topCenterPoint;
            var b = expectBottomPoint - yAxis * areaBottomExtern;//减去调节值的底部位置
            var c = target.transform.position;//当前目标位置
            var finalSpeed = speed.y;
            var ab_dist = Vector3.Distance(a, b);//椎体高度
            var bc_dist = Vector3.Distance(b, c);//物体在椎体内的高度值
            if (ab_dist > bc_dist)//在椎体内
            {
                var rate = (bc_dist / ab_dist);
                finalSpeed = Mathf.Lerp(speed.x, speed.y, rate);//速度调节，越往下越快
            }
            var coneFixValue = (expectBottomPoint - topCenterPoint).normalized * finalSpeed * Time.fixedDeltaTime;
            target.velocity += coneFixValue;//将椎体修复值赋予刚体
            return true;
        }

        bool IsConeContain(Vector3 point)
        {
            var upAxis = -Physics.gravity.normalized;
            var topCenter = transform.localToWorldMatrix.MultiplyPoint3x4(mCacheBounds.center) + upAxis * gradient * 0.5f;
            var bottomCenter = transform.localToWorldMatrix.MultiplyPoint3x4(mCacheBounds.center) - upAxis * gradient * 0.5f;
            var dir = (bottomCenter - topCenter).normalized;
            var a = Vector3.Project(topCenter, dir);//顶部位置再次投影
            var b = Vector3.Project(bottomCenter, dir);//底部位置再次投影
            var c = Vector3.Project(point, dir);//比较点的投影

            var rate = Mathf.Clamp01(Vector3.Distance(a, c) / Vector3.Distance(a, b));//得到投影距离比值
            var currentRadius = Mathf.Lerp(0, radius, rate);//得到当前比值的椎体半径

            return Vector3.Distance(bottomCenter + (c - b), point) < currentRadius;//判断包含关系
        }

        void OnDrawGizmosSelected()
        {
            var cacheColor = Gizmos.color;//缓存颜色
            var yAxis = -Physics.gravity.normalized;
            var center = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint) - yAxis * gradient * 0.5f;//中心
            var diameter = radius * 2f;//直径
            var bounds = new Bounds(center, new Vector3(diameter, gradient, diameter));
            var topCenter = transform.localToWorldMatrix.MultiplyPoint3x4(fixCenterOffsetPoint);//顶部世界位置
            var bottomCenter = topCenter - yAxis * gradient;//底部世界位置
            var forward = Quaternion.FromToRotation(Vector3.up, yAxis) * Vector3.forward;//y轴方向修正后的
            var lastPoint = (Vector3?)null;
            for (int delta = 30, i = -delta; i < 360; i += delta)
            {
                var quat = Quaternion.AngleAxis(i, -yAxis);//每隔30度画圆
                var bottomPoint = bottomCenter + quat * forward * radius;

                Gizmos.DrawLine(bottomCenter, bottomPoint);//底部中心到当前底部点
                Gizmos.DrawLine(bottomPoint, topCenter);//底部点到顶部中心

                if (lastPoint != null)
                    Gizmos.DrawLine(lastPoint.Value, bottomPoint);//连接上一根线

                lastPoint = bottomPoint;
            }
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(bounds.center, bounds.size);//绘制Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(topCenter, bottomCenter - yAxis * areaBottomExtern);//绘制中心线
            Gizmos.color = cacheColor;//恢复缓存颜色
        }
    }
}
