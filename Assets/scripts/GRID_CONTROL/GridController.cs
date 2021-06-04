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
    private Grid2D m_grid2D = null;
    private Vector2 m_middleOfSquareOffset = new Vector2(0.5f, 0.5f);
    private Vector2 m_entireGridOffset;

    const float SIZE_OF_SCANBOX = 0.75f;
    const string OBSTACLE_LAYER_NAME = "OBSTACLE";
    // Start is called before the first frame update
    void Start()
    { 
        m_leftupVector = gameObjectoGridVector2Int(m_leftUpCornerMarker);
        m_rightDownVector = gameObjectoGridVector2Int(m_rightDownCornerMarker);
        m_entireGridOffset = new Vector2(m_leftupVector.x, m_rightDownVector.y);
        m_grid2D = new Grid2D(m_leftupVector, m_rightDownVector);
        scanEnviormentAndSetGrid();




        Collider2D[] myHits = Physics2D.OverlapBoxAll(new Vector2(13,0), new Vector2(0.75f, 0.75f), 0, LayerMask.GetMask(OBSTACLE_LAYER_NAME));
        if(myHits.Length ==0)
        {
            Debug.LogError("Wtf");
        }
        else
        {
            Debug.LogError("Good");
        }




        m_txtContainer = new GameObject("debug text Container");
        m_txtContainer.transform.position = new Vector3(m_entireGridOffset.x, m_entireGridOffset.y,0);

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
        if(m_debugGraph)
        {
            debug_PrintGrid2D();
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
                GridUnitState state = scaningleSquareInEnviorment(new Vector2Int(i, j));

                m_grid2D.setGridUnitState(i,j,state);
            }
        }
        return true;
    }

    private GridUnitState scaningleSquareInEnviorment(Vector2Int location)
    {
        Collider2D[] myHits = Physics2D.OverlapBoxAll (location + m_middleOfSquareOffset, new Vector2(SIZE_OF_SCANBOX, SIZE_OF_SCANBOX),0,LayerMask.GetMask(OBSTACLE_LAYER_NAME));
        
if(myHits.Length > 0 )
        {
            Debug.Log("roiv not empty " + location);
            return GridUnitState.OBSTACLE;
        }
        Debug.Log("roiv  empty " + location);
        return GridUnitState.EMPTY;
    }



    public void debug_PrintGrid2D()
    {
        if(m_grid2D == null)
        {
            Debug.Log("m_grid2D is empty");
        }
        Vector2Int size = m_grid2D.getSizeVector();
        for(int i=0; i < size.x;i++)
        {
            for(int j=0; j < size.y;j++)
            {
                GridUnitState state = m_grid2D.getGridUnitState(i, j);
                createWorldText2D(state == GridUnitState.EMPTY ? "0" : "1",m_txtContainer.transform, new Vector2(i, j)+ m_middleOfSquareOffset, 50, state == GridUnitState.EMPTY ? Color.red :Color.green);
            }
        }
    }
}
