using System.Collections.Generic;
using UnityEngine;

namespace ACTBook
{
    //碰撞管理器简单示例
    public class CollisionManager : MonoBehaviour
    {
        public class TestColliderObject//测试用对象，具体请自行扩展
        {
            public bool isPassive;//是否是被动碰撞
            public void IntersectTest(TestColliderObject obj) { print("Intersect: " + obj); }
        }

        static bool mIsDestroying;//销毁标记
        static CollisionManager mInstance;
        public static CollisionManager Instance
        {
            get
            {
                if (mIsDestroying) return null;
                if (mInstance == null)//创建单例
                {
                    var collisionMgr = new GameObject("[CollisionManager]").AddComponent<CollisionManager>();
                    DontDestroyOnLoad(collisionMgr.gameObject);
                    mInstance = collisionMgr;
                }
                return mInstance;
            }
        }
        List<TestColliderObject> mColliderList;//碰撞器成员List

        //注册碰撞对象
        public void RegistColliderObject(TestColliderObject colliderObject)
        {
            mColliderList.Add(colliderObject);
        }
        //反注册碰撞对象
        public void UnregistColliderObject(TestColliderObject colliderObject)
        {
            mColliderList.Remove(colliderObject);
        }
        //进行初始化操作
        void Awake()
        {
            mColliderList = new List<TestColliderObject>();
        }
        //更新销毁变量
        void OnDestroy()
        {
            mIsDestroying = true;
        }
        //更新注册的碰撞器
        void UpdateColliders(List<TestColliderObject> list)
        {
            for (int x = 0, max = list.Count; x < max; x++)//遍历碰撞器
            {
                if (mColliderList[x].isPassive) continue;//非主动相交对象跳出
                for (int y = x + 1; y < max; y++)//嵌套遍历
                {
                    mColliderList[x].IntersectTest(mColliderList[y]);
                }
            }
        }
    }
}
