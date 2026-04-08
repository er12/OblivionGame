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
        public bool LightAttackPressed { get; set; }    // Light attack (e.g., X button)
        public bool HeavyAttackPressed { get; set; }    // Heavy attack (e.g., Y button)

        public InputCommand(float movement, bool jumpPressed, bool jumpHeld, bool lightAttack = false, bool heavyAttack = false)
        {
            MovementInput = movement;
            JumpPressed = jumpPressed;
            JumpHeld = jumpHeld;
            LightAttackPressed = lightAttack;
            HeavyAttackPressed = heavyAttack;
        }
    }
}
