using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMove : IState
{
    private HeroBehaviour hero;
    public PlayerController player;
    private SpriteRenderer heroSprite;
    
    public HeroMove(HeroBehaviour hero)
    {
        this.hero = hero;
        player = GameManager.Instance.player;
        heroSprite = hero.GetComponent<SpriteRenderer>();
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
        if(moveVec.x < 0)
        {
            heroSprite.flipX = false;
        }
        else
        {
            heroSprite.flipX = true;
        }
        
        Collider2D coll = Physics2D.OverlapCircle(hero.transform.position, hero.HeroRecogRad);
        if(coll.CompareTag("Player"))
        {
            hero.UpdateState(HeroBehaviour.HeroState.Attack);
            return;
        } 
        Vector3 temp = hero.transform.position += hero.HeroSpeed * moveVec.normalized * Time.deltaTime;
        hero.transform.position = new Vector3(temp.x, temp.y, -2);
    }
}
