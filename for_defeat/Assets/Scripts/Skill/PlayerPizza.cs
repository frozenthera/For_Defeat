using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPizza : Skill
{
   public GameObject PizzaObject;

    public override IEnumerator _OnSkillActive()
    {
        Debug.Log("Pizza!");
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
