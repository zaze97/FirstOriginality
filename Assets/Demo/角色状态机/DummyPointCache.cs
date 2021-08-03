using System;
using UnityEngine;

public class DummyPointCache : MonoBehaviour
{
    [Serializable]
    public class Cache                                   //缓存结构
    {
        public string name;
        public GameObject gameObject;
    }
    [SerializeField]
    Cache[] cacheArray;                                         //缓存数组，编辑器下配置
    public GameObject SearchByName(string name)          //外部查找接口
    { 
        for (int i = 0; i < cacheArray.Length; i++)
        {
            var item = cacheArray[i];
            if (item.name == name)                          //遍历并比较名称
                return item.gameObject;                     //返回对应对象
        }
        return null;//若未找到返回空
    }
}