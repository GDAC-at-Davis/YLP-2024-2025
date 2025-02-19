using Movement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.RigidbodyTween
{
    /// <summary>
    ///     Timeline track for hitboxes
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
            var trackBinding = director.GetGenericBinding(this) as Rigidbody2D;
            if (trackBinding == null)
            {
                return;
            }

            driver.AddFromName<Transform>(trackBinding.gameObject, "m_LocalPosition");

            base.GatherProperties(director, driver);
        }
    }
}