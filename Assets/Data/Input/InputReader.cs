using System.Collections;
using System.Collections.Generic;
using MyInputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, MyInputSystem.InputControls.IGameActions
{
    public event UnityAction<Vector2> MoveSelectionEvent = delegate { };
    private InputControls gameInput;
    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new InputControls();
            gameInput.Game.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    private void OnDisable()
    {
        DisableAllInput();
    }
    public void EnableGameplayInput()
    {
        //当前开启别的结束，只存在一个操作系统//当有别的系统时要添加进来
        gameInput.Game.Enable();
    }
    public void DisableAllInput()
    {
        //结束全部
        gameInput.Game.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            MoveSelectionEvent.Invoke(context.ReadValue<Vector2>());
    }
}
