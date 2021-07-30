using UnityEngine;

namespace ACTBook
{
    public class Tutorial : MonoBehaviour
    {
        void OnGUI()
        {
            GUILayout.Box("该案例演示了使用非'内置CharacterController角色控制器'方式实现的角色控制。" +
                "\nW/A/S/D键可以进行角色移动。");
        }
    }
}
