using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaIndicator : MonoBehaviour
{
    private void Update()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.transform.position;
        dir = new Vector3(dir.x, dir.y, 0);
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
