using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OTextureSheet
{
    public string name = "";
    public Texture2D sheet;
    public Vector2 spriteSize;
}

[System.Serializable]
public class OSpriteAnimation
{
    public string name = "";
    public string textureSheet = "";
    public string frames = "";
    public float speed = 0;
}

public class OSpriteAnimationHelper
{
    public static void LoadAnimationProfile(OSpriteAnimationProfile_SO profile, 
        out Dictionary<string, List<Sprite>> spriteLibrary, 
        out Dictionary<string, List<int>> animationFrames, 
        Vector2 pivot)
    {
        spriteLibrary = new Dictionary<string, List<Sprite>>();
        animationFrames = new Dictionary<string, List<int>>();

        // Generate Sprite Library
        foreach (var sheet in profile.textureSheets)
        {
            spriteLibrary.Add(sheet.name, new List<Sprite>());

            for (float y = sheet.sheet.height - sheet.spriteSize.y; y >= 0; y -= sheet.spriteSize.y)
            {
                for (float x = 0; x < sheet.sheet.width; x += sheet.spriteSize.x)
                {
                    Rect rect = new Rect(x, y, sheet.spriteSize.x, sheet.spriteSize.y);
                    spriteLibrary[sheet.name].Add(Sprite.Create(sheet.sheet, rect, pivot));
                }
            }
        }

        // Generate Animation Frames
        foreach (var animation in profile.animations)
        {
            animationFrames.Add(animation.name, new List<int>());

            string frameDesc = animation.frames;

            int firstComma = frameDesc.IndexOf(',');
            int firstDash = frameDesc.IndexOf('-');
            int currentFrame = -1;

            while (firstComma >= 0 || firstDash >= 0)
            {

                if (firstComma >= 0 && (firstComma < firstDash || firstDash < 0))
                {
                    currentFrame = int.Parse(frameDesc.Substring(0, firstComma));
                    frameDesc = frameDesc.Substring(firstComma + 1);

                    animationFrames[animation.name].Add(currentFrame);
                }
                else if (firstDash >= 0 && (firstDash < firstComma || firstComma < 0))
                {
                    currentFrame = int.Parse(frameDesc.Substring(0, firstDash));
                    frameDesc = frameDesc.Substring(firstDash + 1);

                    int nextComma = frameDesc.IndexOf(',');
                    int maxFrame;
                    if (nextComma >= 0) maxFrame = int.Parse(frameDesc.Substring(0, nextComma));
                    else maxFrame = int.Parse(frameDesc);

                    if (maxFrame < currentFrame) continue;

                    while (currentFrame <= maxFrame)
                    {
                        animationFrames[animation.name].Add(currentFrame);
                        ++currentFrame;
                    }
                }
                else
                {
                    break;
                    //firstComma = frameDesc.IndexOf(',');
                    //firstDash = frameDesc.IndexOf('-');
                    //continue;
                }
                firstComma = frameDesc.IndexOf(',');
                firstDash = frameDesc.IndexOf('-');
            }
        }
    }
}