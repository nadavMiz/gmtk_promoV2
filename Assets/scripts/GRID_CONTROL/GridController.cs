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
    GameObject m_txtContainer;
    public Grid2D m_grid2D = null;
    private Vector2 m_middleOfSquareOffset = new Vector2(0.5f, 0.5f);
    private Vector2 m_entireGridOffset;

    private Graph2D m_graph2D;     




    const float SIZE_OF_SCANBOX = 0.75f;
    const string OBSTACLE_LAYER_NAME = "OBSTACLE";




    void Awake()
    {
        m_leftupVector = gameObjectoGridVector2Int(m_leftUpCornerMarker);
        m_rightDownVector = gameObjectoGridVector2Int(m_rightDownCornerMarker);
        m_entireGridOffset = new Vector2(m_leftupVector.x, m_rightDownVector.y);
        m_grid2D = new Grid2D(m_leftupVector, m_rightDownVector);
        scanEnviormentAndSetGrid();
        m_graph2D = new Graph2D(m_grid2D);
        //m_graph2D.startPathFinding(new Vector2Int(0, 0), new Vector2Int(8, 0));



        m_txtContainer = new GameObject("debug text Container");
        m_txtContainer.transform.position = new Vector3(m_entireGridOffset.x, m_entireGridOffset.y, 0);
    }
    // Start is called before the first frame update
    void Start()
    {


    }

    public Vector2Int convectGridToGameWorld(Vector2Int cords)
    {
        if(!m_grid2D.checkValidCordinates(cords.x,cords.y))
        {
            Debug.LogError("convectGridToGameWorld bad cords");
            return new Vector2Int(-1, -1);
        }
        
        return new Vector2Int(Mathf.FloorToInt(m_entireGridOffset.x+cords.x), Mathf.FloorToInt(m_entireGridOffset.y)+cords.y); ;
    }


    public TextMesh createWorldText2D(string text, Transform parent, Vector2 localPosition, int fontSize, Color color)
    {

        GameObject gameObject = new GameObject("world_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        gameObject.transform.localScale = new Vector2(0.1f, 0.1f);
        transform.SetParent(parent, true);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.GetComponent<MeshRenderer>().sortingLayerName = "aboveAll";

        return textMesh;
    }




    /*

        public TextMesh createWorldText2D(string text, Transform parent, Vector2 localPosition, int fontSize, Color color)
        {

            GameObject gameObject = new GameObject("world_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            gameObject.transform.localScale = new Vector2(0.1f, 0.1f);
            transform.SetParent(parent, true);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.text = text;
            textMesh.alignment = TextAlignment.Center;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.GetComponent<MeshRenderer>().sortingLayerName = "aboveAll";

            return textMesh;
        }

        */

    // Update is called once per frame
    void Update()
    {
        if (m_debugGrid)
        {
            debug_PrintGrid2D();
            m_debugGrid = !m_debugGrid;
        }
        if(m_debugGraph)
        {
            debug_PrintGraph2D();
            m_debugGraph = !m_debugGraph;
        }
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
        return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
    }
    public bool scanEnviormentAndSetGrid()
    {
        int x = m_leftupVector.x;
        int y = m_rightDownVector.y;
        for (int i = x; i < m_rightDownVector.x; i++)
        {
            for (int j = y; j < m_leftupVector.y; j++)
            {
                GridUnitState state = scaningleSquareInEnviorment(new Vector2Int(i, j));
                Vector2Int gridCords = convertGameWorldToGridCords(i, j);
                m_grid2D.setGridUnitState(gridCords.x, gridCords.y, state);
            }
        }
        return true;
    }
    public Vector2Int convertGameWorldToGridCords(int x, int y)
    {
        if ((x < m_leftupVector.x) || (x > m_rightDownVector.x))
            return new Vector2Int(-1, -1);
        if ((y < m_rightDownVector.y) || (y > m_leftupVector.y))
            return new Vector2Int(-1, -1);
        return new Vector2Int(Mathf.FloorToInt(x - m_leftupVector.x), Mathf.FloorToInt(y - m_rightDownVector.y));
    }
    private GridUnitState scaningleSquareInEnviorment(Vector2Int realWorldSquare)
    {
        Collider2D[] myHits = Physics2D.OverlapBoxAll(realWorldSquare + m_middleOfSquareOffset, new Vector2(SIZE_OF_SCANBOX, SIZE_OF_SCANBOX), 0, LayerMask.GetMask(OBSTACLE_LAYER_NAME));
        if (myHits.Length > 0)
        {
            return GridUnitState.OBSTACLE;
        }
        return GridUnitState.EMPTY;
    }



    public void debug_PrintGrid2D()
    {
        if (m_grid2D == null)
        {
            Debug.Log("m_grid2D is empty");
        }
        Vector2Int size = m_grid2D.getSizeVector();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                GridUnitState state = m_grid2D.getGridUnitState(i, j);
                createWorldText2D(state == GridUnitState.EMPTY ? "0" : "1", m_txtContainer.transform, new Vector2(i, j) + m_middleOfSquareOffset, 50, state == GridUnitState.EMPTY ? Color.red : Color.green);
            }
        }
    }



    public void debug_PrintGraph2D()
    {
        if (m_graph2D == null )
        {
            Debug.Log("m_graph2D is empty");
        }
        Vector2Int size = m_grid2D.getSizeVector();
        foreach( Node2D node in m_graph2D.m_nodes)
        {
            if(node!=null)
                createWorldText2D("0", m_txtContainer.transform, new Vector2(node.m_cords.x, node.m_cords.y) + m_middleOfSquareOffset, 50, Color.white );

        }

    }
    
}
