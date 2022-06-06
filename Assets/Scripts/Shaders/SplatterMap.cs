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

    public Vector3Int gridSize;
    Vector4 gridExtents;
    Vector2Int splatExtents;

    public int pixelMultiplyer = 1;
    public int lastPixelMultipleyer = 1;
    public int paintSplat = 10;
    public int lastPaintSplat = 10;

    public Vector3 paintOffset;

    public GameObject hitPoint;

    public float minPaintArea = 1;
    public float drawChance = 50;

    public float player1Score = 0;
    public float player2Score = 0;

    public Color paintColour = new Color(1, 0, 0, 0);
    public Color player1Colour = new Color(1, 1, 1, 1);
    public Color player2Colour = new Color(0.4f, 1, 1, 1);

    public List<MeshRenderer> meshes;
    public List<Material> materials;

    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        ResetSplatterMap();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void ResetSplatterMap()
    {
        gridSize = new Vector3Int(Mathf.RoundToInt(renderer.bounds.size.x), Mathf.RoundToInt(renderer.bounds.size.y), Mathf.RoundToInt(renderer.bounds.size.z));
        gridExtents = new Vector4(gridSize.x / 2, gridSize.y / 2, gridSize.z / 2, 0);

        //creates a texture 3d (grid)
        texture3D = new Texture3D(gridSize.x * pixelMultiplyer, gridSize.y * pixelMultiplyer, gridSize.z * pixelMultiplyer, TextureFormat.R8, true);
        int pixelCount = (gridSize.x * pixelMultiplyer) * (gridSize.y * pixelMultiplyer) * (gridSize.z * pixelMultiplyer);
        lastPaintSplat = paintSplat;
        lastPixelMultipleyer = pixelMultiplyer;
        Color[] cols = new Color[pixelCount];

        UpdateColour(currentColour);

        //===================Creates a list of materials to be updated =========================

        foreach (GameObject gm in GameObject.FindGameObjectsWithTag("Paintable"))
        {
            MeshRenderer mesh = gm.GetComponent<MeshRenderer>();

            if (mesh != null)
            {
                foreach (Material material in mesh.materials)
                {
                    if (material.HasProperty("_PaintColour"))
                    {
                        materials.Add(material);
                    }
                }
            }
        }

        for(int i = 0; i < materials.Count; i++)
        {
            materials[i].SetVector("_worldPosition", transform.position);
            //materials[i].SetFloat("_gridSize", gridExtents.x);
            materials[i].SetVector("_gridSize", gridExtents);
            materials[i].SetFloat("_pixelMultiplyer", pixelMultiplyer);
            materials[i].SetFloat("_paintSplat", paintSplat);
            materials[i].SetTexture("_PaintColour", currentColour);
        }

        texture3D.SetPixels(cols);
        texture3D.Apply();


        //Updates the textured3D, so all gameobjects using this are updated at the same time when the texture3D updates

        for(int i = 0; i < materials.Count; i++)
        {
            materials[i].SetTexture("_TextureMask1", texture3D);
        }
    }

    private void Update()
    {
        if (paintSplat != lastPaintSplat)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_paintSplat", paintSplat);
                lastPaintSplat = paintSplat;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ResetSplatterMap();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            paintColour = player1Colour;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            paintColour = player2Colour;
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
        //collisionPosition += new Vector3(1, 0, 1);
        Vector3 position = collisionPosition - this.transform.position;

        //========================== Added offset for better splat centers ====================================
        Vector3Int pixelPosition = new Vector3Int((Mathf.RoundToInt(position.x - gridExtents.x + paintOffset.x) * pixelMultiplyer), (Mathf.RoundToInt(position.y - gridExtents.y + paintOffset.y) * pixelMultiplyer), (Mathf.RoundToInt(position.z - gridExtents.z + paintOffset.z) * pixelMultiplyer));
        //Vector3 pixelPosition = collisionPosition;
        //Vector3Int pixelPosition = new Vector3Int((int)position.x, (int)position.y, (int)position.z);
        texture3D.SetPixel(pixelPosition.x, pixelPosition.y, pixelPosition.z, paintColour);
        int paintRadius = 3 * pixelMultiplyer;
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
                    Color previousColour = texture3D.GetPixel(i, j, k);


                    //Makes it look more splatty with smaller radius'
                    Vector3 delta = new Vector3(i - pixelPosition.x, j - pixelPosition.y, k - pixelPosition.z);
                    if (delta.magnitude < paintRadius / minPaintArea /* paintRadius*/)
                        texture3D.SetPixel(i, j, k, paintColour);
                    else
                    {
                        //decrease the drawchance as the i, j and k values increase to make nicer looking splats2
                        //USE A NOISE INSTEAD?
                        float drawPixel = Random.Range(0, 100);
                        if(drawChance > drawPixel)
                        {
                            texture3D.SetPixel(i, j, k, paintColour);
                        }
                    }

                    Color currentColour = texture3D.GetPixel(i, j, k);

                    //============ UPDATES PAINT SCORE
                    if (currentColour == player1Colour && previousColour == player2Colour)
                    {
                        player1Score++;
                        player2Score--;
                    }
                    else if(currentColour == player2Colour && previousColour == player1Colour)
                    {
                        player2Score++;
                        player1Score--;
                    }

                    else if (previousColour == new Color(0, 1, 1, 1) && currentColour == player1Colour)
                        player1Score++;

                    else if (previousColour == new Color(0, 1, 1, 1) && currentColour == player2Colour)
                        player2Score++;
                }
            }
        }
        //sending data to GPU
        // upating texture buffer
        texture3D.Apply();
        cube.transform.position = collisionPosition;
    }

    public void UpdateColour(Texture2D newColour)
    {
        currentColour = newColour;
        //========================== Could optimise to not do every gameobject ====================================
        for(int i = 0; i < materials.Count; i++)
        {
            materials[i].SetTexture("_PaintColour", currentColour);
        }
    }
}
