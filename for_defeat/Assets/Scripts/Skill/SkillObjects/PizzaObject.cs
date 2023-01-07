using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaObject : MonoBehaviour
{
    public float DOTDamge;
    public float DOTLastTime;
    private float curTime = 0f;
    private void Start()
    {
        Destroy(gameObject, DOTLastTime);
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.transform.CompareTag("Hero"))
        {
            if(curTime < 0) coll.transform.GetComponent<HeroBehaviour>().GetDamage(DOTDamge);
            curTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.transform.CompareTag("Hero"))
        {
            curTime = 0.5f;
        }
    }
}
