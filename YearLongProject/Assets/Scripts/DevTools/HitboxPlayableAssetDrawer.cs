using System;
using System.Text;
using Timeline.Hitboxes;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(HitboxPlayableAsset))]
public class HitboxPlayableAssetDrawer : ClipEditor
{

    public override ClipDrawOptions GetClipOptions(TimelineClip clip)
    {
        ClipDrawOptions clipOptions = base.GetClipOptions(clip);
        string hitboxID = ((HitboxPlayableAsset)clip.asset).template.HitboxGroupId;

        Color color = Color.red;
        string hex = (hitboxID.GetHashCode() & 0x00FFFFFF).ToString("X6");
        ColorUtility.TryParseHtmlString($"#{hex}", out color);

        clipOptions.highlightColor = color;
        //clipOptions.icons = ((HitboxPlayableAsset)clip.asset).template.EndHitboxGroup ? EditorGUIUtility.FindTexture("PauseButton") : null;

        return clipOptions;
    }

}
