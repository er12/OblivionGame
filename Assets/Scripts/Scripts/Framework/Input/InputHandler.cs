using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Scripts.Framework.Input
{
    /// <summary>
    /// Input handler for platformer using the new Input System package.
    /// Handles horizontal movement, jumping, and attacking.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction attackAction;

        // Track jump state
        private bool jumpPressed;
        private bool jumpHeld;

        private void Awake()
        {
            // Get or create PlayerInput component
            playerInput = GetComponent<PlayerInput>();
            if (playerInput == null)
            {
                playerInput = gameObject.AddComponent<PlayerInput>();
            }

            // Get input actions from PlayerInput asset
            if (playerInput.actions != null)
            {
                GetInputActions();
            }
        }

        private void GetInputActions()
        {
            var actionMap = playerInput.actions.FindActionMap("Player");
            if (actionMap != null)
            {
                moveAction = actionMap.FindAction("Move");
                jumpAction = actionMap.FindAction("Jump");
                attackAction = actionMap.FindAction("Attack");

                // Subscribe to jump events for proper press detection
                if (jumpAction != null)
                {
                    jumpAction.performed += OnJumpPerformed;
                    jumpAction.canceled += OnJumpCanceled;
                }
            }
        }

        private void OnEnable()
        {
            if (playerInput?.actions != null)
            {
                playerInput.actions.Enable();
            }
        }

        private void OnDisable()
        {
            if (playerInput?.actions != null)
            {
                playerInput.actions.Disable();
            }
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            jumpPressed = true;
            jumpHeld = true;
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            jumpHeld = false;
        }

        public InputCommand GetInputCommand()
        {
            float movement = GetMovementInput();
            bool attack = GetInputActionPressed(attackAction, KeyCode.Mouse0);

            // Create command with jump input
            InputCommand command = new InputCommand(movement, jumpPressed, jumpHeld, attack);
            
            // Reset the jump pressed flag after reading it
            jumpPressed = false;

            return command;
        }

        private float GetMovementInput()
        {
            // Try to get from Input Action first
            if (moveAction != null && moveAction.enabled)
            {
                // Move action is 1D (float), not 2D (Vector2)
                return moveAction.ReadValue<float>();
            }

            // Fallback to keyboard input
            float horizontal = 0;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.dKey.isPressed) horizontal += 1;
                if (Keyboard.current.aKey.isPressed) horizontal -= 1;
            }

            return horizontal;
        }

        private bool GetInputActionPressed(InputAction action, KeyCode fallbackKey)
        {
            if (action != null && action.enabled)
            {
                return action.triggered;
            }

            // Fallback to keyboard
            return Keyboard.current != null && GetKeyPressed(fallbackKey);
        }

        private bool GetKeyPressed(KeyCode key)
        {
            return key switch
            {
                KeyCode.Space => Keyboard.current.spaceKey.wasPressedThisFrame,
                KeyCode.Mouse0 => Mouse.current.leftButton.wasPressedThisFrame,
                _ => false
            };
        }
    }
}
