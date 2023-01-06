using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIdle : IState
{
    private HeroBehaviour hero;

    public HeroIdle(HeroBehaviour hero)
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
     
    }
}
