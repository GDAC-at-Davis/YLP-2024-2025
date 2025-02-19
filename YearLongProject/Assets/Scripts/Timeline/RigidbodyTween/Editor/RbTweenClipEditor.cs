using Timeline.RigidbodyTween.PositionTween;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

namespace Timeline.RigidbodyTween.Editor
{
    [CustomEditor(typeof(PositionTweenPlayableAsset))]
    public class RbTweenClipEditor : UnityEditor.Editor
    {
        private GUIStyle _labelStyle;

        private void OnEnable()
        {
            _labelStyle = new GUIStyle
            {
                normal = { textColor = Color.black },
                fontStyle = FontStyle.Bold,
                fontSize = 14
            };

            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            var clip = target as PositionTweenPlayableAsset;
            PositionTweenPlayableBehavior template = (target as PositionTweenPlayableAsset)?.template;

            if (template == null)
            {
                return;
            }

            // Start handle
            Vector3 startPos = template.StartPosition;
            Vector3 newStartPos = LabeledPositionHandle(startPos, Color.green, "Start Pos");
            if (startPos != newStartPos)
            {
                OnPositionHandleChange(clip);
                template.StartPosition = newStartPos;
            }

            // End handle
            Vector3 endPos = template.EndPosition;
            Vector3 newEndPos = LabeledPositionHandle(endPos, Color.red, "End Pos");
            if (endPos != newEndPos)
            {
                OnPositionHandleChange(clip);
                template.EndPosition = newEndPos;
            }

            Handles.DrawLine(newStartPos, newEndPos);
        }

        /// <summary>
        ///     Refresh the timeline when the position handle changes, so the tweened object updates to match the new position
        /// </summary>
        /// <param name="clip"></param>
        private void OnPositionHandleChange(PositionTweenPlayableAsset clip)
        {
            Undo.RecordObject(clip, "Edited Timeline Note Clip Position");
            TimelineEditor.Refresh(RefreshReason.ContentsModified);
        }

        private Vector2 LabeledPositionHandle(Vector2 pos, Color c, string label)
        {
            Handles.color = c;
            Vector2 newPos = Handles.PositionHandle(pos, Quaternion.identity);
            Handles.Label(newPos, label, _labelStyle);
            return newPos;
        }
    }
}