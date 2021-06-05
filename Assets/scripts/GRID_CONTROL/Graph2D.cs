using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph2D
{
    bool m_cancleDiagBetweenTwo = false;
    Grid2D m_gridRef = null;
    public Node2D[,] m_nodes;
    SortedSet<Node2D> m_notVisited;







    // Start is called before the first frame update
    public Graph2D(Grid2D grid2D)
    {
        createNodesFromGrid(grid2D);
    }

    private void createNodesFromGrid(Grid2D grid2D)
    {
        m_notVisited = new SortedSet<Node2D>();
        m_gridRef = grid2D;
        Vector2Int size = grid2D.getSizeVector();
        m_nodes = new Node2D[size.x, size.y];
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (m_gridRef.getGridUnitState(i, j) == GridUnitState.EMPTY)
                {
                    Node2D node = new Node2D(new Vector2Int(i, j));
                    m_nodes[i, j] = node;
                }
                else
                {
                    m_nodes[i, j] = null;
                }
            }
        }

    }










    //will return end node that should have the values going back to create a path.



public List<Node2D> startPathFinding(Vector2Int startNodeCords, Vector2Int endNodeCords)
    {
        Node2D starting_node = m_nodes[startNodeCords.x, startNodeCords.y];
        Node2D end_node = m_nodes[endNodeCords.x, endNodeCords.y];

        m_notVisited = new SortedSet<Node2D>();
        if(end_node == null)
        {
            return null;
        }
        calculateAllHvalues(end_node);

        Node2D currentNode = starting_node;
        currentNode.visited = true;
        currentNode.gValue = 0;
        while (currentNode != end_node)
        {
            //Debug.LogError("Current Node = " + currentNode.m_cords);
            Node2D neighborNode;
            bool cancledByEdges = false;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {

                        if (m_gridRef.checkValidCordinates(currentNode.m_cords.x + i, currentNode.m_cords.y + j) && m_nodes[currentNode.m_cords.x + i, currentNode.m_cords.y + j] != null)
                        {
                            neighborNode = m_nodes[currentNode.m_cords.x + i, currentNode.m_cords.y + j];

                            if (Mathf.Abs(i) == Mathf.Abs(j))
                            {
                                if(m_nodes[currentNode.m_cords.x , currentNode.m_cords.y + j] == null || m_nodes[currentNode.m_cords.x+i, currentNode.m_cords.y ]==null)
                                {
                                    cancledByEdges = true;
                                    //Debug.Log("edges cancles "+ currentNode.m_cords + "i ="+i + "j="+j);
                                }
                            }



                            if ((neighborNode.fValue == -1) && (!neighborNode.visited) && (!cancledByEdges))
                            {
                                neighborNode.gValue = currentNode.gValue + getValueOfVertex(i, j);
                                neighborNode.fValue = neighborNode.gValue + neighborNode.hValue;
                                neighborNode.previousNode = currentNode;
                                m_notVisited.Add(neighborNode);
                               //Debug.LogError("Added Node = " + neighborNode.m_cords);

                            }
                            else if ((currentNode.gValue + getValueOfVertex(i, j) < neighborNode.gValue) && (!neighborNode.visited) && (!cancledByEdges))
                            {
                                neighborNode.gValue = currentNode.gValue + getValueOfVertex(i, j);
                                neighborNode.fValue = neighborNode.gValue + neighborNode.hValue;
                                neighborNode.previousNode = currentNode;
                                m_notVisited.Add(neighborNode);
                                //Debug.LogError("Added Node = " + neighborNode.m_cords);

                            }
                            cancledByEdges = false;
                        }
                    }
                }
            }
            currentNode = m_notVisited.Min;
            if(m_notVisited.Count ==0)
            {
                Debug.LogError("dead end");
                return null;
            }
            m_notVisited.Remove(currentNode);
            currentNode.visited = true;
            
        }
        List<Node2D> pathList = new List<Node2D>();
        while (currentNode!=null)
        {
            pathList.Add(currentNode);
            Debug.Log("cords ("+currentNode.m_cords.x+","+ currentNode.m_cords.y+")");
            currentNode = currentNode.previousNode;
        }
        pathList.Reverse();
        return pathList;

    }
    public int getValueOfVertex(int i, int j)
    {
        if (Mathf.Abs(i) == Mathf.Abs(j))
        {
            return 14;
        }
        else return 10;
    }
    public void calculateAllHvalues(Node2D endNode)
    {
        Vector2Int size = m_gridRef.getSizeVector();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (m_nodes[i, j] != null)
                {
                    m_nodes[i, j].calcHscore(endNode);
                }
            }

        }




    }
}
    
public class Node2D: System.IComparable<Node2D>
{
    public Vector2Int m_cords;
    public bool visited = false;
    public Node2D previousNode = null;
    public int fValue = -1 ;
    public int gValue = -1;
    public int hValue = -1;
    public Node2D(Vector2Int cords)
    {
        m_cords  = cords;
    }
    public void calcHscore(Node2D other)
    {
            if(other == null)
            {
                Debug.LogError("calcing distance to not existing node");
            return;
            }
            calcHscore(other.m_cords);
    }
    public void calcHscore(Vector2Int other)
    {


            int distanceX = Mathf.Abs(m_cords.x - other.x);
            int distanceY = Mathf.Abs(m_cords.y - other.y);
            hValue = (distanceX > distanceY ? (distanceY * 14 + (distanceX - distanceY)*10) : (distanceX * 14 + (distanceY - distanceX))*10);

     }
        public int CompareTo(Node2D other)
    {
        if (fValue < other.fValue)
            return -1;
        else if (fValue > other.fValue)
            return 1;
        if (other.fValue == fValue)
        {
            if(m_cords.x < other.m_cords.x)
            {
                return -1;
            }
            else if (m_cords.x > other.m_cords.x)
            {
                return 1;
            }
            else if(m_cords.x == other.m_cords.x)
            {
                if (m_cords.y < other.m_cords.y)
                {
                    return -1;
                }
                else if (m_cords.y > other.m_cords.y)
                {
                    return 1;
                }
            }
        }
        return 0;
        
    }
}

public class Node2dWithAstarScore
{
    int score;
    Node2D m_node;

}
