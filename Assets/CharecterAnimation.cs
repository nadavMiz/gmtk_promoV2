﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterAnimation : MonoBehaviour
{
    private Animator m_animator;
    private Vector2 m_lastDircetion;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void move(Vector2 _direction)
    {
        Debug.Log(_direction);
        if (_direction == Vector2.zero) 
        {
            m_animator.SetBool("isMoving", false);
            return;
        }
        if (System.Math.Sign(_direction.x) != System.Math.Sign(m_lastDircetion.x)) 
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.y);
        }
        m_animator.SetBool("isMoving", true);
        m_animator.SetFloat("speedHorizontal", _direction.x);
        m_animator.SetFloat("speedVertical", _direction.y);
        m_lastDircetion = _direction;
    }
}
