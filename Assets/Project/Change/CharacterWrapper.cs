using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable] [HideMonoScript]
public class CharacterWrapper : ScriptableObject
{
    private string ConstPath;
    public CharacterWrapper(string Path)
    {
        this.ConstPath = Path;
    }
    
    [HideInInspector]
    public bool isHide;
    [BoxGroup("POP"),TableColumnWidth(100, true)] [ShowInInspector, PreviewField(100, ObjectFieldAlignment.Center)]
    public Texture Icon;
    [BoxGroup("POP"),ShowInInspector,TableColumnWidth(100),OnValueChanged(nameof(PathNameOnValue)),Delayed,DisableIf("@isHide"),SuffixLabel("名字")]
    [Required("请填写名字", InfoMessageType.Error)]
    public string Name;

    [BoxGroup("POP"),ShowInInspector,ProgressBar(0, 100),SuffixLabel("生命")]
    public float Shooting;

    [BoxGroup("POP"),ShowInInspector,ProgressBar(0, 100),SuffixLabel("能量")]
    public float Melee;

    [BoxGroup("POP"),ShowInInspector,ProgressBar(0, 100),SuffixLabel("攻击")]
    public float Social;

    [BoxGroup("POP"),ShowInInspector,ProgressBar(0, 100),SuffixLabel("防御")]
    public float Animals;
    
    public void PathNameOnValue()
    {
        
        Path = ConstPath+Name+ ".asset";
    }


    [PropertyOrder(2), ReadOnly] [HideIf("@isHide")]
    public string Path;

    
    [PropertyOrder(2)][ButtonGroup("Button")]
    [Button( "保存",ButtonStyle.Box)]
    [GUIColor(0,1,0)]
    [HideIf("@isHide")]
    public void Save()
    {
        if (!Path.EndsWith(".asset")||Name == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("警告", "文件结尾不是.asset不能创建，请填写Name", "确认");
            return;
        }
        Debug.Log(ConstPath);

        if (Directory.Exists(ConstPath))
        {

            DirectoryInfo direction = new DirectoryInfo(Path);
            
            string[] files = Directory.GetFiles(ConstPath,"*", SearchOption.AllDirectories);
            
            Debug.Log("寻找到" + files.Length + "文件");

            for (int i = 0; i < files.Length; i++)
            {

                if (files[i].EndsWith(".meta"))
                {

                    continue;
                }

                Debug.Log(files[i]);
                if (files[i].Contains(Name))
                {
                    Debug.Log("删除文件"+files[i]);
                    File.Delete(files[i]);
                }
            }
        }


        isHide = true;
        ScriptableObject level =this;
        AssetDatabase.CreateAsset(level, @Path);
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
        // CharacterWrapper.Add();

    }
    [PropertyOrder(2)][ButtonGroup("Button")]
    [Button("重置",ButtonStyle.Box)]
    [GUIColor(1,0,0)]
    [HideIf("@isHide")]
    public void Reset()
    {
        Icon = null;
        Name = null;
        Shooting = 0;
        Melee = 0;
        Social = 0;
        Animals=0;
        Path = "Assets/Data/Levels";
    }
    
}
