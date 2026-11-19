#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class BookSceneBuilder
{
    private const string Root = "Assets/BookSamples";
    private const string SceneFolder = Root + "/Scenes";
    private const string PrefabFolder = Root + "/Prefabs";
    private const string ArtFolder = Root + "/Art";
    private static Sprite squareSprite;

    [MenuItem("Tools/Book Samples/Generate/Refresh Sample Scenes")]
    public static void Generate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.LogWarning("Stop Play mode before generating the sample scenes.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        Directory.CreateDirectory(SceneFolder);
        Directory.CreateDirectory(PrefabFolder);
        Directory.CreateDirectory(ArtFolder);
        AssetDatabase.Refresh();

        EnsureSquareSprite();
        GameObject bulletPrefab = CreateBulletPrefab();
        GameObject normalEnemyPrefab = CreateEnemyPrefab(false);
        GameObject fastEnemyPrefab = CreateEnemyPrefab(true);

        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
        // Keep non-book scenes that might already be in this Unity project.
        foreach (EditorBuildSettingsScene existing in EditorBuildSettings.scenes)
        {
            if (!existing.path.StartsWith(SceneFolder + "/"))
                scenes.Add(existing);
        }

        for (int chapter = 1; chapter <= 12; chapter++)
        {
            CreateChapterScene(chapter, bulletPrefab, normalEnemyPrefab, fastEnemyPrefab);
            string path = SceneFolder + "/Chapter" + chapter.ToString("00") + ".unity";
            scenes.Add(new EditorBuildSettingsScene(path, true));
        }

        EditorBuildSettings.scenes = scenes.ToArray();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Book sample scenes generated: Chapter01 ... Chapter12. Open a scene from Assets/BookSamples/Scenes.");
    }

    private static void EnsureSquareSprite()
    {
        string path = ArtFolder + "/Square.png";
        if (!File.Exists(path))
        {
            const int size = 32;
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            Color32[] pixels = new Color32[size * size];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = new Color32(255, 255, 255, 255);
            texture.SetPixels32(pixels);
            texture.Apply();
            File.WriteAllBytes(path, texture.EncodeToPNG());
            Object.DestroyImmediate(texture);
        }

        AssetDatabase.ImportAsset(path);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null && importer.textureType != TextureImporterType.Sprite)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 32;
            importer.filterMode = FilterMode.Point;
            importer.SaveAndReimport();
        }
        squareSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static GameObject CreateBulletPrefab()
    {
        GameObject bullet = CreateSpriteObject("Bullet", new Color(1f, 0.85f, 0.3f), new Vector2(0.18f, 0.5f));
        Rigidbody2D body = bullet.AddComponent<Rigidbody2D>();
        body.gravityScale = 0f;
        body.bodyType = RigidbodyType2D.Kinematic;
        bullet.AddComponent<BoxCollider2D>().isTrigger = true;
        bullet.AddComponent<Bullet>();
        string path = PrefabFolder + "/Bullet.prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(bullet, path);
        Object.DestroyImmediate(bullet);
        return prefab;
    }

    private static GameObject CreateEnemyPrefab(bool fast)
    {
        string name = fast ? "FastEnemy" : "Enemy";
        Color color = fast ? new Color(0.7f, 0.45f, 1f) : new Color(1f, 0.42f, 0.42f);
        GameObject enemy = CreateSpriteObject(name, color, fast ? new Vector2(0.8f, 0.8f) : new Vector2(1f, 1f));
        enemy.AddComponent<BoxCollider2D>().isTrigger = true;
        if (fast)
            enemy.AddComponent<FastEnemy>();
        else
            enemy.AddComponent<Enemy>();

        string path = PrefabFolder + "/" + name + ".prefab";
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(enemy, path);
        Object.DestroyImmediate(enemy);
        return prefab;
    }

    private static GameObject CreateSpriteObject(string name, Color color, Vector2 scale)
    {
        GameObject obj = new GameObject(name);
        SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
        renderer.sprite = squareSprite;
        renderer.color = color;
        obj.transform.localScale = new Vector3(scale.x, scale.y, 1f);
        return obj;
    }

    private static void CreateChapterScene(int chapter, GameObject bulletPrefab, GameObject enemyPrefab, GameObject fastPrefab)
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5f;
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = new Color(0.04f, 0.07f, 0.14f);
        cameraObject.AddComponent<AudioListener>();
        cameraObject.transform.position = new Vector3(0f, 0f, -10f);

        GameObject player = CreateSpriteObject("Player", new Color(0.2f, 0.9f, 0.95f), new Vector2(0.9f, 0.9f));
        player.transform.position = new Vector3(0f, -3.6f, 0f);

        if (chapter >= 2)
        {
            player.AddComponent<PlayerMovement>();
        }

        if (chapter >= 4)
        {
            Rigidbody2D body = player.AddComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.bodyType = RigidbodyType2D.Kinematic;
            player.AddComponent<BoxCollider2D>().isTrigger = true;

            GameObject muzzle = new GameObject("Muzzle");
            muzzle.transform.SetParent(player.transform, false);
            muzzle.transform.localPosition = new Vector3(0f, 0.65f, 0f);
            player.AddComponent<PlayerShooter>().Configure(bulletPrefab, muzzle.transform);
        }

        if (chapter >= 11)
            player.AddComponent<PlayerPulse>();

        if (chapter >= 8)
            CreateHud(chapter);

        if (chapter == 5)
        {
            GameObject enemy = (GameObject)PrefabUtility.InstantiatePrefab(enemyPrefab);
            enemy.transform.position = new Vector3(0f, 2.8f, 0f);
        }

        if (chapter >= 6)
        {
            GameObject spawnerObject = new GameObject("Enemy Spawner");
            EnemySpawner spawner = spawnerObject.AddComponent<EnemySpawner>();
            GameObject[] enemies = chapter >= 10
                ? new[] { enemyPrefab, enemyPrefab, fastPrefab }
                : new[] { enemyPrefab };
            spawner.Configure(enemies, chapter >= 7);
        }

        string scenePath = SceneFolder + "/Chapter" + chapter.ToString("00") + ".unity";
        EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), scenePath);
    }

    private static void CreateHud(int chapter)
    {
        GameObject managerObject = new GameObject("Game Session");
        GameSession session = managerObject.AddComponent<GameSession>();

        GameObject canvasObject = new GameObject("HUD Canvas", typeof(RectTransform));
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasObject.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        TMP_Text score = CreateLabel(canvasObject.transform, "Score", "SCORE: 0",
            new Vector2(0f, 1f), new Vector2(16f, -16f), new Vector2(280f, 60f),
            TextAlignmentOptions.TopLeft, 30);

        TMP_Text hp = null;
        TMP_Text gameOver = null;
        if (chapter >= 9)
        {
            hp = CreateLabel(canvasObject.transform, "HP", "HP: 3",
                new Vector2(1f, 1f), new Vector2(-16f, -16f), new Vector2(200f, 60f),
                TextAlignmentOptions.TopRight, 30);

            gameOver = CreateLabel(canvasObject.transform, "Game Over", "GAME OVER\nPress R to retry",
                new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(700f, 170f),
                TextAlignmentOptions.Center, 42);
            gameOver.gameObject.SetActive(false);
        }

        session.Configure(score, hp, gameOver, chapter >= 9);
    }

    private static TMP_Text CreateLabel(Transform parent, string name, string content,
        Vector2 anchor, Vector2 position, Vector2 size, TextAlignmentOptions alignment, float fontSize)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        return text;
    }
}
#endif
