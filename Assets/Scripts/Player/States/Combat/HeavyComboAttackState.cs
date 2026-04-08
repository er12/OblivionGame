using UnityEngine;
using Assets.Scripts.Player.States;

namespace Assets.Scripts.Player.States.Combat
{
    /// <summary>
    /// Heavy attack combo state for the player state machine.
    /// Slow but powerful attacks: Overhead → Smash → Crushing Blow
    /// Uses stamina resource.
    /// </summary>
    public class HeavyComboAttackState : ComboAttackStateBase
    {
        private float heavyDamage = 25f;
        private float knockbackForce = 10f;
        private float staminaCost = 20f;
        private float playerStamina = 100f;

        public HeavyComboAttackState(PlayerController player, PlayerStateMachine stateMachine) 
            : base(player, stateMachine)
        {
            // Setup heavy attack animations
            comboAnimations.Add("HeavyAttack1");   // Overhead
            comboAnimations.Add("HeavyAttack2");   // Smash
            comboAnimations.Add("HeavyAttack3");   // Crushing Blow

            comboDuration = 2.0f;      // Longer combo window
            maxComboCount = 3;
            attackDuration = 0.7f;     // Longer animation
            attackCooldown = 1.2f;     // Slower attacks
        }

        /// <summary>
        /// Enter state with stamina check.
        /// </summary>
        public override void Enter()
        {
            // Check if player has enough stamina
            if (playerStamina < staminaCost)
            {
                Debug.Log("❌ Not enough stamina for heavy attack!");
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }

            // Consume stamina
            playerStamina -= staminaCost;

            base.Enter();
        }

        /// <summary>
        /// Execute heavy attack logic.
        /// </summary>
        protected override void OnComboAttackStarted(int comboIndex)
        {
            float damage = heavyDamage * (1f + (comboIndex - 1) * 0.5f);

            Debug.Log($"Heavy Attack Combo {comboIndex}: Damage = {damage}");

            switch (comboIndex)
            {
                case 1: // Overhead
                    Debug.Log("⚒️ Overhead - Powerful strike!");
                    ApplyKnockback(knockbackForce);
                    break;

                case 2: // Smash
                    Debug.Log("⚒️ Smash - Very strong!");
                    ApplyKnockback(knockbackForce * 1.2f);
                    break;

                case 3: // Crushing Blow
                    Debug.Log("⚒️ Crushing Blow - Ultimate finisher!");
                    ApplyKnockback(knockbackForce * 1.8f);
                    ApplyStunEffect(0.5f);
                    break;
            }

            // Spawn visual effects
            SpawnAttackEffects(comboIndex);
        }

        /// <summary>
        /// Apply knockback and damage to enemies.
        /// </summary>
        private void ApplyKnockback(float force)
        {
            // Implement knockback on enemies here
            // Example: Apply force to enemy rigidbodies in range
            Debug.Log($"💥 Heavy Knockback applied: {force}");
        }

        /// <summary>
        /// Apply stun effect to enemies.
        /// </summary>
        private void ApplyStunEffect(float duration)
        {
            Debug.Log($"⏸️ Stun effect applied for {duration}s");
            // Implement status effect system
        }

        /// <summary>
        /// Spawn visual effects.
        /// </summary>
        private void SpawnAttackEffects(int comboIndex)
        {
            float effectScale = 1f + (comboIndex - 1) * 0.3f;

            Color effectColor = comboIndex switch
            {
                1 => new Color(1f, 0.7f, 0f),      // Orange
                2 => new Color(1f, 0.3f, 0f),      // Dark Orange
                3 => new Color(1f, 0f, 0f),        // Red
                _ => Color.white
            };

            Debug.Log($"🔥 Heavy Attack VFX - Combo {comboIndex}, Scale: {effectScale}, Color: {effectColor}");
            // Instantiate larger particle effects at attack position
        }

        /// <summary>
        /// Update stamina regeneration.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Regenerate stamina slowly
            playerStamina = Mathf.Min(playerStamina + Time.deltaTime * 15f, 100f);
        }

        /// <summary>
        /// Get current stamina percentage for UI.
        /// </summary>
        public float GetStaminaPercentage() => playerStamina / 100f;

        /// <summary>
        /// Reset combo and stamina on exit.
        /// </summary>
        public override void Exit()
        {
            base.Exit();
            ResetCombo();
        }
    }
}
