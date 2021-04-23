using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OSpriteAnimationEditor : EditorWindow
{
    [MenuItem("OWL/Sprite Animation Editor")]
    public static void ShowWindow()
    {
        OSpriteAnimationEditor editor = GetWindow<OSpriteAnimationEditor>("OWL - Sprite Animation Editor");
        editor.animationsCollapsed = new Dictionary<OSpriteAnimation, bool>();
        editor.texturesCollapsed = new Dictionary<OTextureSheet, bool>();
    }

    OSpriteAnimationProfile_SO animProfile;

    // New Profile
    string profileName = "NewProfile";
    bool showNewProfile = false;

    // Texture Sheets
    Vector2 textureScrollPos;
    Dictionary<OTextureSheet, bool> texturesCollapsed = new Dictionary<OTextureSheet, bool>();

    // Sprite Animations
    Vector2 animationScrollPos;
    Dictionary<OSpriteAnimation, bool> animationsCollapsed = new Dictionary<OSpriteAnimation, bool>();
    Dictionary<string, OSpriteAnimation> animationLibrary = new Dictionary<string, OSpriteAnimation>();

    // Animation Preview
    string currentAnimation = "";
    bool animate = false;
    float currentFrame = 0;
    Sprite currentSprite;
    Dictionary<string, List<Sprite>> spriteLibrary = new Dictionary<string, List<Sprite>>();
    Dictionary<string, List<int>> animationFrames = new Dictionary<string, List<int>>();
    float currentTime = 0;
    float deltaTime = 0;

    // Styles
    GUIStyle bold = new GUIStyle();

    private void OnGUI()
    {
        bold.normal.textColor = Color.white;
        bold.fontStyle = FontStyle.Bold;
        bold.padding.left = 5;
        bold.fontSize = 12;
        GUILayout.Label("Load Animation Profile Here", bold);

        GUILayout.BeginHorizontal();
        animProfile = EditorGUILayout.ObjectField("Animation Profile", animProfile, typeof(OSpriteAnimationProfile_SO), false) as OSpriteAnimationProfile_SO;

#if UNITY_EDITOR
        if(GUILayout.Button("Save"))
        {
            if (animProfile != null) EditorUtility.SetDirty(animProfile);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif

        GUILayout.EndHorizontal();

        CreateNewProfile();

        GUILayout.Label("");
        GUILayout.BeginHorizontal();
        TextureSheetsLoader();
        AnimationEditor();
        GUILayout.EndHorizontal();
        GUILayout.Label("");

        PreviewAnimation();
    }

    private void Update()
    {
        deltaTime = Time.realtimeSinceStartup - currentTime;
        currentTime = Time.realtimeSinceStartup;

        if (animate) Animate();
        Repaint();
    }

    void CreateNewProfile()
    {
        showNewProfile = EditorGUILayout.BeginFoldoutHeaderGroup(showNewProfile, "Create New Profile");

        if(showNewProfile)
        {
            GUILayout.BeginHorizontal();

            profileName = EditorGUILayout.TextField("Profile Name", profileName);

            if (GUILayout.Button("Create Profile"))
            {
                OSpriteAnimationProfile_SO profile = ScriptableObject.CreateInstance<OSpriteAnimationProfile_SO>();
                string path = "Assets\\OResources\\AnimationProfiles\\";
                path += profileName;
                path += ".asset";
                showNewProfile = false;

#if UNITY_EDITOR
                AssetDatabase.CreateAsset(profile, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif

                animProfile = profile;
            }

            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    void TextureSheetsLoader()
    {
        if (animProfile == null) return;
        GUILayout.BeginVertical();
        GUILayout.Label("Sprite Sheets", bold);
        textureScrollPos = EditorGUILayout.BeginScrollView(textureScrollPos, GUILayout.Width(300), GUILayout.Height(300));

        int count = 0;
        Queue<int> deleteQueue = new Queue<int>();

        if (animProfile.textureSheets == null) animProfile.textureSheets = new List<OTextureSheet>();

        foreach(var sheet in animProfile.textureSheets)
        {
            if (sheet == null) continue;
            if (!texturesCollapsed.ContainsKey(sheet)) texturesCollapsed.Add(sheet, false);

            texturesCollapsed[sheet] = EditorGUILayout.BeginFoldoutHeaderGroup(texturesCollapsed[sheet], sheet.name);

            if(texturesCollapsed[sheet])
            {
                sheet.name = EditorGUILayout.TextField("Name", sheet.name);
                sheet.sheet = EditorGUILayout.ObjectField("Sprite Sheet", sheet.sheet, typeof(Texture2D), false) as Texture2D;
                sheet.spriteSize = EditorGUILayout.Vector2Field("Sprite Size", sheet.spriteSize);

                if (GUILayout.Button("Delete"))
                {
                    deleteQueue.Enqueue(count);
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            ++count;
        }

        while(deleteQueue.Count > 0)
        {
            int current = deleteQueue.Dequeue();
            animProfile.textureSheets.RemoveAt(current);
        }

        if(GUILayout.Button("Add Sheet"))
        {
            animProfile.textureSheets.Add(new OTextureSheet());
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    void AnimationEditor()
    {
        if (animProfile == null) return;
        GUILayout.BeginVertical();
        GUILayout.Label("Animations", bold);
        animationScrollPos = EditorGUILayout.BeginScrollView(animationScrollPos, GUILayout.Width(300), GUILayout.Height(300));

        int count = 0;
        Queue<int> deleteQueue = new Queue<int>();

        if (animProfile.animations == null) animProfile.animations = new List<OSpriteAnimation>();

        foreach (var anim in animProfile.animations)
        {
            if (!animationsCollapsed.ContainsKey(anim)) animationsCollapsed.Add(anim, false);
            if (!animationLibrary.ContainsKey(anim.name)) animationLibrary.Add(anim.name, anim);

            animationsCollapsed[anim] = EditorGUILayout.BeginFoldoutHeaderGroup(animationsCollapsed[anim], anim.name);

            if (animationsCollapsed[anim])
            {
                anim.name = EditorGUILayout.TextField("Name", anim.name);
                anim.textureSheet = EditorGUILayout.TextField("Sprite Sheet", anim.textureSheet);
                anim.frames = EditorGUILayout.TextField("Frames", anim.frames);
                anim.speed = EditorGUILayout.FloatField("Speed", anim.speed);

                if (GUILayout.Button("Delete"))
                {
                    deleteQueue.Enqueue(count);
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            ++count;
        }

        while (deleteQueue.Count > 0)
        {
            int current = deleteQueue.Dequeue();
            animProfile.animations.RemoveAt(current);
        }

        if (GUILayout.Button("Add Animation"))
        {
            animProfile.animations.Add(new OSpriteAnimation());
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    void PreviewAnimation()
    {
        GUILayout.Label("");
        GUILayout.Label("Animation Preview", bold);

        GUILayout.BeginHorizontal();
        currentAnimation = EditorGUILayout.TextField("Animation", currentAnimation);
        if(GUILayout.Button("Play"))
        {
            OSpriteAnimationHelper.LoadAnimationProfile(animProfile, out spriteLibrary, out animationFrames, new Vector2(0.5f, 0.5f));
            animate = true;
            if (animationFrames.ContainsKey(currentAnimation)) currentFrame = animationFrames[currentAnimation][0];
        }
        if(GUILayout.Button("Stop"))
        {
            animate = false;
        }
        GUILayout.EndHorizontal();

        Rect previewRect = new Rect(150, 500, 300, 300);
        DrawTexturePreview(previewRect, currentSprite);
    }

    void Animate()
    {
        if (!animationFrames.ContainsKey(currentAnimation)) return;

        currentFrame += deltaTime * animationLibrary[currentAnimation].speed;
        if (currentFrame >= animationFrames[currentAnimation].Count) currentFrame = 0;
        if (currentFrame < 0) currentFrame = animationFrames[currentAnimation].Count - 1;

        currentSprite = spriteLibrary[currentAnimation][animationFrames[currentAnimation][(int)currentFrame]];
    }

    private void DrawTexturePreview(Rect position, Sprite sprite)
    {
        if (sprite == null) return;
        Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        Vector2 fullSize = new Vector2(currentSprite.texture.width, currentSprite.texture.height);
        Rect coords = currentSprite.textureRect;
        coords.x /= fullSize.x;
        coords.width /= fullSize.x;
        coords.y /= fullSize.y;
        coords.height /= fullSize.y;

        Vector2 ratio;
        ratio.x = position.width / size.x;
        ratio.y = position.height / size.y;
        float minRatio = Mathf.Min(ratio.x, ratio.y);

        Vector2 center = position.center;
        position.width = size.x * minRatio;
        position.height = size.y * minRatio;
        position.center = center;

        GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
    }
}
