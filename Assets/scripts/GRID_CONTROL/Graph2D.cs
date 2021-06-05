﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph2D 
{
    bool m_cancleDiagBetweenTwo = false;
    Grid2D m_gridRef = null;
    public List<Node2D> m_nodes = new List<Node2D>();
   
    // Start is called before the first frame update
    public Graph2D(Grid2D grid2D)
    {
        m_gridRef = grid2D;
        Vector2Int size = grid2D.getSizeVector();
        for (int i=0; i < size.x;i++)
        {
            for(int j=0; j < size.y; j++)
            {
                if(m_gridRef.getGridUnitState(i, j) == GridUnitState.EMPTY)
                {
                    Node2D node = new Node2D(new Vector2Int(i, j));
                    m_nodes.Add(node);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }







}
public class Node2D
{
    public Vector2Int m_cords;
    List<Edge2D> m_edges = new List<Edge2D>();
    public Node2D(Vector2Int cords)
    {
        m_cords  = cords;
    }
}
public class Edge2D
{

}