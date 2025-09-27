using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        player.Rigidbody.velocity = new Vector2(movement.x * player.moveSpeedInHUB, movement.y * player.moveSpeedInHUB);

    }

    // Update is called once per frame
    void Update()
    {

        movement.x = player.move.ReadValue<Vector2>().x;
        movement.y = player.move.ReadValue<Vector2>().y;
        if (movement.x != 0)
        {
            if (movement.x > 0 && !player.isPlayerFacingRight)
            {
                PlayerFlipped.Invoke();
            }
            else if (movement.x < 0 && player.isPlayerFacingRight)
            {
                PlayerFlipped.Invoke();
            }
        }
        else if (movement.x == 0 && movement.y == 0)
        {
            player.TransitionToState(player.PlayerIdleInHUBState);
        }

    }
    
    public override void FixUpdate(PlayerController player)
    {
        player.Rigidbody.velocity = new Vector2(movement.x * player.moveSpeedInHUB, movement.y * player.moveSpeedInHUB);
    }
}
