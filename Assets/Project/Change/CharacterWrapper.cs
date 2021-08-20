using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[Serializable]
[HideMonoScript]
public class CharacterWrapper : ScriptableObject
{
    private string ConstPath;

    public CharacterWrapper(string Path)
    {
        this.ConstPath = Path+"Change/";
    }

    private bool isHide;

    [HorizontalGroup("Change", 0.5f, LabelWidth = 40)] //设置一个父节点，然后水平排列，然后Box子节点垂直排列
    [BoxGroup("Change/POP"), TableColumnWidth(100, true)]
    [ShowInInspector, PreviewField(100, ObjectFieldAlignment.Center), LabelText("图标")]
    public Texture Icon;

    [BoxGroup("Change/POP"), ShowInInspector, TableColumnWidth(100), OnValueChanged(nameof(PathNameOnValue)), Delayed,
     DisableIf("@isHide"), LabelText("名字")]
    [Required("请填写名字", InfoMessageType.Error)]
    public string Name;

    [BoxGroup("Change/POP"), ShowInInspector, ProgressBar(0, 100), LabelText("装备")]
    public float Shooting;

    [BoxGroup("Change/POP"), ShowInInspector, ProgressBar(0, 100), LabelText("能量")]
    public float Melee;

    [BoxGroup("Change/POP"), ShowInInspector, ProgressBar(0, 100), LabelText("攻击")]
    public float Social;

    [BoxGroup("Change/POP"), ShowInInspector, ProgressBar(0, 100), LabelText("防御")]
    public float Animals;



    [BoxGroup("Change/Prop"), EnumToggleButtons, ShowInInspector, LabelText("装备"), LabelWidth(50)]
    public Prop prop;

    [BoxGroup("Change/Prop"), EnumToggleButtons, ShowInInspector, ShowIf(nameof(prop), Prop.武器), LabelText("武器类型"),
     LabelWidth(50)]
    public WeaponType weaponType;

    [BoxGroup("Change/Prop"), EnumToggleButtons, ShowInInspector,
     ShowIf("@this.prop==Prop.武器&&this.weaponType==WeaponType.单手"), LabelText("单手武器"), LabelWidth(50)]
    public OneWeapon oneWeapon;

    [BoxGroup("Change/Prop"), EnumToggleButtons, ShowInInspector,
     ShowIf("@this.prop==Prop.武器&&this.weaponType==WeaponType.双手"), LabelText("双手武器-左"), LabelWidth(50)]
    public TwoWeaponLeft twoWeaponLeft;

    [BoxGroup("Change/Prop"), EnumToggleButtons, ShowInInspector,
     ShowIf("@this.prop==Prop.武器&&this.weaponType==WeaponType.双手"), LabelText("双手武器-右"), LabelWidth(50)]
    public TwoWeaponRight twoWeaponRight;


    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.头发), ShowInInspector, LabelText("头发"), LabelWidth(100)]
    public GameObject Hair;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.头), ShowInInspector, LabelText("头"), LabelWidth(100)]
    public GameObject Face;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf("@this.prop==Prop.武器&&this.weaponType==WeaponType.单手"), ShowInInspector,
     LabelText("武器"), LabelWidth(100)]
    public GameObject Weapon;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf("@this.prop==Prop.武器&&this.weaponType==WeaponType.双手"), ShowInInspector,
     LabelText("左手武器"), LabelWidth(100)]
    public GameObject LeftWeapon;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf("@this.prop==Prop.武器&&this.weaponType==WeaponType.双手"), ShowInInspector,
     LabelText("右手武器"), LabelWidth(100)]
    public GameObject RightWeapon;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.衣服), ShowInInspector, LabelText("衣服"), LabelWidth(100)]
    public GameObject Cloth;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.护腕), ShowInInspector, LabelText("护腕"), LabelWidth(100)]
    public GameObject Glove;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.头盔), ShowInInspector, LabelText("头盔"), LabelWidth(100)]
    public GameObject Hat;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.鞋), ShowInInspector, LabelText("鞋"), LabelWidth(100)]
    public GameObject Shoe;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.肩甲), ShowInInspector, LabelText("肩甲"), LabelWidth(100)]
    public GameObject ShoulderPad;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.腰带), ShowInInspector, LabelText("腰带"), LabelWidth(100)]
    public GameObject Belt;

    [InlineEditor(InlineEditorObjectFieldModes.Boxed)]
    [BoxGroup("Change/Prop"), ShowIf(nameof(prop), Prop.王冠), ShowInInspector, LabelText("王冠"), LabelWidth(100)]
    public GameObject Crown;

    public void PathNameOnValue()
    {

        Path = ConstPath  + Name + ".asset";
    }


    [PropertyOrder(2), ReadOnly] [HideIf("@isHide"), LabelText("文件路径")]
    public string Path;


    [PropertyOrder(2)]
    [ButtonGroup("Button")]
    [Button("保存", ButtonStyle.Box)]
    [GUIColor(0, 1, 0)]
    [HideIf("@isHide")]
    public void Save()
    {
        if (!Path.EndsWith(".asset") || Name == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("警告", "文件结尾不是.asset不能创建，请填写Name", "确认");
            return;
        }
        
        var filePath = IOFileHelper.LoadFile(ConstPath, Name);
        Debug.Log(filePath);
        if (filePath.Count > 0)
        {
            bool isdelete =
                UnityEditor.EditorUtility.DisplayDialog("警告", "已存在" + ConstPath + "文件，是否删除,并重新创建", "是", "否");
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

        isHide = true;
        ScriptableObject level = this;
        AssetDatabase.CreateAsset(level, @Path);
        AssetDatabase.SaveAssets(); //存储资源
        AssetDatabase.Refresh(); //刷新

    }

    [PropertyOrder(2)]
    [ButtonGroup("Button")]
    [Button("重置", ButtonStyle.Box)]
    [GUIColor(1, 0, 0)]
    [HideIf("@isHide")]
    public void Reset()
    {
        Icon = null;
        Name = null;
        Shooting = 0;
        Melee = 0;
        Social = 0;
        Animals = 0;
        Path = ConstPath;
    }

    [PropertyOrder(2)]
    [ButtonGroup("Button")]
    [Button("删除", ButtonStyle.Box)]
    [GUIColor(1, 0, 0)]
    [ShowIf("@isHide")]
    public void Delete()
    {
        var filePath = IOFileHelper.LoadFile(ConstPath, Name); 
        if (filePath.Count > 0)
        {
            bool isdelete = UnityEditor.EditorUtility.DisplayDialog("警告", "是否确认删除", "是", "否");
            if (isdelete)
            {
                for (int i = 0; i < filePath.Count; i++)
                {
                    Debug.Log("删除文件"+ConstPath + filePath[i]);
                    IOFileHelper.DeleteFile(ConstPath + filePath[i]);

                }

                AssetDatabase.Refresh(); //刷新 
            }
            else
            {
                return;
            }
        }

    }

    public enum Prop
    {
        头发 = 0,
        头 = 1,
        武器 = 2,
        衣服 = 3,
        护腕 = 4,
        头盔 = 5,
        鞋 = 6,
        肩甲 = 7,
        腰带 = 8,
        王冠 = 9,
    }

    public enum WeaponType
    {
        单手 = 0,
        双手 = 1,
    }

    public enum OneWeapon
    {
        魔杖 = 0,
        大剑 = 4,
    }

    public enum TwoWeaponLeft
    {
        弓_左 = 0,
        斧_左 = 1,
        锤_左 = 2,
        盾_左 = 3,
    }

    public enum TwoWeaponRight
    {
        箭_右 = 0,
        斧_右 = 1,
        锤_右 = 2,
        剑_右 = 3,
    }
}