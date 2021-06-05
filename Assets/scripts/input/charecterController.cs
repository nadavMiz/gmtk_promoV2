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
    static private float STUN_TIME = 2f;

    public float m_speed = 4.0f;

    private Rigidbody2D m_rigidbody2D;
    private Collider2D m_collider;
    private Vector3 m_Velocity = Vector3.zero;
    private float m_MovementSmoothing = 0.05f;
    private CharecterAnimation m_animationController;

    private Vector2 m_orientation = Vector2.right;
    private bool m_isAttackMode = false;
    private bool m_isStuned = false;

    private Vector2 m_moveToDirection = Vector2.zero;
    private Vector2 m_moveToPosition;
    private bool m_isMoveTo = false;

    [SerializeField] private ulong m_size;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        m_animationController = GetComponent<CharecterAnimation>();
    }


    private void FixedUpdate()
    {
        if (m_isMoveTo) 
        {
            Move(m_moveToDirection);
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            if (Vector2.Distance(position, m_moveToPosition) < 0.2) 
            {
                m_isMoveTo = false;
            }
        }
    }
    public void Move(Vector2 _direction)
    {
        if (m_isStuned) 
        {
            _direction = Vector2.zero;
        }
        m_animationController.move(_direction);
        changeOrientation(_direction);
        Vector3 targetVelocity = new Vector2(_direction.x * m_speed, _direction.y * m_speed);
        m_rigidbody2D.velocity = Vector3.SmoothDamp(m_rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    public void MoveTo(Vector2 _target) 
    {
        m_moveToPosition = _target;
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        m_moveToDirection = (_target - position).normalized;
        m_isMoveTo = true;
    }

    public void eat(GameObject obj) 
    {
        if (m_size == FULL_GROWTH_SIZE) 
        {
            return;
        }
        m_size += GROWTH_RATE;
        Debug.Log("num num " + m_size);
        if (m_size >= FULL_GROWTH_SIZE) 
        {
            startAttackMode();
        }
        m_animationController.setLevel((int)m_size);
    }

    public bool isAttackMode() { return m_isAttackMode; }

    public int getLevel() 
    {
        return (int)m_size;
    }

    public void stun() 
    {
        m_isStuned = true;
        m_animationController.stun(true);
        Invoke("endStunMode", STUN_TIME);
        Debug.Log("I am stuned");
    }

    public void getHit() 
    {
        m_animationController.initiateDeath();
        Debug.Log("I am hit");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        charecterController other = collision.GetComponent<charecterController>();
        if (other != null) 
        {
            checkCollition(other);
            return;
        }
        collision.SendMessage("chararcterCollide", m_collider);
    }

    private void checkCollition(charecterController other)
    {
        if (m_isAttackMode) 
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

    private void endStunMode() 
    {
        m_isStuned = false;
        m_animationController.stun(false);
    }

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
