using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDissolve : MonoBehaviour
{
    public List<Material> materials;
    public float cutOffOffset;
    public float cutOffSpeed = 0.1f;
    public float currentSpeed;
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
        if(materials[0].HasProperty("_VertexFrequency"))
        {
            materials[1].SetFloat("_MovementFrequency", materials[0].GetFloat("_VertexFrequency"));
            materials[1].SetVector("_Movement", materials[0].GetVector("_VertexAmount"));
        }
        currentSpeed = cutOffSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (materials.Count == 1)
        {
            if (materials[0].GetFloat("_CutOff") >= 1)
            {
                currentSpeed = -cutOffSpeed;
            }
            else if (materials[0].GetFloat("_CutOff") <= -1)
            {
                currentSpeed = cutOffSpeed;
            }
            materials[0].SetFloat("_CutOff", materials[0].GetFloat("_CutOff") + (Time.deltaTime * currentSpeed));
        }
        else
        {
            if (materials[1].GetFloat("_CutOff") + cutOffOffset >= 1)
            {
                currentSpeed = -cutOffSpeed;
            }
            else if (materials[1].GetFloat("_CutOff") + cutOffOffset <= -1)
            {
                currentSpeed = cutOffSpeed;
            }

            materials[1].SetFloat("_CutOff", materials[1].GetFloat("_CutOff") + (Time.deltaTime * currentSpeed));
            materials[0].SetFloat("_CutOff", (materials[1].GetFloat("_CutOff") + cutOffOffset));
        }

    }
}
