using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterMap : MonoBehaviour
{
    Texture3D texture3D;
    public Transform cube;
    //public GameObject point;
    public Texture2D splat;

    Vector3 gridSize = new Vector3(40, 40, 40);
    Vector3 gridExtents;

    // Start is called before the first frame update
    void Start()
    {
        gridExtents = new Vector3(gridSize.x / 2, gridSize.y / 2, gridSize.z / 2);
        texture3D = new Texture3D((int)gridSize.x, (int)gridSize.y, (int)gridSize.z, TextureFormat.ARGB32, false);
        int pixelCount = (int)gridSize.x * (int)gridSize.y * (int)gridSize.z;
        Color[] cols = new Color[pixelCount];

        for (int i = 0; i < (int)gridSize.x; i++)//x
            for (int j = 0; j < (int)gridSize.y; j++)//y
                for (int k = 0; k < (int)gridSize.z; k++)//z
                    cols[i + j * (int)gridSize.x + k * (int)gridSize.y * (int)gridSize.z] = new Color(0, 0, 0, 1);

        //x
        for (int i = 0; i < (int)gridSize.x; i++)
            //y
            for (int j = 0; j < (int)gridSize.y; j++)
                //z
                for (int k = 0; k < (int)gridSize.z; k++)
                {
                    //top left
                    if (i >= 0 && i <= 5 && k >= 35 && k <= 40)
                        cols[i + j * (int)gridSize.x + k * (int)gridSize.y * (int)gridSize.z] = new Color(1, 1, 1, 1);
                    //top right
                    else if (i >= 35 && i <= 40 && k >= 35 && k <= 40)
                        cols[i + j * (int)gridSize.x + k * (int)gridSize.y * (int)gridSize.z] = new Color(1, 1, 1, 1);
                    //bottom left
                    else if (i >= 0 && i <= 5 && k >= 0 && k <= 5)
                        cols[i + j * (int)gridSize.x + k * (int)gridSize.y * (int)gridSize.z] = new Color(1, 1, 1, 1);
                    //bottom right
                    else if (i >= 35 && i <= 40 && k >= 0 && k <= 5)
                        cols[i + j * (int)gridSize.x + k * (int)gridSize.y * (int)gridSize.z] = new Color(1, 1, 1, 1);
                    //everywhere else
                    else
                        cols[i + j * (int)gridSize.x + k * (int)gridSize.y * (int)gridSize.z] = new Color(0, 0, 0, 1);
                }

        texture3D.SetPixels(cols);
        texture3D.Apply();

        MeshRenderer ren = GetComponent<MeshRenderer>();
        ren.material.SetTexture("_voxels", texture3D);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = cube.transform.position;
            Vector3 pixelPosition = new Vector3(position.x - gridExtents.x, position.y - gridExtents.y, position.z - gridExtents.z);
            for (int i = (int)pixelPosition.x - 3; i < (int)pixelPosition.x + 3; i++)
            {
                //y
                for (int j = (int)pixelPosition.y - 3; j < (int)pixelPosition.y + 3; j++)
                {
                    //z
                    for (int k = (int)pixelPosition.z - 3; k < (int)pixelPosition.z + 3; k++)
                    {
                        Vector3 pixelLocation = position;
                        texture3D.SetPixel((int)i, (int)j, (int)k, new Color(1, 1, 1, 1));
                        //point.transform.position = pixelLocation;
                        Debug.Log(pixelLocation);
         
                    }
                }
            }
            texture3D.Apply();
        }

    }
}
