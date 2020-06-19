using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShapeGen : MonoBehaviour
{
    public GameObject quad;
    public float guiScale = 1f;
    public int count = 3;
    public float randomPosition = 0.05f;
    public bool greyScale = false;

    private int countmax = 100;
    private MeshRenderer[] renderers;
    private Object[] texture_shapes;

    void Start()
    {
        texture_shapes = Resources.LoadAll("Textures/Shapes", typeof(Texture2D));
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
                renderers[i].transform.localPosition = RandomV3(Vector3.one*randomPosition*-i, Vector3.one*randomPosition*i);

                //Random rotation
                Vector3 rot = RandomV3(Vector3.zero, Vector3.one*360.0f);
                renderers[i].transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

                //Random scale
                renderers[i].transform.localScale = RandomV3(Vector3.one*0.5f, Vector3.one*5.5f);

                //Make sure Renderer is enabled
                renderers[i].enabled = true;
            }
            else
            {
                renderers[i].enabled = false;
            }
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

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    public IEnumerator ExportPNG()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture(1);
        byte[] textureBytes = texture.EncodeToJPG();

        string fileName = "SilhouetteForConceptArt_"+System.DateTime.Now.ToString("yyyyMMdd_hh-mm-ss")+".JPG";
        string dataString = "data:image/jpg;base64," + System.Convert.ToBase64String(textureBytes);
        //ScreenCapture.CaptureScreenshot(fileName);

        #if !UNITY_EDITOR
        openWindow(dataString);
        #endif

        Destroy(texture);
    }

    void OnGUI()
    {
        //The box
        float w = 400 * guiScale;
        float h = 160 * guiScale;
        GUILayout.BeginArea(new Rect(5, 5, w, h), GUI.skin.box);

        //Styles
        GUI.skin.label.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.button.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.toggle.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.skin.horizontalSlider.fontSize = Mathf.RoundToInt ( 16 * guiScale );
        GUI.color = Color.white;

        //Settings
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("\n Generate \n",GUILayout.Height(50 * guiScale))) GenNewShape();
            if(GUILayout.Button("\n Download \n", GUILayout.Height(50 * guiScale))) StartCoroutine(ExportPNG());
            //if(GUILayout.Button("\n Show In Explorer \n", GUILayout.Height(50 * guiScale))) ShowInExplorer();
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
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


        // End of box
        GUILayout.EndArea();
    }
}
