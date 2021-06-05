using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public charecterController m_controller;
    public GridController m_gridController;
    public foodSpawner m_spawner;
    bool nadav = true;

    private Vector2 m_diretion = Vector2.zero;
    private Vector2 m_target = Vector2.zero;
    private int m_nodeIdx = 0;
    private List<Node2D> m_path = new List<Node2D>();

    private void getNextDirection() 
    {
        if (m_nodeIdx >= m_path.Count) 
        {
            return;
        }
        Node2D node = m_path[m_nodeIdx];
        Vector2Int targetNode = m_gridController.convectGridToGameWorld(node.m_cords);
        m_target = new Vector2(targetNode.x + 0.5f, targetNode.y + 0.5f);
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        m_diretion = (m_target - position).normalized;
        Debug.Log("direction " + m_diretion);
    }

    private void getNewPath() 
    {
        Vector2Int target = m_spawner.getRandomFood();
        Vector2Int position = m_gridController.convertGameWorldToGridCords((int)transform.position.x, (int)transform.position.y);
        List<Node2D> path = m_gridController.m_graph2D.startPathFinding(position, target);
        if (path == null) 
        {
            return;
        }
        m_path = path;
        Debug.Log(m_path.Count);
        m_nodeIdx = 0;
        getNextDirection();
    }

    private void FixedUpdate()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        if (m_nodeIdx >= m_path.Count) 
        {
            m_controller.Move(Vector2.zero);
            getNewPath();
            Debug.Log("new path");
            return;
        }
        m_controller.Move(m_diretion);
        //Debug.Log(Vector2.Distance(position, m_target));
        if (Vector2.Distance(position, m_target) < 0.2) 
        {
            ++m_nodeIdx;
            getNextDirection();
        }
    }
}
