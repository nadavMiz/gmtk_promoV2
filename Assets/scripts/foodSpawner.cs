using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class foodSpawner : MonoBehaviour
{
    static private int MAX_FOOD_COUNT = 10;
    public GridController m_gridController;
    public GameObject m_foodPrefab;
    private Grid2D m_grid;
    private int m_gridWidth;
    private int m_gridHeight;
    private float m_TimeBetweenSpawns = 2f;
    private float m_lastTime;
    private int m_foodId = 0;
    private Dictionary<int, Vector2Int> m_foods = new Dictionary<int, Vector2Int>();

    private void Start()
    {
        m_grid = m_gridController.m_grid2D;
        Vector2 gridSize = m_grid.getSizeVector();
        m_gridWidth = (int)gridSize.x;
        m_gridHeight = (int)gridSize.y;
        m_lastTime = Time.time;
    }

    public void removeFood(int _id) 
    {
        m_foods.Remove(_id);
    }

    public Vector2Int getRandomFood() 
    {
        if (m_foods.Count == 0) 
        {
            return getEmptyCoordinate();
        }
        int idx = Random.Range(0, m_foods.Count);
        return m_foods.Values.ElementAt(idx);
    }

    private void FixedUpdate()
    {
        if (/*(m_foods.Count < MAX_FOOD_COUNT) &&*/ (Time.time > m_lastTime + m_TimeBetweenSpawns))
        {
            Vector2Int coordinates = getEmptyCoordinate();
            Vector2Int worldCoordinates = m_gridController.convectGridToGameWorld(coordinates);
            Vector3 worldPosition = new Vector3((float)worldCoordinates.x + 0.5f, (float)worldCoordinates.y + 0.5f, 0);
            SpawnFood(worldPosition, m_foodId);
            m_foods.Add(m_foodId, coordinates);
            ++m_foodId;
            m_lastTime = Time.time;
        } 
        
    }

    private Vector2Int getEmptyCoordinate() 
    {
        Vector2Int res;
        do
        {
            int x = Random.Range(0, m_gridWidth);
            int y = Random.Range(0, m_gridHeight);

            res = new Vector2Int(x, y);
        } while (m_grid.getGridUnitState(res.x, res.y) == GridUnitState.OBSTACLE);

        return res;
    }

    private void SpawnFood(Vector3 worldCoordinates, int id) 
    {
        GameObject obj = GameObject.Instantiate(m_foodPrefab, worldCoordinates, Quaternion.identity, transform);
        obj.GetComponent<food>().setId(id);
    }
}
