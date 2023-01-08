using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour
{
    public int trapHP;
    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.transform.CompareTag("Hero"))
        {
            HeroBehaviour HB = coll.GetComponent<HeroBehaviour>();
            if(HB.isImmune)
            {
                Destroy(gameObject);   
            }
            else
            {
                Debug.Log("Trapped!");
                HB.UpdateState(HeroBehaviour.HeroState.Trapped, gameObject);
            }
        }
    }

    public void GetDamage(int damage)
    {
        trapHP -= damage;
        Debug.Log(trapHP);
        if(trapHP <= damage) Destroy(gameObject);
    }
}