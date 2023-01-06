using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWait : IState
{
    private PlayerController player;
    public PlayerWait()
    {
        player = GameManager.Instance.player;
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
            player.UpdateState(PlayerController.PlayerState.Move);
        }
    }
}
