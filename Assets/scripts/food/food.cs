using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{
    public void chararcterCollide(Collider2D collider) 
    {
        collider.GetComponent<charecterController>().eat(gameObject);
        Destroy(gameObject);
    }
}
