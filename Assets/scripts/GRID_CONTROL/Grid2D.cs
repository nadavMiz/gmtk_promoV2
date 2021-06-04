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




    public  Grid2D(Vector2Int leftUpCorner, Vector2Int rightDownCorner)
    {
        m_sizeX = rightDownCorner.x - leftUpCorner.x;
        m_sizeY = leftUpCorner.y - rightDownCorner.y;
        if(m_sizeX <= 0|| m_sizeY <= 0)
        {
            Debug.LogError("Grid2D, init with bad values check leftUpcorner and rightDownCorner");
            return;
        }
        m_gridStateMatrix = new GridUnitState[m_sizeX, m_sizeY];
        for(int i=0; i < m_sizeX; i++)
        {
            for(int j=0; j<m_sizeY; j++)
            {
                m_gridStateMatrix[i,j] = GridUnitState.EMPTY;
            }
        }
    }
    /// <summary>
    /// sets gridUnit state success = return true
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="gu"></param>
    /// <returns></returns>
    public bool setGridUnitState(int x, int y, GridUnitState gus)
    {
        if (!checkValidCordinates(x, y))
        {
            return false;
        }
        m_gridStateMatrix[x, y] = gus;
        return true;
    }
    public GridUnitState getGridUnitState(int x,int y )
    {
        if (!checkValidCordinates(x,y))
        {
            Debug.LogError("getGridUnitState got bad position");
            return GridUnitState.OBSTACLE;
        }
        return m_gridStateMatrix[x, y];
    }
    public GridUnitState getGridUnitState(Vector2Int pos)
    {
        if (!checkValidCordinates(pos.x, pos.y))
        {
            Debug.LogError("getGridUnitState got bad position");
            return GridUnitState.OBSTACLE;
        }
        return m_gridStateMatrix[pos.x, pos.y];
    }
    public Vector2Int getSizeVector()
    {
        return new Vector2Int(m_sizeX, m_sizeY);
    }

    public bool checkValidCordinates(int x, int y)
    {
        if((x < 0 || x >= m_sizeX) || (y < 0 || y >= m_sizeY))
        {
            return false;
        }
        return true;
    }
}
public enum GridUnitState
{
    EMPTY,
    OBSTACLE,
}
