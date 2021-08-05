using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos.RPGEditor;
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
        
        if (Directory.Exists(ConstPath))
        {

            DirectoryInfo direction = new DirectoryInfo(Path);

            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            Debug.Log("寻找到" + files.Length + "文件");

            for (int i = 0; i < files.Length; i++)
            {

                if (files[i].Name.EndsWith(".meta"))
                {

                    continue;
                }

                Debug.Log("Name:" + files[i].Name);
            }
        }
        
         tree.AddAllAssetsAtPath("预览道具", ConstPath, typeof(ScriptableObject), true, true);
            //添加指定的Icon
         tree.EnumerateTree().AddIcons<CharacterWrapper>(x => x.Icon);

        }  
        

    
    
}
