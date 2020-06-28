using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class CommonTools : MonoBehaviour
{
    public static bool renderGUI = true;
    public float scale = 1f;
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        NextScene();
    }

    void OnGUI()
    {
        if(!renderGUI) return;

        GUI.skin.label.fontSize = Mathf.RoundToInt ( 16 * scale );
        GUI.color = new Color(1, 1, 1, 1);
        float w = 150 * scale;
        float h = 110 * scale;
        GUILayout.BeginArea(new Rect(Screen.width - w -5, Screen.height - h -5, w, h), GUI.skin.box);

        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = GUI.skin.label.fontSize;
        if(GUILayout.Button("\n Download Image \n", customButton,GUILayout.Height(50 * scale))) StartCoroutine(ExportPNG());
        if(GUILayout.Button("\n Switch Generator \n",customButton,GUILayout.Height(50 * scale))) NextScene();

        //int currentpage = SceneManager.GetActiveScene().buildIndex;
        //int totalpages = SceneManager.sceneCountInBuildSettings-1;
        //GUILayout.Label( currentpage + " / " + totalpages + " " + SceneManager.GetActiveScene().name );

        GUILayout.EndArea();
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    public IEnumerator ExportPNG()
    {
        renderGUI = false;
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture(1);
        byte[] textureBytes = texture.EncodeToJPG();

        string fileName = "SilhouetteForConceptArt_"+System.DateTime.Now.ToString("yyyyMMdd_hh-mm-ss")+".JPG";
        string dataString = "data:image/jpg;base64," + System.Convert.ToBase64String(textureBytes);

        #if !UNITY_EDITOR
        openWindow(dataString);
        #else
        System.IO.File.WriteAllBytes(Application.dataPath + fileName, textureBytes);
        #endif

        Destroy(texture);
        renderGUI = true;
    }

    public void NextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(sceneIndex + 1);
        else
            SceneManager.LoadScene(1);
    }

    // public void PrevScene()
    // {
    //     int sceneIndex = SceneManager.GetActiveScene().buildIndex;

    //     if (sceneIndex > 1)
    //         SceneManager.LoadScene(sceneIndex - 1);
    //     else
    //         SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    // }

    //=================================================================================
    public static Vector3 RandomV3(Vector3 rmin, Vector3 rmax)
    {
        Vector3 v;
        v.x = Random.Range(rmin.x, rmax.x);
        v.y = Random.Range(rmin.y, rmax.y);
        v.z = Random.Range(rmin.z, rmax.z);
        return v;
    }
}