// DictionaryExamplesComponent.cs

using System;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR // Editor namespaces can only be used in the editor.
using Sirenix.OdinInspector.Editor.Examples;
#endif

public class DictionaryExamplesComponent : MonoBehaviour
{
 
    [HorizontalGroup]
    public int A;

    [HorizontalGroup("Group 1", LabelWidth = 20)]
    public int C;

    [HorizontalGroup("Group 1")]
    public int D;

    [HorizontalGroup("Group 1")]
    public int E;

    [PropertySpace(40, 40)]
    public string space = "";

    [HorizontalGroup("Split", 0.5f, LabelWidth = 40)]
    [BoxGroup("Split/Left")]
    public int L;
    [BoxGroup("Split/Left")]
    public int N;
    [BoxGroup("Split/Right")]
    public int M;
    [BoxGroup("Split/Right")]
    public int O;

    [PropertySpace(40, 40)]
    public string space1 = "";

    [Button(ButtonSizes.Large)]
    [FoldoutGroup("Buttons in Boxes")]
    [HorizontalGroup("Buttons in Boxes/Horizontal")]
    [BoxGroup("Buttons in Boxes/Horizontal/One")]
    public void Button1() { }

    [Button(ButtonSizes.Large)]
    [BoxGroup("Buttons in Boxes/Horizontal/Two")]
    public void Button2() { }
    
    [Button(ButtonSizes.Large)]
    [BoxGroup("Buttons in Boxes/Horizontal/Three")]
    public void Button3() { }
    [Button]
    [HorizontalGroup("Buttons in Boxes/Horizontal", Width = 80)]
    [BoxGroup("Buttons in Boxes/Horizontal/Double")]
    public void Accept() { }

    [Button]
    [BoxGroup("Buttons in Boxes/Horizontal/Double")]
    public void Cancel() { }
}

