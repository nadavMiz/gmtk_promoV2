using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public GameObject m_leftUpCornerMarker;
    public GameObject m_rightDownCornerMarker;
    private Vector2Int m_leftupVector;
    private Vector2Int m_rightDownVector;
    public bool m_debugGrid;
    public bool m_debugGraph;
    private Grid2D m_grid2D;
    // Start is called before the first frame update
    void Start()
    { 
        m_leftupVector = gameObjectoGridVector2Int(m_leftUpCornerMarker);
        m_rightDownVector = gameObjectoGridVector2Int(m_rightDownCornerMarker);
        m_grid2D = new Grid2D(m_leftupVector, m_rightDownVector);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// takes a game object and gives its location grid form [Vector2Int].
    /// any changes needed to fit grid will happen here.
    /// </summary>
    /// <returns></returns>
    public Vector2Int gameObjectoGridVector2Int(GameObject gameObject)
    {
        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        return new Vector2Int(Mathf.FloorToInt(x),Mathf.FloorToInt(y));
    }
    public bool scanEnviormentAndSetGrid()
    {
        int x = m_leftupVector.x;
        int y = m_rightDownVector.y;
        for(int i = x; i < m_rightDownVector.x; i++)
        {
            for(int j = y; j < m_leftupVector.y; j++)
            {
                scanSquareEnviorment(new Vector2Int(i, j));
            }
        }
        return true;
    }

    private GridUnitState scanSquareEnviorment(Vector2Int vector2Int)
    {
        throw new NotImplementedException();
    }
}
