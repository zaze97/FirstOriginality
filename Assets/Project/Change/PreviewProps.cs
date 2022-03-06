using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

public class PreviewProps : ScriptableObject
{
    private OdinMenuTree Tree;
    private string ConstPath;
    public PreviewProps(string Path,OdinMenuTree tree)
    {
        ConstPath = Path;
        Tree = tree;
        
        tree.AddAllAssetsAtPath("预览道具", ConstPath+"Change", typeof(ScriptableObject), true, true);
            //添加指定的Icon
        tree.EnumerateTree().AddIcons<CharacterWrapper>(x => x.Icon);

         
         
         
    }

    [Title("预览ScriptObject")][InlineEditor()]
    public Object scriptableObject;




}
