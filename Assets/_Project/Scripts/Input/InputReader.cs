using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace _Project.Scripts.Input
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerActions();
    }
    
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public UnityAction<Vector2> Move = delegate {  };
        public UnityAction<bool> Jump = delegate {  };

        public InputSystem_Actions InputActions;
        
        public Vector2 Direction => InputActions.Player.Move.ReadValue<Vector2>();
        
        public bool IsJumpKeyPressed => InputActions.Player.Jump.IsPressed();
        public void EnablePlayerActions()
        {
            if (InputActions == null)
            {
                InputActions = new InputSystem_Actions();
                InputActions.Player.SetCallbacks(this);
            }
            InputActions.Enable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            
        }
        public void OnCrouch(InputAction.CallbackContext context)
        {
            
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    Jump?.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump?.Invoke(false);
                    break;
            }
        }
        public void OnPrevious(InputAction.CallbackContext context)
        {
            
        }
        public void OnNext(InputAction.CallbackContext context)
        {
            
        }
        public void OnSprint(InputAction.CallbackContext context)
        {
            
        }
    }
}
