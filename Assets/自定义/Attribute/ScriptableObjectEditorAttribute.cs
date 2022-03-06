using System;

namespace EditorWindows
{
    public class ScriptableObjectEditorAttribute:Attribute
    {
        public int RenderOrder { get; private set; }

        public ScriptableObjectEditorAttribute(int order=-1)
        {
            RenderOrder = order;
        }
    }
}