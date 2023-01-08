using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroKnuckBack : IState
{
    private HeroBehaviour hero;
    public Vector3 KnuckBackDist;
    public float KnuckBackTime;
    private float leftKnuckBackTime;
    public HeroKnuckBack(HeroBehaviour hero)
    {
        this.hero = hero;
    }

    public void OperateEnter()
    {
        hero.isInKnuckBack = true;
        leftKnuckBackTime = KnuckBackTime;
        Debug.Log("knuckback enter : " + leftKnuckBackTime + ", " + KnuckBackDist.magnitude);
    }

    public void OperateExit()
    {
        hero.isInKnuckBack = false;
    }
    public void OperateUpdate()
    {
        if(leftKnuckBackTime <= 0 || !hero.isInKnuckBack) hero.UpdateState(HeroBehaviour.HeroState.Move);
        hero.transform.position += KnuckBackDist * Time.deltaTime;
        leftKnuckBackTime -= Time.deltaTime;
    }
}
