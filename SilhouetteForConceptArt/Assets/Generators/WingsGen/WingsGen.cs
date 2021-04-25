using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsGen : MonoBehaviour
{
    public Mirror mirror;
    public GameObject quad;
    public float guiScale = 1f;
    //public bool colored = false;

    private List<GameObject> objs;
    private Object[] textures;
    private Camera cam;

    private GameObject obj;
    private Vector3 startPosition;
    private int texID = 0;

    void Start()
    {
        cam = Camera.main;
        textures = Resources.LoadAll("WingsGen", typeof(Texture2D));
        objs = new List<GameObject>();
    }

    private Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            pos = hit.point;
        }
        return pos;
    }

    public void OnMouseDown()
    {
        //Start position
        startPosition = mirrorComensationPos();

        //Make new object
        Vector3 pos = startPosition;
        pos.z = 0.001f * objs.Count;
        obj = Instantiate(quad,pos,Quaternion.identity);
        MeshRenderer ren = obj.GetComponentInChildren<MeshRenderer>();

        //Material
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        //props.SetColor("_BaseColor", colored? Color.white : Color.black );
        props.SetTexture("_BaseMap",(Texture2D)textures[texID]);
        ren.SetPropertyBlock(props);
    }

    public void OnMouseDrag()
    {
        Vector3 mousePos = mirrorComensationPos();
        Debug.DrawLine(cam.transform.position,mousePos,Color.red);

        //Scale base on distance
        float dist = Vector3.Distance(startPosition,mousePos);
        obj.transform.localScale = Vector3.one*dist;

        //Rotation base on direction
        float angle = AngleBetweenTwoPoints(startPosition,mousePos);
        obj.transform.rotation = Quaternion.Euler(0,0,angle-90f);
    }

    private Vector3 mirrorComensationPos()
    {
        Vector3 pos = GetMousePosition();
        if(mirror.mirrorMode == 1)
        {
            pos.x = Mathf.Abs(pos.x);
            pos.x *= 2f;
            pos.x -= 10.23f;
            pos.y *= 2f;
        }
        Debug.Log(pos.y);
        return pos;
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) 
    {
    return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void OnMouseUp()
    {
        objs.Add(obj);
    }

    public void Undo()
    {
        int id = objs.Count-1;
        if(id >= 0)
        {
            Destroy(objs[id]);
            objs.Remove(objs[id]);
        }
    }

    public void Clear()
    {
        for(int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }
        objs.Clear();
    }

    void OnGUI()
    {
        if(!CommonTools.renderGUI) return;

        //The box
        float hTextureSlot = 80f;
        float w = 200 * guiScale;
        float h = (220 + hTextureSlot * textures.Length ) * guiScale;
        GUILayout.BeginArea(new Rect(5, 5, w, h), GUI.skin.box);

        //Styles
        GUI.skin.label.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.button.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.toggle.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.color = Color.white;

        //Title
        GUIStyle titleStyle = new GUIStyle("label");
        titleStyle.fontSize = Mathf.RoundToInt(GUI.skin.label.fontSize*1.2f);
        titleStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label( "WingsGen",titleStyle );
        GUILayout.Space(20);

        //Settings
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("\n Undo \n",GUILayout.Height(50 * guiScale))) Undo();
        if(GUILayout.Button("\n Clear \n",GUILayout.Height(50 * guiScale))) Clear();
        GUILayout.EndHorizontal();
        if(GUILayout.Button("\n Toggle Mirror \n",GUILayout.Height(50 * guiScale))) mirror.mirrorMode = mirror.mirrorMode==0? 1 : 0;
        //if(GUILayout.RepeatButton("\n Wings Preview \n",GUILayout.Height(50 * guiScale))) mirror.mirrorMode = 1;
        //else mirror.mirrorMode = 0;
        GUILayout.Space(10);

        //List of textures below
        GUILayout.Label( "Select feather:" );
        for(int i = 0; i < textures.Length; i++)
        {
            if(GUILayout.Button((Texture2D)textures[i],GUILayout.Height(hTextureSlot * guiScale))) texID = i;
        }
        
        //colored = GUILayout.Toggle(colored, " Colored");

        // End of box
        GUILayout.EndArea();
    }
}
