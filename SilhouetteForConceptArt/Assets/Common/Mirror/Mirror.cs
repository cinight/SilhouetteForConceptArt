using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Mirror : MonoBehaviour
{
    public int mirrorMode = 0;
    public Material mirrorMaterial;

    public void Update()
    {
        if(mirrorMaterial != null) mirrorMaterial.SetInt("_MirrorMode",mirrorMode);
    }
}
