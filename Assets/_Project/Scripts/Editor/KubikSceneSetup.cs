using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class KupikSceneSetup
{
    [MenuItem("Kupik/Setup/Create Prototype Scene")]
    public static void CreatePrototypeScene()
    {
        // Yeni boþ sahne oluþtur
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "Level_01_Prototype";

        CreateFolders();
        CreateMaterials();

        GameObject levelRoot = new GameObject("Level_01_Prototype");
        GameObject blocksRoot = new GameObject("Blocks");
        blocksRoot.transform.SetParent(levelRoot.transform);

        CreateGround(blocksRoot.transform);
        CreatePlayerPlaceholder(levelRoot.transform);
        CreateCamera();
        CreateDirectionalLight();

        // Sahneyi kaydet
        string scenePath = "Assets/_Project/Scenes/Level_01_Prototype.unity";
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);

        Debug.Log("Kupik prototype scene created successfully.");
    }

    private static void CreateFolders()
    {
        CreateFolderIfMissing("Assets", "_Project");
        CreateFolderIfMissing("Assets/_Project", "Scenes");
        CreateFolderIfMissing("Assets/_Project", "Scripts");
        CreateFolderIfMissing("Assets/_Project", "Prefabs");
        CreateFolderIfMissing("Assets/_Project", "Materials");
        CreateFolderIfMissing("Assets/_Project", "Models");
        CreateFolderIfMissing("Assets/_Project", "Textures");
    }

    private static void CreateFolderIfMissing(string parent, string folder)
    {
        string path = parent + "/" + folder;

        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder(parent, folder);
        }
    }

    private static void CreateMaterials()
    {
        CreateMaterial("Grass_Mat", new Color(0.35f, 0.75f, 0.25f));
        CreateMaterial("Dirt_Mat", new Color(0.55f, 0.32f, 0.16f));
        CreateMaterial("Player_Blue_Mat", new Color(0.1f, 0.45f, 1f));
    }

    private static void CreateMaterial(string materialName, Color color)
    {
        string path = "Assets/_Project/Materials/" + materialName + ".mat";

        if (AssetDatabase.LoadAssetAtPath<Material>(path) != null)
            return;

        Shader shader = Shader.Find("Standard");

        if (shader == null)
        {
            shader = Shader.Find("Diffuse");
        }

        Material material = new Material(shader);
        material.color = color;

        AssetDatabase.CreateAsset(material, path);
        AssetDatabase.SaveAssets();
    }

    private static void CreateGround(Transform parent)
    {
        Material grassMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/Grass_Mat.mat");

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground_Block";
        ground.transform.SetParent(parent);

        ground.transform.position = Vector3.zero;
        ground.transform.rotation = Quaternion.identity;
        ground.transform.localScale = new Vector3(6f, 1f, 6f);

        Renderer renderer = ground.GetComponent<Renderer>();
        renderer.sharedMaterial = grassMat;
    }

    private static void CreatePlayerPlaceholder(Transform parent)
    {
        Material playerMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_Project/Materials/Player_Blue_Mat.mat");

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player_Placeholder";
        player.transform.SetParent(parent);

        player.transform.position = new Vector3(0f, 1.6f, 0f);
        player.transform.rotation = Quaternion.identity;
        player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        Renderer renderer = player.GetComponent<Renderer>();
        renderer.sharedMaterial = playerMat;

        Rigidbody rb = player.AddComponent<Rigidbody>();
        rb.mass = 1f;
        rb.drag = 0f;
        rb.angularDrag = 0.05f;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private static void CreateCamera()
    {
        GameObject cameraObject = new GameObject("Main Camera");
        Camera camera = cameraObject.AddComponent<Camera>();

        cameraObject.tag = "MainCamera";

        cameraObject.transform.position = new Vector3(8f, 8f, -8f);
        cameraObject.transform.rotation = Quaternion.Euler(35f, -45f, 0f);

        camera.orthographic = true;
        camera.orthographicSize = 7f;
        camera.clearFlags = CameraClearFlags.Skybox;
        camera.nearClipPlane = 0.3f;
        camera.farClipPlane = 1000f;
    }

    private static void CreateDirectionalLight()
    {
        GameObject lightObject = new GameObject("Directional Light");
        Light light = lightObject.AddComponent<Light>();

        light.type = LightType.Directional;
        light.intensity = 1f;

        lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }
}