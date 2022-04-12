using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterMap : MonoBehaviour
{
    Texture3D texture3D;
    public Transform cube;
    //public GameObject point;
    //public Texture2D splat;

    Vector3Int gridSize = new Vector3Int(40, 40, 40);
    Vector3Int gridExtents;
    Vector2Int splatExtents;

    // Start is called before the first frame update
    void Start()
    {
        gridExtents = new Vector3Int(gridSize.x / 2, gridSize.y / 2, gridSize.z / 2);
        //splatExtents = new Vector2Int(splat.height / 2, splat.width / 2);

        //creates a texture 3d (grid)
        texture3D = new Texture3D(gridSize.x, gridSize.y, gridSize.z, TextureFormat.ARGB32, false);
        int pixelCount = gridSize.x * gridSize.y * gridSize.z;
        Color[] cols = new Color[pixelCount];

        //searches through the grid and adds paint to the corners of a 40 * 40 grid (-20, 20)
        for (int i = 0; i < gridSize.x; i++)//x
            for (int j = 0; j < gridSize.y; j++)//y
                for (int k = 0; k < gridSize.z; k++)//z
                {
                    //top left
                    if (i >= 0 && i <= 5 && k >= 35 && k <= 40)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //top right
                    else if (i >= 35 && i <= 40 && k >= 35 && k <= 40)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //bottom left
                    else if (i >= 0 && i <= 5 && k >= 0 && k <= 5)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //bottom right
                    else if (i >= 35 && i <= 40 && k >= 0 && k <= 5)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //everywhere else
                    else
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(0, 0, 0, 1);
                }

        texture3D.SetPixels(cols);
        texture3D.Apply();

        MeshRenderer ren = GetComponent<MeshRenderer>();
        ren.material.SetTexture("_voxels", texture3D);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    int count = 0;
        //    Vector3 position = cube.transform.position;
        //    Vector3 pixelPosition = new Vector3(position.x - gridExtents.x, position.y - gridExtents.y, position.z - gridExtents.z);
        //    for (int i = pixelPosition.x - splatExtents.x; i < pixelPosition.x + splatExtents.x; i++)
        //    {
        //        for (int j = pixelPosition.y - splatExtents.y; j < pixelPosition.y + splatExtents.y; j++)
        //        {
        //            for (int k = pixelPosition.z - splatExtents.y; k < pixelPosition.z + splatExtents.y; k++)
        //            {
        //                Vector2Int splatLocation = new Vector2Int(position.x, position.y);
        //                if (splat.GetPixel(splatLocation.x, splatLocation.y) == new Color(0, 0, 0, 1))
        //                {
        //                    texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
        //                    Debug.Log(position);
        //                }
        //                //texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
        //                //point.transform.position = pixelLocation;
        //                count++;
        //            }
        //        }
        //    }
        //    texture3D.Apply();
        //}


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = cube.transform.position;
            Vector3Int pixelPosition = new Vector3Int((int)position.x - gridExtents.x, (int)position.y - gridExtents.y, (int)position.z - gridExtents.z);
            for (int i = pixelPosition.x - 3; i < pixelPosition.x + 3; i++)
            {
                //y
                for (int j = pixelPosition.y - 3; j < pixelPosition.y + 3; j++)
                {
                    //z
                    for (int k = pixelPosition.z - 3; k < pixelPosition.z + 3; k++)
                    {
                        Vector3 pixelLocation = position;
                        texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
                        //point.transform.position = pixelLocation;
                        Debug.Log(pixelLocation);

                    }
                }
            }
            //sending data to GPU
            // upating texture buffer
            texture3D.Apply();
        }
    }
}
