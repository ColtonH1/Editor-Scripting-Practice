using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class CharacterDesignerWindow : EditorWindow
{
    Texture2D headerSectionTexture;
    Texture2D playerSectionTexture;
    Texture2D npcSectionTexture;
    Texture2D enemySectionTexture;
    Texture2D playerTexture;
    Texture2D npcTexture;
    Texture2D enemyTexture;

    Color headerSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    Rect headerSection;
    Rect playerSection;
    Rect npcSection;
    Rect enemySection;
    Rect playerIconSection;
    Rect npcIconSection;
    Rect enemyIconSection;

    GUISkin skin;

    static PlayerData playerData;
    static NPCData npcData;
    static EnemyData enemyData;

    public static PlayerData PlayerInfo { get { return playerData; } }
    public static NPCData NPCInfo { get { return npcData; } }
    public static EnemyData EnemyInfo { get { return enemyData; } }

    float iconSize = 250;

    [MenuItem("Window/Character Designer")]
    static void OpenWindow()
    {
        CharacterDesignerWindow window = (CharacterDesignerWindow)GetWindow(typeof(CharacterDesignerWindow));
        window.minSize = new Vector2(600, 150);
        window.Show();
    }

    void OnEnable()
    {
        InitTextures();
        InitData();
        skin = Resources.Load<GUISkin>("guiStyles/CharacterDesignerSkin");
    }

    public static void InitData()
    {
        playerData = (PlayerData)ScriptableObject.CreateInstance(typeof(PlayerData));
        npcData = (NPCData)ScriptableObject.CreateInstance(typeof(NPCData));
        enemyData = (EnemyData)ScriptableObject.CreateInstance(typeof(EnemyData));
    }

    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();

        playerSectionTexture = Resources.Load<Texture2D>("Icons/UI pack/Green gradient square");
        npcSectionTexture = Resources.Load<Texture2D>("Icons/UI pack/Blue gradient square");
        enemySectionTexture = Resources.Load<Texture2D>("Icons/UI pack/Red gradient square");

        playerTexture = Resources.Load<Texture2D>("Icons/Player");
        npcTexture = Resources.Load<Texture2D>("Icons/NPC");
        enemyTexture = Resources.Load<Texture2D>("Icons/Enemy");
    }

    void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawPlayerSettings();
        DrawNPCSettings();
        DrawEnemySettings();
    }
    private void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;

        playerSection.x = 0;
        playerSection.y = 50;
        playerSection.width = Screen.width / 3f;
        playerSection.height = Screen.width - 50;

        playerIconSection.x = (playerSection.x + playerSection.width / 2f) - iconSize / 2f ;
        playerIconSection.y = playerSection.y + 8;
        playerIconSection.width = iconSize;
        playerIconSection.height = iconSize;

        npcSection.x = Screen.width / 3f;
        npcSection.y = 50;
        npcSection.width = Screen.width / 3f;
        npcSection.height = Screen.width - 50;

        npcIconSection.x = (npcSection.x + npcSection.width / 2f) - iconSize / 2f;
        npcIconSection.y = npcSection.y + 8;
        npcIconSection.width = iconSize;
        npcIconSection.height = iconSize;

        enemySection.x = (Screen.width / 3f) * 2;
        enemySection.y = 50;
        enemySection.width = Screen.width / 3f;
        enemySection.height = Screen.width - 50;

        enemyIconSection.x = (enemySection.x + enemySection.width / 2f) - iconSize / 2f;
        enemyIconSection.y = enemySection.y + 8;
        enemyIconSection.width = iconSize;
        enemyIconSection.height = iconSize;

        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(playerSection, playerSectionTexture);
        GUI.DrawTexture(npcSection, npcSectionTexture);
        GUI.DrawTexture(enemySection, enemySectionTexture);
        GUI.DrawTexture(playerIconSection, playerTexture);
        GUI.DrawTexture(npcIconSection, npcTexture);
        GUI.DrawTexture(enemyIconSection, enemyTexture);
    }
    private void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);

        GUILayout.Label("Character Designer", skin.GetStyle("Header"));

        GUILayout.EndArea();
    }
    private void DrawPlayerSettings()
    {
        GUILayout.BeginArea(playerSection);

        GUILayout.Space(iconSize + 8);

        GUILayout.Label("Player", skin.GetStyle("CharacterHeader"));

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.PLAYER);
        }

        GUILayout.EndArea();
    }

    private void DrawNPCSettings()
    {
        GUILayout.BeginArea(npcSection);

        GUILayout.Space(iconSize + 8);

        GUILayout.Label("NPC", skin.GetStyle("CharacterHeader"));

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.NPC);
        }

        GUILayout.EndArea();
    }

    private void DrawEnemySettings()
    {
        GUILayout.BeginArea(enemySection);

        GUILayout.Space(iconSize + 8);

        GUILayout.Label("Enemy", skin.GetStyle("CharacterHeader"));

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ENEMY);
        }

        GUILayout.EndArea();
    }
}

public class GeneralSettings : EditorWindow
{
    public enum SettingsType
    {
        PLAYER, 
        NPC, 
        ENEMY
    }

    static SettingsType dataSetting;
    static GeneralSettings window;

    public static void OpenWindow(SettingsType setting)
    {
        dataSetting = setting;
        window = (GeneralSettings)GetWindow(typeof(GeneralSettings));
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    void OnGUI()
    {
        switch (dataSetting)
        {
            case SettingsType.PLAYER:
                DrawSettings((CharacterData)CharacterDesignerWindow.PlayerInfo);
                break;
            case SettingsType.NPC:
                DrawSettings((CharacterData)CharacterDesignerWindow.NPCInfo);
                break;
            case SettingsType.ENEMY:
                DrawSettings((CharacterData)CharacterDesignerWindow.EnemyInfo);
                break;
        }
    }

    void DrawSettings(CharacterData charData)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Prefab: ");
        charData.prefab = (GameObject) EditorGUILayout.ObjectField(charData.prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Abilities: ");

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Health: ");
        charData.maxHealth = EditorGUILayout.FloatField(charData.maxHealth);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Can Walk: ");
        charData.canWalk = EditorGUILayout.Toggle(charData.canWalk);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Walk Speed: ");
        charData.walkSpeed = EditorGUILayout.IntField(charData.walkSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Can Run: ");
        charData.canRun = EditorGUILayout.Toggle(charData.canRun);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Run Speed: ");
        charData.runSpeed = EditorGUILayout.IntField(charData.runSpeed);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Can Jump: ");
        charData.canJump = EditorGUILayout.Toggle(charData.canJump);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Jump Height: ");
        charData.jumpHeight = EditorGUILayout.IntField(charData.jumpHeight);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name: ");
        charData.charName = EditorGUILayout.TextField(charData.charName);
        EditorGUILayout.EndHorizontal();



        if (charData.prefab == null)
        {
            EditorGUILayout.HelpBox("This character needs a [Prefab] before it can be created.", MessageType.Warning);
        }

        else if(charData.charName == null || charData.charName.Length < 1)
        {
            EditorGUILayout.HelpBox("This character needs a [Name] before it can be created.", MessageType.Warning);
        }

        else if(GUILayout.Button("Finish and Save", GUILayout.Height(30)))
        {
            SaveCharacterData();
            window.Close();
        }

        void SaveCharacterData()
        {
            string prefabPath;
            string newPrefabPath = "Assets/Prefabs/Characters/";
            string dataPath = "Assets/Resources/Character Data/Data/";

            switch(dataSetting)
            {
                case SettingsType.PLAYER:
                    dataPath += "Player/" + CharacterDesignerWindow.PlayerInfo.charName + ".asset";
                    AssetDatabase.CreateAsset(CharacterDesignerWindow.PlayerInfo, dataPath);

                    newPrefabPath += "Player/" + CharacterDesignerWindow.PlayerInfo.name + ".prefab";
                    prefabPath = AssetDatabase.GetAssetPath(CharacterDesignerWindow.PlayerInfo.prefab);
                    AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    GameObject playerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                    if(!playerPrefab.GetComponent<Player>())
                    {
                        playerPrefab.AddComponent(typeof(Player));
                    }
                    playerPrefab.GetComponent<Player>().playerData = CharacterDesignerWindow.PlayerInfo;

                    break;
                case SettingsType.NPC:
                    dataPath += "NPC/" + CharacterDesignerWindow.NPCInfo.charName + ".asset";
                    AssetDatabase.CreateAsset(CharacterDesignerWindow.NPCInfo, dataPath);

                    newPrefabPath += "NPC/" + CharacterDesignerWindow.NPCInfo.name + ".prefab";
                    prefabPath = AssetDatabase.GetAssetPath(CharacterDesignerWindow.NPCInfo.prefab);
                    AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    GameObject npcPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                    if (!npcPrefab.GetComponent<NPC>())
                    {
                        npcPrefab.AddComponent(typeof(NPC));
                    }
                    npcPrefab.GetComponent<NPC>().npcData = CharacterDesignerWindow.NPCInfo;
                    break;
                case SettingsType.ENEMY:
                    dataPath += "Enemy/" + CharacterDesignerWindow.EnemyInfo.charName + ".asset";
                    AssetDatabase.CreateAsset(CharacterDesignerWindow.EnemyInfo, dataPath);

                    newPrefabPath += "Enemy/" + CharacterDesignerWindow.EnemyInfo.name + ".prefab";
                    prefabPath = AssetDatabase.GetAssetPath(CharacterDesignerWindow.EnemyInfo.prefab);
                    AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    GameObject enemyPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                    if (!enemyPrefab.GetComponent<Enemy>())
                    {
                        enemyPrefab.AddComponent(typeof(Enemy));
                    }
                    enemyPrefab.GetComponent<Enemy>().enemyData = CharacterDesignerWindow.EnemyInfo;

                    break;
            }
        }
    }
}
