using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGen : MonoBehaviour
{
    public GameObject quad;
    public float guiScale = 1f;
    [Range(3,100)] public int count = 3;
    [Range(0f,0.1f)] public float randomPosition = 0.05f;
    public bool greyScale = false;

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
            renderers[i].transform.localPosition = RandomV3(Vector3.one*randomPosition*-i, Vector3.one*randomPosition*i);

            //Random rotation
            Vector3 rot = RandomV3(Vector3.zero, Vector3.one*360.0f);
            renderers[i].transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

            //Random scale
            renderers[i].transform.localScale = RandomV3(Vector3.one*0.5f, Vector3.one*5.5f);
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

    public void ExportPNG()
    {
        string fileName = "SilhouetteForConceptArt_"+System.DateTime.Now.ToString("yyyyMMdd_hh-mm-ss")+".PNG";
        ScreenCapture.CaptureScreenshot(fileName);
    }

    public void ShowInExplorer()
    {
        string folderPath = Application.dataPath;
        folderPath = folderPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select,"+folderPath);
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.color = Color.white;
        float w = 350 * guiScale;
        float h = 200 * guiScale;
        GUILayout.BeginArea(new Rect(5, 5, w, h), GUI.skin.box);

        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = GUI.skin.label.fontSize;
        if(GUILayout.Button("\n Generate \n",customButton,GUILayout.Height(50 * guiScale))) GenNewShape();
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("\n Export PNG \n",customButton, GUILayout.Height(50 * guiScale))) ExportPNG();
            if(GUILayout.Button("\n Show In Explorer \n",customButton, GUILayout.Height(50 * guiScale))) ShowInExplorer();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            GUILayout.Label( "Scattering" );
            randomPosition = GUILayout.HorizontalSlider(randomPosition, 0f, 0.1f);
        GUILayout.EndHorizontal();
            greyScale = GUILayout.Toggle(greyScale, "Greyscale");

        GUILayout.EndArea();
    }
}
