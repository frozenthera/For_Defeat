using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerErosion : Skill
{
   public GameObject ErosionObject;
   [SerializeField] private float DOTLastingTime; 
   [SerializeField] private float DOTDamagePerSec;
   [SerializeField] private float RadiusMultiplier;
   [SerializeField] private float damage;

    public override IEnumerator _OnSkillActive()
    {
         int AngerStep = (int)(GameManager.Instance.player.CurAngerGauge / 333) + 1;
        //TODO : apply damage when first casted
        Collider2D coll = Physics2D.OverlapCircle(origin.transform.position, RadiusMultiplier * (AngerStep + 1));
        if(coll != null && coll.CompareTag("Hero"))
        {
            coll.GetComponent<HeroBehaviour>().GetDamage(damage);
        }
        ErosionObject EO = Instantiate(ErosionObject, origin.transform.position, Quaternion.identity).GetComponent<ErosionObject>();
        EO.lastTime = DOTLastingTime * (AngerStep + 1);
        EO.damagePerSec = DOTDamagePerSec * (AngerStep + 1);
        EO.transform.localScale = Vector3.forward + new Vector3(1, 1, 0) * RadiusMultiplier * (AngerStep + 1);
        yield return null;
    }

    public override float CalcDamage()
    {
        return skilldamage;
    }
}
