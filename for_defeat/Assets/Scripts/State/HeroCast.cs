using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCast : IState
{
    private HeroBehaviour hero;
    public int skillIdx;
    public HeroCast(HeroBehaviour hero)
    {
        this.hero = hero;
    }

    public void OperateEnter()
    {
        hero.StartCoroutine(ECheckDone());
    }

    public void OperateExit()
    {

    }
    public void OperateUpdate()
    {
     
    }

    private IEnumerator ECheckDone()
    {
        hero.skillList[skillIdx].origin = hero.gameObject;
        hero.skillList[skillIdx].target = GameManager.Instance.player.gameObject;
        yield return hero.skillList[skillIdx].StartCoroutine(hero.skillList[skillIdx].OnSkillActive());
        hero.UpdateState(HeroBehaviour.HeroState.Move);
    }
}
