using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashObject : MonoBehaviour
{
    public GameObject origin;
    public GameObject target;
    public float damage;
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
            PC.GetDamage(damage);
            Destroy(this.gameObject);
        }
    }
    
}
