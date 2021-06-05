using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charecterController : MonoBehaviour
{
    [System.Serializable]
    public struct Stage 
    {
        public ulong m_growthRate;
        public Sprite m_sprite;
    }
    public List<Stage> m_stages;

    static private ulong FULL_GROWTH_SIZE = 10;
    static private ulong GROWTH_RATE = 1;
    static private float ATTACK_MODE_TIME = 10.0f;

    public float m_speed = 4.0f;

    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider;
    private Vector3 m_Velocity = Vector3.zero;
    private float m_MovementSmoothing = 0.05f;
    private int m_stageIdx;
    private CharecterAnimation m_animationController;

    private Vector2 m_orientation = Vector2.right;
    private bool m_isAttackMode = false;

    [SerializeField] private ulong m_size;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        m_animationController = GetComponent<CharecterAnimation>();
    }

    public void Move(Vector2 _direction)
    {
        m_animationController.move(_direction);
        changeOrientation(_direction);
        Vector3 targetVelocity = new Vector2(_direction.x * m_speed, _direction.y * m_speed);
        m_rigidbody2D.velocity = Vector3.SmoothDamp(m_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public void eat(GameObject obj) 
    {
        m_size += GROWTH_RATE;
        while (m_stageIdx < m_stages.Count && m_size >= m_stages[m_stageIdx].m_growthRate) 
        {
            ++m_stageIdx;
            grow();
        }
        if (m_size >= FULL_GROWTH_SIZE) 
        {
            startAttackMode();
        }
        m_animationController.setLevel((int)m_size);

        Debug.Log("num num " + m_size);
    }

    public bool isAttackMode() { return m_isAttackMode; }

    public int getLevel() 
    {
        return (int)m_size;
    }

    public void stun() 
    {
        Debug.Log("I am stuned");
    }

    public void getHit() 
    {
        Debug.Log("I am hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.SendMessage("chararcterCollide", m_collider);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        charecterController other = collision.gameObject.GetComponent<charecterController>();
        if (other == null || m_isAttackMode) 
        {
            return;
        }

        if (other.isAttackMode()) 
        {
            getHit();
            return;
        }

        if (other.getLevel() > getLevel()) 
        {
            stun();
        }
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

    private void grow() { /*TODO need animations*/}

    private void endAttackMode() 
    {
        m_isAttackMode = false;
        m_size = 0L;
        m_animationController.setLevel((int)m_size);
        Debug.Log("attackMode end");
    }

    private void startAttackMode() 
    {
        m_isAttackMode = true;
        Invoke("endAttackMode", ATTACK_MODE_TIME);
        Debug.Log("attackMode bahhhh");
    }
}
