using System;
using UnityEngine;
public class AnimEventListener : MonoBehaviour
    {
        private string _clipName;
        private Action _action;
        private Action<string> _stringAction;
        private int _times;
        /// <summary>
        /// 添加帧事件(不带参)
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="clipName">动画片段的名字(priject)</param>
        /// <param name="time">添加到指定时间的位置小数表示</param>
        /// <param name="action">要添加的事件</param>
        /// <param name="times">执行的次数，-1表示持续执行</param>
        public static void AddEvent( GameObject obj, string clipName, float time, Action action, int times = -1)
        {
            GetListener(obj, clipName, times)._action = action;
            obj.AddAnimationEvent(clipName, "DoAction", time);
        }
        private void DoAction()
        {
            if (_action != null)
            {
                _action();
                CheckTimes();
            }
        }
        /// <summary>
        /// 添加帧事件(带参)
        /// </summary>
        /// <param name="obj">物体</param>
        /// <param name="clipName">动画片段的名字(priject)</param>
        /// <param name="time">添加到指定时间的位置小数表示</param>
        /// <param name="action">要添加的事件</param>
        /// <param name="stringParameter">事件的参数</param>
        /// <param name="times">执行的次数，-1表示持续执行</param>
        public static void AddEvent(GameObject obj, string clipName, float time, Action<string> action, string stringParameter, int times = -1)
        {
            GetListener(obj, clipName, times)._stringAction = action;
            obj.AddAnimationEvent(clipName, "DoStringAction", time, stringParameter);
        }
        
        private void DoStringAction(string stringParameter)
        {
            if (_stringAction != null)
            {
                _stringAction(stringParameter);
                CheckTimes();
            }
        }
        /// <summary>
        /// 侦听事件
        /// </summary>
        /// <returns></returns>
        private static AnimEventListener GetListener(GameObject obj, string clipName, int times)
        {
            AnimEventListener listener = obj.GetComponent<AnimEventListener>()??null;
            if (listener == null) listener = obj.AddComponent<AnimEventListener>();
            listener._clipName = clipName;
            listener._times = times;
            return listener;
        }
        private void CheckTimes()
        {
            if (_times >= 0)
            {
                _times--;
                if (_times <= 0) Destroy(this);
            }
  
        }
        private void OnDestroy()
        {
            gameObject.GetClip(_clipName).UnSubscription();
        }
    }

