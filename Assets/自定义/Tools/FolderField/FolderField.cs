using UnityEditor;
using UnityEngine;

namespace EditorFramework
{
    public class FolderField : GUIBase
    {
        public FolderField(string path = "Assets/", string folder = "Assets", string title = "Select Folder",
            string defaultName = "")
        {
            mPath = path;
            Title = title;
            Folder = folder;
            DefaultName = defaultName;
        }
        
        protected string mPath;
        
        
        public string Path => mPath;
        public string Title;
        public string Folder;
        public string DefaultName;

        public void SetPath(string path)
        {
            mPath = path;
        }
        
        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);
            
            var rects = position.VerticalSplit(position.width - 30);
            var leftRect = rects[0];
            var rightRect = rects[1];

            var currentGUIEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.TextField(leftRect, mPath); // 左边的 rect width - 30
            GUI.enabled = currentGUIEnabled;
            
            if (GUI.Button(rightRect,GUIContents.Folder)) // 右边的 rect 30
            {
                var path = EditorUtility.OpenFolderPanel(Title, Folder, DefaultName);

                if (!string.IsNullOrEmpty(path) && path.IsDirectory())
                {
                    mPath = path.ToAssetsPath()+"/";
                }
            }

            DragAndDropTool.Drag(leftRect, OnComplete);
            
        }
        private void OnComplete(DragAndDropTool.DragInfo info)
        {
            if (info.Paths[0].IsDirectory())
            {
                mPath = info.Paths[0]+"/";
            }
        }
        protected override void OnDispose()
        {

        }
    }
}