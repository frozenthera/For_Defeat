using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : IState
{
    private PlayerController player;
    private Vector3 targetPosition;
    private float accelatedSpeed;
    private SpriteRenderer playerSprite;
    public PlayerMove(PlayerController player)
    {
        this.player = player;
        playerSprite = player.GetComponent<SpriteRenderer>();
    }
    public void OperateEnter()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = player.transform.position.z;
        accelatedSpeed = player.PlayerSpeed;
    }
    public void OperateExit()
    {
        
    }
    public void OperateUpdate()
    {
        //Input에 따른 이동
        if(Input.GetMouseButton(1))
        {
            accelatedSpeed = player.PlayerSpeed;
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = player.transform.position.z;
            if ((targetPosition.x - player.transform.position.x) < 0)
            {
                playerSprite.flipX = false;
            }
            else
            {
                playerSprite.flipX = true;
            }
        }
        accelatedSpeed += player.PlayerAccel * Time.deltaTime;
        player.transform.position += accelatedSpeed * (targetPosition - player.transform.position).normalized * Time.deltaTime;

        if((targetPosition-player.transform.position).magnitude < player.PlayerMoveError)
        {
            player.UpdateState(PlayerController.PlayerState.Wait);
        }
    }
}
