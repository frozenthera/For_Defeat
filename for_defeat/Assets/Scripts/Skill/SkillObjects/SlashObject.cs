using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashObject : MonoBehaviour
{
    public GameObject origin;
    public GameObject target;
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
    
}
