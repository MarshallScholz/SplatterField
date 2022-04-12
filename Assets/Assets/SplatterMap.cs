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
        texture3D = new Texture3D(gridSize.x, gridSize.y, gridSize.z, TextureFormat.ARGB32, true);
        int pixelCount = gridSize.x * gridSize.y * gridSize.z;
        Color[] cols = new Color[pixelCount];

        //searches through the grid and adds paint to the corners of a 40 * 40 grid (-20, 20)
        for (int i = 0; i < gridSize.x; i++)//x
            for (int j = 0; j < gridSize.y; j++)//y
                for (int k = 0; k < gridSize.z; k++)//z
                {
                    //top left
                    if (i >= 5 && i <= 10 && k >= 30 && k <= 35)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //top right
                    else if (i >= 30 && i <= 35 && k >= 30 && k <= 35)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //bottom left
                    else if (i >= 5 && i <= 10 && k >= 5 && k <= 10)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //bottom right
                    else if (i >= 30 && i <= 35 && k >= 5 && k <= 10)
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
                    //everywhere else
                    else
                        cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(0, 0, 0, 1);
                }

        texture3D.SetPixels(cols);
        texture3D.Apply();

        //Sets "_voxels" in each shader to textured3D, so all gameobjects using this are updated at the same time when the texture3D updates
        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.SetTexture("_voxels", texture3D);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = cube.transform.position; //- this.transform.position;
            Vector3Int pixelPosition = new Vector3Int((int)position.x - gridExtents.x, (int)position.y - gridExtents.y, (int)position.z - gridExtents.z);

            //not on this splatter map
            if (pixelPosition.x < -40 || pixelPosition.x > 40 || //x
                pixelPosition.y < -40 || pixelPosition.y > 40 || //y 
                pixelPosition.z < -40 || pixelPosition.z > 40)   //z
                return;
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
