using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  
/// </summary>
public class Grid2D 
{
    private int m_sizeX;
    private int m_sizeY;
    Vector2Int m_leftUpCorner;
    Vector2Int m_rightDownCorner;
    GridUnitState[,] m_gridStateMatrix;

    Grid2D(Vector2Int leftUpCorner, Vector2Int rightDownCorner)
    {
        m_sizeX = rightDownCorner.x - leftUpCorner.x;
        m_sizeY = leftUpCorner.y - rightDownCorner.y;
        if(m_sizeX <= 0|| m_sizeY <= 0)
        {
            Debug.LogError("Grid2D, init with bad values check leftUpcorner and rightDownCorner");
            return;
        }
        m_gridStateMatrix = new GridUnitState[m_sizeX, m_sizeY];
    }
}
public enum GridUnitState
{
    Empty,
    Occupied
}
