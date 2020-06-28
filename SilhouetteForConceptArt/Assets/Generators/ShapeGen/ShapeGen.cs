using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGen : MonoBehaviour
{
    public GameObject quad;
    public float guiScale = 1f;
    public int count = 3;
    public float randomPosition = 0.05f;
    public bool greyScale = false;
    public Mirror mirror;

    private int countmax = 100;
    private MeshRenderer[] renderers;
    private Object[] texture_shapes;

    void Start()
    {
        texture_shapes = Resources.LoadAll("ShapeGen", typeof(Texture2D));
        renderers = new MeshRenderer[countmax];
        for(int i=0; i<countmax; i++)
        {
            GameObject newObj = Instantiate(quad);
            renderers[i] = newObj.GetComponent<MeshRenderer>();
        }
        GenNewShape();
    }

    public void GenNewShape()
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        for(int i=0; i<countmax; i++)
        {
            if(i<count)
            {
                //MATERIAL

                //Random grey color
                if(greyScale)
                {
                    float grey = Random.Range(0.0f, 0.3f);
                    props.SetColor("_BaseColor", new Color(grey, grey, grey));
                }
                else
                {
                    props.SetColor("_BaseColor", Color.black);
                }

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
                renderers[i].transform.localPosition = CommonTools.RandomV3(Vector3.one*randomPosition*-i, Vector3.one*randomPosition*i);

                //Random rotation
                Vector3 rot = CommonTools.RandomV3(Vector3.zero, Vector3.one*360.0f);
                renderers[i].transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

                //Random scale
                renderers[i].transform.localScale = CommonTools.RandomV3(Vector3.one*0.5f, Vector3.one*5.5f);

                //Make sure Renderer is enabled
                renderers[i].enabled = true;
            }
            else
            {
                renderers[i].enabled = false;
            }
        }
    }

    void OnGUI()
    {
        if(!CommonTools.renderGUI) return;

        //The box
        float w = 400 * guiScale;
        float h = 230 * guiScale;
        GUILayout.BeginArea(new Rect(5, 5, w, h), GUI.skin.box);

        //Styles
        GUI.skin.label.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.button.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.toggle.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.horizontalSlider.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.color = Color.white;

        //Title
        GUIStyle titleStyle = new GUIStyle("label");
        titleStyle.fontSize = Mathf.RoundToInt(GUI.skin.label.fontSize*1.2f);
        titleStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label( "ShapeGen",titleStyle );
        GUILayout.Space(20);

        //Settings
        GUILayout.BeginHorizontal();
            GUILayout.Label( "Count     " );
            count = Mathf.RoundToInt(GUILayout.HorizontalSlider(count, 3, countmax, GUILayout.Width(250 * guiScale)));
            GUILayout.Label( ""+count );
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            GUILayout.Label( "Scattering" );
            randomPosition = GUILayout.HorizontalSlider(randomPosition, 0f, 0.1f, GUILayout.Width(250 * guiScale));
            GUILayout.Label( ""+randomPosition.ToString("F3") );
        GUILayout.EndHorizontal();
            greyScale = GUILayout.Toggle(greyScale, " Greyscale");
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("No Mirror")) mirror.mirrorMode = 0;
            if(GUILayout.Button("MirrorLeft")) mirror.mirrorMode = 1;
            if(GUILayout.Button("MirrorRight")) mirror.mirrorMode = 2;
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        if(GUILayout.Button("\n Generate \n",GUILayout.Height(50 * guiScale))) GenNewShape();


        // End of box
        GUILayout.EndArea();
    }
}
