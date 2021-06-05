using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    private int m_id;

    public void setId(int id) 
    {
        m_id = id;
    }

    public void chararcterCollide(Collider2D collider) 
    {
        collider.GetComponent<charecterController>().eat(gameObject);
        foodSpawner spawner = GetComponentInParent<foodSpawner>();
        if (spawner != null) 
        {
            spawner.removeFood(m_id);
        }
        Destroy(gameObject);
    }
}
