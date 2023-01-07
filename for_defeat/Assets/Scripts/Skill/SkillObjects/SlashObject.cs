using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashObject : MonoBehaviour
{
    public GameObject origin;
    public GameObject target;
    public float damage;
    public float angerGaugeGain;
    [SerializeField] private float speed;
    private Vector3 dir;

    private void Start()
    {
        dir = (target.transform.position - origin.transform.position).normalized;
    }

    private void Update()
    {
        transform.position += speed * dir * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        // Debug.Log("collision"); 
        if(coll.transform.CompareTag("Player"))
        {
            // Debug.Log("Slash Hit!");
            PlayerController PC = coll.transform.GetComponent<PlayerController>();
            Debug.Log("damaged by slash");
            PC.GetDamage(damage, angerGaugeGain);
            Destroy(this.gameObject);
        }
    }
    
}
