using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charecterController : MonoBehaviour
{
    public float m_speed = 4.0f;

    private Rigidbody2D m_rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private float m_MovementSmoothing = 0.05f;

    private Vector2 m_orientation = Vector2.right;

    private int m_equipedItem;
    private GameObject m_heldItem;

    private Collider2D m_collider;



    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void changeOrientation(Vector2 direction)
    {
        Vector2 orientation;
        if (direction == Vector2.zero)
        {
            //don't change orientation when stop moving
            return;
        }
        else if (direction.x > 0)
        {
            orientation = Vector2.right;
        }
        else if (direction.x < 0)
        {
            orientation = Vector2.left;
        }
        else if (direction.y > 0)
        {
            orientation = Vector2.up;
        }
        else
        {
            //default to face down
            orientation = Vector2.down;
        }

        // guarantee to set orientation only if changed 
        if (orientation != m_orientation)
        {
            m_orientation = orientation;
        }
    }

    public void Move(Vector2 _direction)
    {
        //m_animator.move(_direction);
        changeOrientation(_direction);
        Vector3 targetVelocity = new Vector2(_direction.x * m_speed, _direction.y * m_speed);
        m_rigidbody2D.velocity = Vector3.SmoothDamp(m_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }
}
