using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : IState
{
    private HeroBehaviour hero;
    private bool isAttackable = false;
    public HeroAttack(HeroBehaviour hero)
    {
        this.hero = hero;
    }

    public void OperateEnter()
    {
        Debug.Log("AttackEnter");
        GameManager.Instance.StartCoroutine(EAttackBDelay());
    }

    public void OperateExit()
    {

    }
    public void OperateUpdate()
    {
        if(isAttackable)
        {
            Debug.Log("Attack!");
            GameManager.Instance.StartCoroutine(EAttackADelay());
            isAttackable = false;
        }
    }

    private IEnumerator EAttackBDelay()
    {
        float BDelay = hero.HeroNormalBDelay;
        while(BDelay >= 0)
        {
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
            ADelay -= Time.deltaTime;
            yield return null;
        }
        hero.UpdateState(HeroBehaviour.HeroState.Move);
    }
}
