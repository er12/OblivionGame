using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Player.States;

namespace Assets.Scripts.Player.States.Combat
{
    /// <summary>
    /// Base class for combo attack states in the state machine.
    /// Handles combo counting, timing, and animation transitions.
    /// </summary>
    public abstract class ComboAttackStateBase : PlayerStateBase
    {
        // Combo configuration
        protected float comboDuration = 1.5f;           // Time window for next attack in combo
        protected int maxComboCount = 3;                // Maximum combo attacks in sequence
        protected List<string> comboAnimations;         // Animation names for each combo level
        protected float attackDuration = 0.5f;          // How long the attack animation plays

        // Combo tracking
        protected int currentComboCount = 0;
        protected float lastAttackTime = 0f;
        protected float stateEnterTime = 0f;
        protected float attackCooldown = 0.5f;

        public ComboAttackStateBase(PlayerController player, PlayerStateMachine stateMachine) 
            : base(player, stateMachine)
        {
            comboAnimations = new List<string>();
        }

        /// <summary>
        /// Called when entering this attack state.
        /// </summary>
        public override void Enter()
        {
            stateEnterTime = Time.time;

            // Check if combo chain is still active
            if (Time.time - lastAttackTime > comboDuration)
            {
                currentComboCount = 0;
            }

            // Increment combo count
            currentComboCount = Mathf.Min(currentComboCount + 1, maxComboCount);
            lastAttackTime = Time.time;

            // Stop movement during attack
            player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);

            // Play combo animation
            PlayComboAnimation();

            Debug.Log($"{GetType().Name}: Combo {currentComboCount} started");
        }

        /// <summary>
        /// Update state every frame.
        /// </summary>
        public override void Update()
        {
            // Check if attack animation finished
            if (Time.time - stateEnterTime >= attackDuration)
            {
                // Transition based on input or back to idle
                HandleStateTransition();
            }
        }

        /// <summary>
        /// Fixed update for physics.
        /// </summary>
        public override void FixedUpdate()
        {
            // Keep player still horizontally during attack
            player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);
        }

        /// <summary>
        /// Called when exiting this state.
        /// </summary>
        public override void Exit()
        {
            player.animator.SetBool("IsAttacking", false);
            Debug.Log($"{GetType().Name}: Exited attack state");
        }

        /// <summary>
        /// Play the appropriate combo animation.
        /// </summary>
        protected virtual void PlayComboAnimation()
        {
            if (comboAnimations.Count == 0)
            {
                Debug.LogWarning($"{GetType().Name}: No animations configured!");
                return;
            }

            int animIndex = Mathf.Min(currentComboCount - 1, comboAnimations.Count - 1);
            string animName = comboAnimations[animIndex];

            player.animator.SetBool("IsAttacking", true);
            player.animator.SetInteger("ComboCount", currentComboCount);
            player.animator.SetTrigger(animName);

            // Call attack-specific logic
            OnComboAttackStarted(currentComboCount);
        }

        /// <summary>
        /// Handle state transitions after attack finishes.
        /// </summary>
        protected virtual void HandleStateTransition()
        {
            if (Mathf.Abs(player.moveInput) > PlayerController.movementThreshold)
                stateMachine.ChangeState(stateMachine.walkingState);
            else
                stateMachine.ChangeState(stateMachine.idleState);
        }

        /// <summary>
        /// Called when combo attack is executed. Override in derived classes.
        /// </summary>
        protected abstract void OnComboAttackStarted(int comboIndex);

        /// <summary>
        /// Get current combo count.
        /// </summary>
        public int GetComboCount() => currentComboCount;

        /// <summary>
        /// Reset combo chain.
        /// </summary>
        public virtual void ResetCombo()
        {
            currentComboCount = 0;
            lastAttackTime = 0f;
        }
    }
}
