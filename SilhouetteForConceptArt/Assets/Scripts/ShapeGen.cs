using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGen : MonoBehaviour
{
    public GameObject quad;
    public float guiScale = 1f;
    [Range(3,20)] public int count = 3;
    [Range(0f,5f)] public float randomPosition = 1.0f;

    private MeshRenderer[] renderers;
    private Object[] texture_shapes;

    void Start()
    {
        texture_shapes = Resources.LoadAll("Textures/Shapes", typeof(Texture2D));
        renderers = new MeshRenderer[count];
        for(int i=0; i<count; i++)
        {
            GameObject newObj = Instantiate(quad);
            renderers[i] = newObj.GetComponent<MeshRenderer>();
        }
        GenNewShape();
    }

    public void GenNewShape()
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        for(int i=0; i<count; i++)
        {
            //MATERIAL

            //Random grey color
            float grey = Random.Range(0.0f, 1.0f);
            //props.SetColor("_BaseColor", new Color(grey, grey, grey));
            props.SetColor("_BaseColor", Color.black);

            //Random texture
            int tid = Random.Range(0,texture_shapes.Length-1);
            props.SetTexture("_BaseMap",(Texture2D)texture_shapes[tid]);

            //Random cutoff
            float cutoff = Random.Range(0.1f, 1.0f);
            props.SetFloat("_Cutoff",cutoff);

            //Assign to MaterialPropertyBlock
            renderers[i].SetPropertyBlock(props);
                        
            // TRANSFORM

            //Random position
            renderers[i].transform.localPosition = RandomV3(Vector3.one*randomPosition*-1.0f, Vector3.one*randomPosition);

            //Random rotation
            Vector3 rot = RandomV3(Vector3.zero, Vector3.one*360.0f);
            renderers[i].transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

            //Random scale
            renderers[i].transform.localScale = RandomV3(Vector3.one*0.5f, Vector3.one*3.5f);
        }
    }

    private Vector3 RandomV3(Vector3 rmin, Vector3 rmax)
    {
        Vector3 v;
        v.x = Random.Range(rmin.x, rmax.x);
        v.y = Random.Range(rmin.y, rmax.y);
        v.z = Random.Range(rmin.z, rmax.z);
        return v;
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.color = Color.white;
        float w = 410 * guiScale;
        float h = 90 * guiScale;
        GUILayout.BeginArea(new Rect(Screen.width - w -5, Screen.height - h -5, w, h), GUI.skin.box);

        GUILayout.BeginHorizontal();
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = GUI.skin.label.fontSize;
        if(GUILayout.Button("\n Generate \n",customButton,GUILayout.Width(200 * guiScale), GUILayout.Height(50 * guiScale))) GenNewShape();
        //if(GUILayout.Button("\n Export PNG \n",customButton,GUILayout.Width(200 * guiScale), GUILayout.Height(50 * guiScale))) NextScene();
        GUILayout.EndHorizontal();

        //int currentpage = SceneManager.GetActiveScene().buildIndex;
        //int totalpages = SceneManager.sceneCountInBuildSettings-1;
        //GUILayout.Label( currentpage + " / " + totalpages + " " + SceneManager.GetActiveScene().name );

        GUILayout.EndArea();
    }
}
