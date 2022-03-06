using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorFramework
{
    public static class DragAndDropTool 
    {
        public class DragInfo
        {
            public bool Dragging;
            public bool EnterArea;
            public bool Complete;
            public Object[] ObjectReferences => DragAndDrop.objectReferences;
            public string[] Paths => DragAndDrop.paths;
            public DragAndDropVisualMode VisualMode => DragAndDrop.visualMode;
            public int ActiveControlID => DragAndDrop.activeControlID;
        }

        private static DragInfo mDragInfo = new DragInfo();

        private static bool mDragging;//拖拽进区域为ture
        private static bool mEnterArea;//拖拽进content区域为ture
        private static bool mComplete;
        /// <summary>
        /// 拖拽通用方法
        /// </summary>
        /// <param name="content">检测区域</param>
        /// <param name="OnComplete">完成后的时间，利用泛型传进详细的DragInfo</param>
        /// <param name="mode"></param>
        public static void Drag(Rect content,Action<DragInfo> onComplete=null, DragAndDropVisualMode mode = DragAndDropVisualMode.Generic)
        {

            Event e=Event.current;
            if (e.type == EventType.DragUpdated)
            {
                mComplete = false;
                mDragging = true;
                mEnterArea = content.Contains(e.mousePosition);
                if (mEnterArea)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    e.Use();
                }
            }
            else if (e.type == EventType.DragPerform)
            {
                mComplete = true;
                mDragging = false;
                mEnterArea = content.Contains(e.mousePosition);
                DragAndDrop.AcceptDrag();
                e.Use();
            }
            else if (e.type == EventType.DragExited)
            {
                mComplete = true;
                mDragging = false;
                mEnterArea = content.Contains(e.mousePosition);
            }

            mDragInfo.Complete = mComplete && e.type == EventType.Used;
            mDragInfo.EnterArea = mEnterArea;
            mDragInfo.Dragging = mDragging;
            if (mDragInfo.EnterArea && mDragInfo.Complete && !mDragInfo.Dragging)
            {
                onComplete?.Invoke(mDragInfo);
            }
        }
        
        public static DragInfo Drag(Event e,Rect content,DragAndDropVisualMode mode = DragAndDropVisualMode.Generic)
        {
            if (e.type == EventType.DragUpdated)
            {
                mComplete = false;
                mDragging = true;
                mEnterArea = content.Contains(e.mousePosition);
                if (mEnterArea)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    e.Use();
                }
            }
            else if (e.type == EventType.DragPerform)
            {
                mComplete = true;
                mDragging = false;
                mEnterArea = content.Contains(e.mousePosition);
                DragAndDrop.AcceptDrag();
                e.Use();
            }
            else if (e.type == EventType.DragExited)
            {
                mComplete = true;
                mDragging = false;
                mEnterArea = content.Contains(e.mousePosition);
            }
            // else
            // {
            //     mComplete = false;
            //     mDragging = false;
            //     mEnterArea = content.Contains(e.mousePosition);
            // }
  
            
            mDragInfo.Complete = mComplete && e.type == EventType.Used;
            mDragInfo.EnterArea = mEnterArea;
            mDragInfo.Dragging = mDragging;
            return mDragInfo;
        }
    }
}