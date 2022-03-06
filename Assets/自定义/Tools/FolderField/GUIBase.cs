using System;
using UnityEngine;

namespace EditorFramework
{
    public abstract class GUIBase :IDisposable
    {
        public bool mDisposed { get; protected set; }
        public Rect mPosition { get; protected set; }

        public virtual void OnGUI(Rect position)
        {
            mPosition = position;
        }
        public void Dispose()
        {
            if(mDisposed) return;
            OnDispose();
            mDisposed = true;
        }

        protected abstract void OnDispose();
    }
}