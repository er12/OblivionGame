using UnityEngine;

namespace Assets.Scripts.Scripts.Framework.Input
{
    /// <summary>
    /// Input command structure for platformer gameplay.
    /// Decouples input source from gameplay logic.
    /// </summary>
    public struct InputCommand
    {
        public float MovementInput { get; set; }        // Horizontal movement (-1 to 1)
        public bool JumpPressed { get; set; }           // Jump input
        public bool JumpHeld { get; set; }              // Jump held down
        public bool AttackPressed { get; set; }         // Attack button

        public InputCommand(float movement, bool jumpPressed, bool jumpHeld, bool attackPressed)
        {
            MovementInput = movement;
            JumpPressed = jumpPressed;
            JumpHeld = jumpHeld;
            AttackPressed = attackPressed;
        }
    }
}
