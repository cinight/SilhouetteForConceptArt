using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvGen : MonoBehaviour
{
    public GameObject quad;
    public float guiScale = 1f;
    public int count = 3;
    private float randomPosition = 11f;
    public bool greyScale = false;
    public int layers = 5;

    private int countmax = 100;
    private MeshRenderer[] renderers;

    private List<Texture2D> tex_ground;
    private List<Texture2D> tex_tree;
    private List<Texture2D> tex_rock;

    private List<Texture2D> GetRandomType(int type = -1)
    {
        if(type == -1) type = Random.Range(1, 4);
        switch(type)
        {
            case 1: return tex_ground;
            case 2: return tex_tree;
            case 3: return tex_rock;
        }
        return tex_ground;
    }

    void Start()
    {
        tex_ground = new List<Texture2D>();
        tex_tree = new List<Texture2D>();
        tex_rock = new List<Texture2D>();

        //Sort the textures
        Object[] textures = Resources.LoadAll("EnvGen", typeof(Texture2D));
        for(int i=0; i<textures.Length; i++)
        {
            if(textures[i].name.Contains("ground")) tex_ground.Add( (Texture2D) textures[i] );
            if(textures[i].name.Contains("tree")) tex_tree.Add( (Texture2D) textures[i] );
            if(textures[i].name.Contains("rock")) tex_rock.Add( (Texture2D) textures[i] );
        }

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
                //LAYER

                int layer = Random.Range(1, layers);

                //MATERIAL

                //Random grey color
                float grey = 1f;
                //if(greyScale)
                //{
                    grey = Random.Range(0.7f, 1.0f);
                //}
                // else
                // {
                //     grey = 0f;
                // }
                grey *= (float)layer/layers; //apply layer color
                props.SetColor("_BaseColor", new Color(grey, grey, grey));

                //Random texture
                var type = GetRandomType();
                int tid = Random.Range(0,type.Count-1);
                props.SetTexture("_BaseMap",type[tid]);

                //Random cutoff
                // float cutoff = Random.Range(0.1f, 1.0f);
                // props.SetFloat("_Cutoff",cutoff);

                //Assign to MaterialPropertyBlock
                renderers[i].SetPropertyBlock(props);
                            
                // TRANSFORM

                //Random position
                var pos = Vector3.zero;
                pos.x = Random.Range(-randomPosition, randomPosition);
                pos.y = 0;
                pos.z = (float)layer/layers * 2f;
                renderers[i].transform.localPosition = pos;

                //Random rotation
                // Vector3 rot = CommonTools.RandomV3(Vector3.zero, Vector3.one*360.0f);
                // renderers[i].transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

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
        float h = 200 * guiScale;
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
        GUILayout.Label( "EnvGen",titleStyle );
        GUILayout.Space(20);

        //Settings
        GUILayout.BeginHorizontal();
            GUILayout.Label( "Layer     " );
            layers = Mathf.RoundToInt(GUILayout.HorizontalSlider(layers, 3, 10, GUILayout.Width(250 * guiScale)));
            GUILayout.Label( ""+layers );
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            GUILayout.Label( "Count     " );
            count = Mathf.RoundToInt(GUILayout.HorizontalSlider(count, 3, countmax, GUILayout.Width(250 * guiScale)));
            GUILayout.Label( ""+count );
        GUILayout.EndHorizontal();
        // GUILayout.BeginHorizontal();
        //     GUILayout.Label( "Scattering" );
        //     randomPosition = GUILayout.HorizontalSlider(randomPosition, 0f, 0.1f, GUILayout.Width(250 * guiScale));
        //     GUILayout.Label( ""+randomPosition.ToString("F3") );
        // GUILayout.EndHorizontal();
            greyScale = GUILayout.Toggle(greyScale, " Greyscale");
        GUILayout.Space(10);
        if(GUILayout.Button("\n Generate \n",GUILayout.Height(50 * guiScale))) GenNewShape();


        // End of box
        GUILayout.EndArea();
    }
}
