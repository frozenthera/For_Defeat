using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : IState
{
    private HeroBehaviour hero;
    public PlayerController player;
    
    public HeroMove(HeroBehaviour hero)
    {
        this.hero = hero;
        player = GameManager.Instance.player;
    }

    public void OperateEnter()
    {
        Debug.Log("MoveEnter");
    }

    public void OperateExit()
    {

    }

    public void OperateUpdate()
    {
        Vector3 moveVec = player.transform.position - hero.transform.position;
        if(moveVec.magnitude <= hero.HeroRecogRad)
        {
            hero.UpdateState(HeroBehaviour.HeroState.Attack);
            return;
        } 
        hero.transform.position += hero.HeroSpeed * moveVec.normalized * Time.deltaTime;
    }
}
