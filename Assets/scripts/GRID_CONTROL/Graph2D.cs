using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph2D 
{
    bool m_cancleDiagBetweenTwo = false;
    Grid2D m_gridRef = null;
    public Node2D[,] m_nodes;




    // Start is called before the first frame update
    public Graph2D(Grid2D grid2D)
    {
        m_gridRef = grid2D;
        Vector2Int size = grid2D.getSizeVector();
        m_nodes = new Node2D[size.x, size.y];
        for (int i=0; i < size.x;i++)
        {
            for(int j=0; j < size.y; j++)
            {
                if(m_gridRef.getGridUnitState(i, j) == GridUnitState.EMPTY)
                {
                    Node2D node = new Node2D(new Vector2Int(i, j));
                    m_nodes[i,j] = node;
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
    public bool visited = false;
    int hValues;
    List<Edge2D> m_edges = new List<Edge2D>();
    public Node2D(Vector2Int cords)
    {
        m_cords  = cords;
    }
}

public class Node2dWithAstarScore
{
    int score;
    Node2D m_node;

}
public class Edge2D
{

}

