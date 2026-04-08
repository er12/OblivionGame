using UnityEngine;
using Assets.Scripts.Player.States;

namespace Assets.Scripts.Player.States.Combat
{
    /// <summary>
    /// Light attack combo state for the player state machine.
    /// Fast attacks with lower damage: Jab → Cross → Hook
    /// </summary>
    public class LightComboAttackState : ComboAttackStateBase
    {
        private float lightDamage = 10f;
        private float knockbackForce = 5f;

        public LightComboAttackState(PlayerController player, PlayerStateMachine stateMachine) 
            : base(player, stateMachine)
        {
            // Setup light attack animations
            comboAnimations.Add("LightAttack1");  // Jab
            comboAnimations.Add("LightAttack2");  // Cross
            comboAnimations.Add("LightAttack3");  // Hook

            comboDuration = 1.5f;
            maxComboCount = 3;
            attackDuration = 0.4f;
            attackCooldown = 0.5f;
        }

        /// <summary>
        /// Execute light attack logic.
        /// </summary>
        protected override void OnComboAttackStarted(int comboIndex)
        {
            float damage = lightDamage * (1f + (comboIndex - 1) * 0.25f);

            Debug.Log($"Light Attack Combo {comboIndex}: Damage = {damage}");

            switch (comboIndex)
            {
                case 1: // Jab
                    Debug.Log("⚔ Jab - Quick strike!");
                    break;

                case 2: // Cross
                    Debug.Log("⚔ Cross - Stronger hit!");
                    break;

                case 3: // Hook
                    Debug.Log("⚔ Hook - Finisher combo!");
                    ApplyKnockback(knockbackForce * 1.5f);
                    break;
            }

            // Spawn visual effects
            SpawnAttackEffects(comboIndex);
        }

        /// <summary>
        /// Apply knockback to enemies.
        /// </summary>
        private void ApplyKnockback(float force)
        {
            // Implement knockback on enemies here
            // Example: Raycast to find enemies in range and apply force
            Debug.Log($"Knockback applied: {force}");
        }

        /// <summary>
        /// Spawn visual effects.
        /// </summary>
        private void SpawnAttackEffects(int comboIndex)
        {
            Color effectColor = comboIndex switch
            {
                1 => Color.yellow,
                2 => new Color(1f, 0.5f, 0f),  // Orange
                3 => Color.red,
                _ => Color.white
            };

            Debug.Log($"🌟 Light Attack VFX - Combo {comboIndex}, Color: {effectColor}");
            // Instantiate particle effects at attack position
        }
    }
}
