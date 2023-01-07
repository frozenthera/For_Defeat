using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWait : IState
{
    private PlayerController player;
    public PlayerWait(PlayerController player)
    {
        this.player = player;
    }
    public void OperateEnter()
    {

    }
    public void OperateExit()
    {
        
    }
    public void OperateUpdate()
    {
        if(Input.GetMouseButtonDown(1))
        {
            player.UpdateState(PlayerController.EPlayerState.Move);
        }
    }
}
