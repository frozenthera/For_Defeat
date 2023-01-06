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

    }

    public void OperateExit()
    {

    }
    public void OperateUpdate()
    {
     
    }
}
