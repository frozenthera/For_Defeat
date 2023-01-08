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
            if((hero.transform.position - GameManager.Instance.player.transform.position).magnitude <= hero.HeroRecogRad)
            {
                Debug.Log("damaged by normal attack");
                GameManager.Instance.player.GetDamage(hero.HeroNormalAttackDamage, hero.NormalAttackGaugeGain);
            } 
            isAttackable = false;
            GameManager.Instance.StartCoroutine(EAttackADelay());
        }
    }

    private IEnumerator EAttackBDelay()
    {
        isAttackable = false;
        hero.isInDelay = true;
        hero.heroAttackCircle.transform.position = hero.transform.position;
        hero.heroAttackCircle.SetActive(true);
        Debug.Log("Show Circle!");
        float BDelay = hero.HeroNormalBDelay;
        while(BDelay >= 0)
        {
            if(hero.isInKnuckBack)
            {
                hero.isInDelay = false;
                hero.heroAttackCircle.SetActive(false);        
                yield break;
            }
            // Debug.Log(BDelay);
            hero.heroAttackCircle.transform.position = hero.transform.position;
            BDelay -= Time.deltaTime;
            yield return null;
        }
        isAttackable = true;
        hero.isInDelay = false;
        hero.heroAttackCircle.SetActive(false);
    }

    private IEnumerator EAttackADelay()
    {
        hero.isInDelay = true;
        float ADelay = hero.HeroNormalADelay;
        while(ADelay >= 0)
        {
            if(hero.isInKnuckBack) yield break;
            ADelay -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Attack->Move");
        hero.isInDelay = false;
        hero.UpdateState(HeroBehaviour.HeroState.Move);
    }
}
