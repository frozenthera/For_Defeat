using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCast : IState
{
    private HeroBehaviour hero;

    public HeroCast(HeroBehaviour hero)
    {
        this.hero = hero;
    }

    public void OperateEnter()
    {
        GameManager.Instance.StartCoroutine(ECheckDone());
    }

    public void OperateExit()
    {

    }
    public void OperateUpdate()
    {
     
    }

    private IEnumerator ECheckDone()
    {
        yield return hero.skillList[0].StartCoroutine(hero.skillList[0].OnSkillActive());
        hero.UpdateState(HeroBehaviour.HeroState.Move);
    }
}
