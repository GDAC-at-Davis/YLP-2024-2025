using Movement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween
{
    /// <summary>
    ///     Timeline track for various clips controlling CharacterRigidbody2D.
    /// </summary>
    [TrackColor(0f, 0.7f, 0f)]
    [TrackClipType(typeof(RbTweenPlayableAsset))]
    [TrackBindingType(typeof(CharacterRigidbody2D))]
    public class RbTweenTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<RbTweenTrackMixerBehavior>.Create(graph, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            var trackBinding = director.GetGenericBinding(this) as CharacterRigidbody2D;
            if (trackBinding == null)
            {
                return;
            }

            // This is to make sure properties reset when the timeline finishes prevewing in edit mode
            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalPosition");
            driver.AddFromName<CharacterRigidbody2D>(trackBinding.gameObject, "gravityAcceleration");

            base.GatherProperties(director, driver);
        }
    }
}