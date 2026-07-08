using UnityEditor;
using UnityEngine;

public static class KupikLevelBuilder
{
    [MenuItem("Kupik/Level/Build Simple Cubic Island")]
    public static void BuildSimpleCubicIsland()
    {
        GameObject levelRoot = GameObject.Find("Level_01_Prototype");

        if (levelRoot == null)
        {
            levelRoot = new GameObject("Level_01_Prototype");
        }

        Transform blocksRoot = levelRoot.transform.Find("Blocks");

        if (blocksRoot == null)
        {
            GameObject blocksObject = new GameObject("Blocks");
            blocksObject.transform.SetParent(levelRoot.transform);
            blocksRoot = blocksObject.transform;
        }

        ClearOldBlocks(blocksRoot);
        EnsureMaterials();
        CreateIsland(blocksRoot);
        MovePlayerToStart();

        Debug.Log("Simple cubic island created.");
    }

    private static void ClearOldBlocks(Transform blocksRoot)
    {
        for (int i = blocksRoot.childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(blocksRoot.GetChild(i).gameObject);
        }
    }

    private static void EnsureMaterials()
    {
        CreateMaterialIfMissing("Grass_Mat", new Color(0.35f, 0.75f, 0.25f));
        CreateMaterialIfMissing("Dirt_Mat", new Color(0.55f, 0.32f, 0.16f));
        CreateMaterialIfMissing("Stone_Mat", new Color(0.55f, 0.55f, 0.55f));
    }

    private static void CreateMaterialIfMissing(string materialName, Color color)
    {
        string path = "Assets/_Project/Materials/" + materialName + ".mat";

        if (AssetDatabase.LoadAssetAtPath<Material>(path) != null)
            return;

        Shader shader = Shader.Find("Standard");

        Material material = new Material(shader);
        material.color = color;

        AssetDatabase.CreateAsset(material, path);
        AssetDatabase.SaveAssets();
    }

    private static void CreateIsland(Transform parent)
    {
        /*
         * Her satýr bir Z hattý.
         * X = blok var
         * . = boþluk
         *
         * Bu ilk ada çok basit:
         * - baþlangýç alaný
         * - küçük boþluk
         * - yükselti
         * - çýkýþ alaný
         */

        string[] map =
        {
            ".....",
            ".XXX.",
            "XXXXX",
            "XXXXX",
            "XXX.X",
            "XXXXX",
            ".XXX.",
            "....."
        };

        int depth = map.Length;
        int width = map[0].Length;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[z][x] != 'X')
                    continue;

                int centeredX = x - width / 2;
                int centeredZ = z - depth / 2;

                float height = GetHeightForBlock(centeredX, centeredZ);

                CreateBlock(
                    parent,
                    new Vector3(centeredX, height, centeredZ),
                    height
                );
            }
        }
    }

    private static float GetHeightForBlock(int x, int z)
    {
        // Küçük yükselti alaný
        if (z >= 1 && x >= 1)
            return 1f;

        // Orta geçiþ alaný
        if (z == 0 && x >= 0)
            return 0.5f;

        return 0f;
    }

    private static void CreateBlock(Transform parent, Vector3 position, float height)
    {
        Material grassMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/Grass_Mat.mat");
        Material dirtMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/Dirt_Mat.mat");
        Material stoneMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/Stone_Mat.mat");

        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = "Block_" + position.x + "_" + position.z;
        block.transform.SetParent(parent);

        block.transform.position = position;
        block.transform.localScale = Vector3.one;

        Renderer renderer = block.GetComponent<Renderer>();

        if (height >= 1f)
        {
            renderer.sharedMaterial = stoneMat;
        }
        else
        {
            renderer.sharedMaterial = grassMat != null ? grassMat : dirtMat;
        }

        int groundLayer = LayerMask.NameToLayer("Ground");

        if (groundLayer != -1)
        {
            block.layer = groundLayer;
        }
    }

    private static void MovePlayerToStart()
    {
        GameObject player = GameObject.Find("Player_Placeholder");

        if (player == null)
            return;

        player.transform.position = new Vector3(-1.5f, 2f, -2.5f);

        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}