using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

/// <summary>
/// 全局的初始化，用的的类需要在此初始化
/// </summary>
public class GlobalInit : Architecture<GlobalInit>
{
    protected override void Init()
    {
        RegisterSystem<IPanelManager>(new PanelManager());
        RegisterSystem<ISceneLoadManager>(new SceneLoadManager());
    }
}
