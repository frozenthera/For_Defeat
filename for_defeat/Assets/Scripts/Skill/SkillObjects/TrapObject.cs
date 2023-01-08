using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour
{
    public float elapsedTime;
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
                elapsedTime = 2f;
                Debug.Log("Trapped!");
                GameManager.Instance.hero.isInTrap = true;
                HB.transform.position = this.transform.position;
                HB.UpdateState(HeroBehaviour.HeroState.Trapped, gameObject);
            }
        }
    }

    private void Update()
    {
        elapsedTime -= Time.deltaTime;
    }

    public void GetDamage(int damage)
    {
        trapHP -= damage;
        if(trapHP <= 0) Destroy(this.gameObject);
    }
}