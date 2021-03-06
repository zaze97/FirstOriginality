using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class ChangeEditor :OdinMenuEditorWindow
{
    public string Path="Assets/Data/Levels/";
    
    [MenuItem("Utility/换装系统编辑器")]
    public static void CreatWizard()
    {
        GetWindow<ChangeEditor>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;
        // tree.Add("预览面板", new PreviewPane());
        // tree.Add("添加道具", new CharacterWrapper(Path));
        // tree.Add("预览道具", new PreviewProps(Path,tree));
        // tree.Add("模型装备信息分类",new ModelToPrefab(Path));
        
        tree.AddAllAssetsAtPath("预览道具", Path+"Change", typeof(ScriptableObject), true, true);
        //添加指定的Icon
        tree.EnumerateTree().AddIcons<CharacterWrapper>(x => x.Icon);
        return tree;
    }
}

public class PreviewPane : SerializedScriptableObject
{

}
