using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineShaders : MonoBehaviour
{
    public List<Material> materials;
    public float cutOffOffset;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Material material in GetComponent<MeshRenderer>().materials)
        {
            materials.Add(material);
        }
        //materials[1].SetTexture("_MainText", materials[0].GetTexture("_MainText"));
        //materials[1].SetTexture("_AlphaText", materials[0].GetTexture("_MaskText"));
        //materials[1].SetVector("_AlphaTextTiling", materials[0].GetVector("_MaskTextTiling"));
        //materials[1].SetVector("_AlphaTextOffset", materials[0].GetVector("_MaskTextOffset"));
        if(materials[0].GetFloat("_VertexFrequency") != 0)
        {
            materials[1].SetFloat("_MovementFrequency", materials[0].GetFloat("_VertexFrequency"));
            materials[1].SetVector("_Movement", materials[0].GetVector("_VertexAmount"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        materials[0].SetFloat("_CutOff", materials[1].GetFloat("_CutOff") + cutOffOffset);
    }
}
