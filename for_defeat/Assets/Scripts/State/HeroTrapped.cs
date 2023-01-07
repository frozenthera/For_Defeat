using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTrapped : IState
{
    private HeroBehaviour hero;
    private bool isAttackable = false;
    public TrapObject trapObject;
    private bool isContinuable = true;
    public HeroTrapped(HeroBehaviour hero)
    {
        this.hero = hero;
    }

    public void OperateEnter()
    {
        
    }

    public void OperateExit()
    {

    }
    public void OperateUpdate()
    {
        if(isAttackable)
        {
            trapObject.GetDamage(1);
            hero.StartCoroutine(EAttackADelay());
            isAttackable = false;
            if(trapObject.trapHP == 1) 
            {
                isContinuable = false;
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