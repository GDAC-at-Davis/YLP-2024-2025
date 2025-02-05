using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Samples
{
    // Editor used by the TimelineEditor to customize the view of TweenClip.
    [CustomTimelineEditor(typeof(TweenClip))]
    public class TweenClipEditor : ClipEditor
    {
        private static readonly GUIStyle s_StartTextStyle;
        private static readonly GUIStyle s_EndTextStyle;

        static TweenClipEditor()
        {
            s_StartTextStyle = GUI.skin.label;
            s_EndTextStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
        }

        // Called by the Timeline editor to draw the background of a TweenClip.
        public override void DrawBackground(TimelineClip clip, ClipBackgroundRegion region)
        {
            var asset = clip.asset as TweenClip;

            if (asset == null)
            {
                return;
            }

            PlayableDirector director = TimelineEditor.inspectedDirector;

            if (director == null)
            {
                return;
            }

            var startLocation =
                director.GetReferenceValue(asset.startLocation.exposedName, out bool startFound) as Transform;
            var endLocation = director.GetReferenceValue(asset.endLocation.exposedName, out bool endFound) as Transform;

            if (startFound && startLocation != null)
            {
                EditorGUI.LabelField(region.position, startLocation.gameObject.name, s_StartTextStyle);
            }

            if (endFound && endLocation != null)
            {
                EditorGUI.LabelField(region.position, endLocation.gameObject.name, s_EndTextStyle);
            }
        }
    }
}