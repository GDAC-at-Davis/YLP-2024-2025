using Hitbox.HitboxAreas;
using System;
using System.Collections.Generic;
using System.Text;
using Timeline.Hitboxes;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(HitboxPlayableAsset))]
public class HitboxPlayableAssetDrawer : ClipEditor
{
    static List<Texture2D> endHitboxGroup, notEndHitboxGroup;
    string hitboxID;
    bool endGroup;
    Color color;

    public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
    {
        base.OnCreate(clip, track, clonedFrom);

        ((HitboxPlayableAsset)clip.asset).template.HitboxGroupId = "NewHitbox";
    }

    public override void OnClipChanged(TimelineClip clip)
    {
        // It resets when scripts are edited and the domain reloads :(
        endHitboxGroup = new List<Texture2D> { EditorGUIUtility.FindTexture("PauseButton") };
        notEndHitboxGroup = new();

        hitboxID = ((HitboxPlayableAsset)clip.asset).template.HitboxGroupId;
        endGroup = ((HitboxPlayableAsset)clip.asset).template.EndHitboxGroup;

        color = Color.red;
        string hex = (hitboxID.GetHashCode() & 0x00FFFFFF).ToString("X6");
        ColorUtility.TryParseHtmlString($"#{hex}", out color);
    }

    public override ClipDrawOptions GetClipOptions(TimelineClip clip)
    {
        ClipDrawOptions clipOptions = base.GetClipOptions(clip);
        clipOptions.displayClipName = false;

        clipOptions.highlightColor = color;
        clipOptions.icons = endGroup ? endHitboxGroup : notEndHitboxGroup;
        clipOptions.tooltip = hitboxID;

        return clipOptions;
    }
}
