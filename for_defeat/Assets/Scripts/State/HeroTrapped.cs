using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTrapped : IState
{
    private HeroBehaviour hero;
    private PlayerController player;
    private bool isAttackable = false;
    public TrapObject trapObject;
    private bool isContinuable = true;
    public HeroTrapped(HeroBehaviour hero)
    {
        this.hero = hero;
        player = GameManager.Instance.player;
    }

    public void OperateEnter()
    {
        isAttackable = false;
        isContinuable = true;
        hero.curHP -= 100;
        player.curHP += 100;
    }

    public void OperateExit()
    {

    }
    public void OperateUpdate()
    {
        if(isAttackable)
        {
            //trapObject.GetDamage(1);
            hero.StartCoroutine(EAttackADelay());
            isAttackable = false;
            if(trapObject.elapsedTime <= 0) 
            {
                GameObject.Destroy(trapObject.gameObject);
                isContinuable = false;
                Debug.Log("Trap -> Move");
                hero.isInTrap = false;
                hero.UpdateState(HeroBehaviour.HeroState.Move);
            }
        }
        else
        {
            hero.StartCoroutine(EAttackBDelay());
        }
    }

    private IEnumerator EAttackBDelay()
    {
        float BDelay = hero.HeroNormalBDelay;
        while(BDelay >= 0)
        {
            if(!isContinuable) yield break;
            BDelay -= Time.deltaTime;
            yield return null;
        }
        isAttackable = true;
    }

    private IEnumerator EAttackADelay()
    {
        float ADelay = hero.HeroNormalADelay;
        while(ADelay >= 0)
        {
            if(!isContinuable) yield break;
            ADelay -= Time.deltaTime;
            yield return null;
        }
    }
}