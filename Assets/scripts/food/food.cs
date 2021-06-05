using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    private Vector2Int m_position;

    public void setPosition(Vector2Int _pos) 
    {
        m_position = _pos;
    }

    public void chararcterCollide(Collider2D collider) 
    {
        collider.GetComponent<charecterController>().eat(gameObject);
        foodSpawner spawner = GetComponentInParent<foodSpawner>();
        if (spawner != null) 
        {
            spawner.removeFood(m_position);
        }
        Destroy(gameObject);
    }
}
