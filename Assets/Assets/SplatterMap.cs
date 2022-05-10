using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SplatterMap : NetworkBehaviour
{
    Texture3D texture3D;
    public Transform cube;
    public Texture2D currentColour;
    //public GameObject point;
    //public Texture2D splat;

    public int gridSizeBox;
    Vector3Int gridSize;
    Vector3Int gridExtents;
    Vector2Int splatExtents;

    public int pixelMultiplyer = 1;
    public int lastPixelMultipleyer = 1;
    public int paintSplat = 10;
    public int lastPaintSplat = 10;

    public GameObject hitPoint;
    // Start is called before the first frame update
    void Start()
    {
        gridSize = new Vector3Int(gridSizeBox, gridSizeBox, gridSizeBox);
        gridExtents = new Vector3Int(gridSize.x / 2, gridSize.y / 2, gridSize.z / 2);

        //creates a texture 3d (grid)
        texture3D = new Texture3D(gridSize.x * pixelMultiplyer, gridSize.y * pixelMultiplyer, gridSize.z * pixelMultiplyer, TextureFormat.ARGB32, true);
        int pixelCount = (gridSize.x * pixelMultiplyer) * (gridSize.y * pixelMultiplyer) * (gridSize.z * pixelMultiplyer);
        lastPaintSplat = paintSplat;
        lastPixelMultipleyer = pixelMultiplyer;
        Color[] cols = new Color[pixelCount];


        foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.SetVector("_worldPosition", transform.position);
            mesh.material.SetFloat("_gridSize", gridExtents.x);
            mesh.material.SetFloat("_pixelMultiplyer", pixelMultiplyer);
            mesh.material.SetFloat("_paintSplat", paintSplat);
        }
        //searches through the grid and adds paint to the corners of a 40 * 40 grid (-20, 20)
        //for (int i = 0; i < gridSize.x; i++)//x
        //    for (int j = 0; j < gridSize.y; j++)//y
        //        for (int k = 0; k < gridSize.z; k++)//z
        //        {
        //            //top left
        //            if (i >= 5 && i <= 10 && k >= 30 && k <= 35)
        //            {
        //                cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
        //            }
        //            //top right
        //            else if (i >= 30 && i <= 35 && k >= 30 && k <= 35)
        //                cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
        //            //bottom left
        //            else if (i >= 5 && i <= 10 && k >= 5 && k <= 10)
        //                cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
        //            //bottom right
        //            else if (i >= 30 && i <= 35 && k >= 5 && k <= 10)
        //                cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(1, 1, 1, 1);
        //            //everywhere else
        //            else
        //                cols[i + j * gridSize.x + k * gridSize.y * gridSize.z] = new Color(0, 0, 0, 1);
        //        }

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
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 position = cube.transform.position - this.transform.position;
        //    Vector3Int pixelPosition = new Vector3Int((int)position.x - gridExtents.x, (int)position.y - gridExtents.y, (int)position.z - gridExtents.z);

        //    //not on this splatter map
        //    if (pixelPosition.x < -40 || pixelPosition.x > 0 || //x
        //        pixelPosition.y < -40 || pixelPosition.y > 0 || //y 
        //        pixelPosition.z < -40 || pixelPosition.z > 0)   //z
        //        return;
        //    for (int i = pixelPosition.x - 1; i < pixelPosition.x + 1; i++)
        //    {
        //        //y
        //        for (int j = pixelPosition.y - 1; j < pixelPosition.y + 1; j++)
        //        {
        //            //z
        //            for (int k = pixelPosition.z - 1; k < pixelPosition.z + 1; k++)
        //            {
        //                Vector3Int pixelLocation = pixelPosition;// + new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        //                texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
        //                //point.transform.position = pixelLocation;
        //                Debug.Log(pixelLocation);

        //            }
        //        }
        //    }

        //    texture3D.SetPixel(pixelPosition.x, pixelPosition.y, pixelPosition.z, new Color(1, 1, 1, 1));
        //    //sending data to GPU
        //    // upating texture buffer
        //    texture3D.Apply();
        //}
        if (paintSplat != lastPaintSplat)
            foreach (MeshRenderer mesh in GetComponentsInChildren<MeshRenderer>())
            {

                mesh.material.SetFloat("_paintSplat", paintSplat);
                lastPaintSplat = paintSplat;
            }
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdatePaint(Vector3 collisionPoint)
    {
        //tells all clients to do it
        RpcUpdatePaint(collisionPoint);
    }

    [ClientRpc]
    void RpcUpdatePaint(Vector3 collisionPoint)
    {
        UpdatePaint(collisionPoint);
    }

    //LOOK AT ARTICLE TO OPTIMIZE upto 15x faster https://answers.unity.com/questions/266170/for-different-texture-sizes-which-is-faster-setpix.html
    public void UpdatePaint(Vector3 collisionPosition)
    {
        Vector3 position = collisionPosition - this.transform.position;
        Vector3Int pixelPosition = new Vector3Int(((int)position.x - gridExtents.x) * pixelMultiplyer, ((int)position.y - gridExtents.y) * pixelMultiplyer, ((int)position.z - gridExtents.z) *pixelMultiplyer);
        //Vector3Int pixelPosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        texture3D.SetPixel(pixelPosition.x, pixelPosition.y, pixelPosition.z, new Color(1, 1, 1, 1));
        int paintRadius = 20 / pixelMultiplyer;
        //Debug.Log("Raw Position : " + position);
        //Debug.Log("Splatter map position: " + pixelPosition);
        for (int i = pixelPosition.x - paintRadius; i < pixelPosition.x + paintRadius; i++)
        {
            //y   xxx
            for (int j = pixelPosition.y - paintRadius; j < pixelPosition.y + paintRadius; j++)
            {
                //z
                for (int k = pixelPosition.z - paintRadius; k < pixelPosition.z + paintRadius; k++)
                {
                    //Makes it look more splatty with smaller radius'
                    //Vector3 delta = new Vector3(i - pixelPosition.x, j - pixelPosition.y, k - pixelPosition.z);
                    //if (delta.magnitude < paintRadius /* paintRadius*/)
                    //{
                        //Vector3Int pixelLocation = pixelPosition;// + new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
                        texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
                        //hitPoint.transform.position = pixelPosition;
                        //Debug.Log("Splatter map position: " + pixelPosition);
                    //}
                }
            }
        }
        //sending data to GPU
        // upating texture buffer
        texture3D.Apply();
    }

    public void UpdateColour(Texture2D newColour)
    {
        currentColour = newColour;
        foreach (MeshRenderer mesh in FindObjectOfType<SplatterMap>().GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.SetTexture("_PaintColour", currentColour);
        }
    }
}


//Vector3 position = this.transform.position - collisionPosition;
//Vector3Int pixelPosition = new Vector3Int((int)collisionPosition.x, (int)collisionPosition.y, (int)collisionPosition.z);
//texture3D.SetPixel(pixelPosition.x, pixelPosition.y, pixelPosition.z, new Color(1, 1, 1, 1));

//for (int i = pixelPosition.x - 2; i < pixelPosition.x + 2; i++)
//{
//    //y
//    for (int j = pixelPosition.y - 2; j < pixelPosition.y + 2; j++)
//    {
//        //z
//        for (int k = pixelPosition.z - 2; k < pixelPosition.z + 2; k++)
//        {
//            Vector3Int pixelLocation = pixelPosition;// + new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
//            texture3D.SetPixel(i, j, k, new Color(1, 1, 1, 1));
//            //point.transform.position = pixelLocation;
//            Debug.Log(pixelLocation);

//        }
//    }
//}
//cube.transform.position = collisionPosition;
////sending data to GPU
//// upating texture buffer
//texture3D.Apply();