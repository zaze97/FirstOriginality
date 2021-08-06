using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
[HideMonoScript]
public class ModelToPrefab : ScriptableObject
{
    private string ConstPath;
    private bool IsToggled;

    public ModelToPrefab(string Path)
    {
        this.ConstPath = Path;
    }

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)] [Title("Hierarchy面板的模型")][ShowIf("@ IsToggled== false")]
    public GameObject ModelPrefab;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("头发")] [HideLabel,ReadOnly]
    public string Hair;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("头")] [HideLabel,ReadOnly]
    public string Face;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("单手武器")] [HideLabel,ReadOnly]
    public string Weapon;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("左手武器")] [HideLabel,ReadOnly]
    public string LeftWeapon;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("右手武器")] [HideLabel,ReadOnly]
    public string RightWeapon;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("衣服")] [HideLabel,ReadOnly]
    public string Cloth;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("护腕")] [HideLabel,ReadOnly]
    public string Glove;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("头盔")] [HideLabel,ReadOnly]
    public string Hat;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("鞋")] [HideLabel,ReadOnly]
    public string Shoe;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("肩甲")] [HideLabel,ReadOnly]
    public string ShoulderPad;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("腰带")] [HideLabel,ReadOnly]
    public string Belt;

    [MultiLineProperty(10), HorizontalGroup("message")] [Title("王冠")] [HideLabel,ReadOnly]
    public string Crown;

    [ButtonGroup("Button")]
    [Button("开始转换", ButtonSizes.Gigantic)]
    [GUIColor(0, 1, 0)]
    [ShowIf("@ IsToggled== false")]
    public void ToTransform()
    {
        if (ModelPrefab == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("警告", "模型为空，请添加模型", "确认");
            return;
        }
        EditorApplication.update += Updte;
    }

    [ButtonGroup("Button")]
    [Button("清空", ButtonSizes.Gigantic)]
    [GUIColor(1, 0, 0)]
    [ShowIf("@ IsToggled== false")]
    public void Clear()
    {
        ModelPrefab = null;
        Hair = null;
        Face = null;
        Weapon = null;
        LeftWeapon = null;
        RightWeapon = null;
        Cloth = null;
        Glove = null;
        Hat = null;
        Shoe = null;
        ShoulderPad = null;
        Belt = null;
        Crown = null;
    }

    [ButtonGroup("Button")]
    [Button("导出ScroptOnject", ButtonSizes.Gigantic)]
    [GUIColor(0, 0, 1)]
    [ShowIf("@ IsToggled== false")]
    public void Derive()
    {
        string SOPath = ConstPath + nameof(ModelToPrefab) + ".asset";
        if (!SOPath.EndsWith(".asset"))
        {
            UnityEditor.EditorUtility.DisplayDialog("警告", "文件结尾不是.asset不能创建", "确认");
            return;
        }
        Debug.Log(SOPath);

        var filePath =IOFileHelper.LoadFile(ConstPath, nameof(ModelToPrefab));
        if (filePath.Count > 0)
        {
            bool isdelete  =UnityEditor.EditorUtility.DisplayDialog("警告", "已存在"+ConstPath+"文件，是否删除,并重新创建", "是","否");
            if (isdelete)
            {
                for (int i = 0; i < filePath.Count; i++)
                {
                    Debug.Log("删除文件");
                    IOFileHelper.DeleteFile(ConstPath + filePath[i]);

                }

                AssetDatabase.Refresh(); //刷新 
            }
            else
            {
                return;
            }
        }
        IsToggled = true;
        ScriptableObject level =this;
        AssetDatabase.CreateAsset(level, @SOPath);
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新
    }
    
    private void Updte()
    {
        Transform[] gos = ModelPrefab.GetComponentsInChildren<Transform>(true);
        foreach (var go in gos)
        {
            EquipmentFication(go.name);
        }

        IsToggled = false;
        EditorApplication.update -= Updte;
    }

    private void EquipmentFication(string name)
    {
        if (name.Contains("Belt"))
        {
            Belt += name + ",";
        }
        else if (name.Contains("Cloth"))
        {
            Cloth += name + ",";
        }
        else if (name.Contains("Crown"))
        {
            Crown += name + ",";
        }
        else if (name.Contains("Face"))
        {
            Face += name + ",";
        }
        else if (name.Contains("Glove"))
        {
            Glove += name + ",";
        }
        else if (name.Contains("Hair"))
        {
            Hair += name + ",";
        }
        else if (name.Contains("Hat") || name.Contains("Helm"))
        {
            Hat += name + ",";
        }
        else if (name.Contains("Shoe"))
        {
            Shoe += name + ",";
        }
        else if (name.Contains("ShoulderPad"))
        {
            ShoulderPad += name + ",";
        }
        else if (name.Contains("_L"))
        {
            LeftWeapon += name + ",";
        }
        else if (name.Contains("_R"))
        {
            if (name.Contains("_D"))
            {
                Weapon += name + ",";
            }
            else
            {
                RightWeapon += name + ",";
            }
        }
    }
}