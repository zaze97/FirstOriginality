using UnityEngine;

    public static class AnimManager
    {
        public static float GetAnimationLenghtByName(this Animator animator, string name)
        {
            RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
            AnimationClip[] clips = runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Equals(name))
                {
                    return clip.length;
                }
            }
            return 0;
        }
        public static bool IsPlayAnimation(this Animator animator, string animationName, int layerIndex = 0)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return stateInfo.IsName(animationName);
        }
        public static AnimationClip GetClip(this GameObject obj,string clipName)
        {
            return GetClip(obj.GetComponent<Animator>(), clipName);
        }
        public static AnimationClip GetClip(this Animator animator,string clipName)
        {
            AnimationClip[] animationClips = GetClips(animator);
            if (animationClips == null) return null;
            for (int i = 0; i < animationClips.Length; i++)
            {
                if (animationClips[i].name == clipName) return animationClips[i];
            }
            Debug.LogError("找不到该动画：" + clipName);
            return null;
        }
        public static AnimationClip GetClip(this GameObject obj, string clipName, int layerIndex)
        {
            return GetClip(obj.GetComponent<Animator>(), clipName, layerIndex);
        }
        public static AnimationClip GetClip(this Animator animator, string clipName, int layerIndex)
        {
            AnimatorClipInfo[] animatorClipInfos = GetAnimatorClipInfos(animator, layerIndex);
            for (int i = 0; i < animatorClipInfos.Length; i++)
            {
                if (animatorClipInfos[i].clip.name == clipName) return animatorClipInfos[i].clip;
            }
            Debug.LogError("找不到该动画：" + clipName);
            return null;
        }
        public static AnimationClip[] GetClips(this Animator animator)
        {
            RuntimeAnimatorController m_runtimeAnimatorController = animator.runtimeAnimatorController;
            if (m_runtimeAnimatorController == null) return null;
            return m_runtimeAnimatorController.animationClips;
        }
        public static AnimatorClipInfo[] GetAnimatorClipInfos(this Animator animator,int layerIndex)
        {
            return animator.GetCurrentAnimatorClipInfo(layerIndex);
        }
        /// <summary>
        /// Destroy 要将事件移除，否则会被调多次
        /// </summary>
        public static AnimationEvent AddAnimationEvent(this GameObject obj, string clipName,string functionName, float time=0, string stringParameter = null)
        {
            return AddAnimationEvent(obj.GetClip(clipName), functionName, time, stringParameter);
        }
        /// <summary>
        /// Destroy 要将事件移除，否则会被调多次
        /// </summary>
        public static AnimationEvent AddAnimationEvent(this AnimationClip clip,string functionName,float time=0,string stringParameter = null)
        {
            //创建动画事件
            AnimationEvent animationEvent = new AnimationEvent();
            //设置事件回掉函数名字
            animationEvent.functionName = functionName;
            //传入参数
            animationEvent.stringParameter = stringParameter;
            //设置触发帧
            animationEvent.time = time;
            //注册事件
            clip.AddEvent(animationEvent);
            return animationEvent;
        }
        /// <summary>
        /// 注销所有事件
        /// </summary>
        public static void UnSubscription(this AnimationClip animationClip)
        {
            if(animationClip!=null) animationClip.events = default(AnimationEvent[]);
        }
    }

