using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "OWL/Animation/AnimationProfile")]
public class OSpriteAnimationProfile_SO : ScriptableObject
{
    public List<OTextureSheet> textureSheets;
    public List<OSpriteAnimation> animations;
}
