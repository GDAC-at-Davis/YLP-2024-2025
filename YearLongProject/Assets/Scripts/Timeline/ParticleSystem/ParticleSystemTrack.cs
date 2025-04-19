using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace Timeline.ParticleSystemTimeline
{
	[TrackClipType(typeof(ParticleSystemAsset))]
	[TrackBindingType(typeof(ParticleSystem))]
	public class ParticleSystemTrack : TrackAsset 
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return ScriptPlayable<ParticleSystemMixer>.Create(graph, inputCount);
		}

		protected override void OnCreateClip(TimelineClip clip)
		{
			base.OnCreateClip(clip);

			ParticleSystemAsset asset = (ParticleSystemAsset)(clip.asset);
			if (asset != null)
			{
				asset.owningClip = clip;
			}
		}
	    
	}
}
