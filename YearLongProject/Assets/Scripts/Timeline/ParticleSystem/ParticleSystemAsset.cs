using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.ParticleSystemTimeline
{

	public class ParticleSystemAsset : PlayableAsset, ITimelineClipAsset
	{	
		public ParticleSystemBehaviour template;
		public TimelineClip owningClip;
		
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<ParticleSystemBehaviour>.Create(graph, template);

			// use the assets clip to assign end and start time to the behavior
			ParticleSystemBehaviour behaviour = playable.GetBehaviour();
			if (owningClip != null)
			{
				behaviour.owningClip = owningClip;
				//behaviour.startTime = owningClip.start;
				//behaviour.endTime = owningClip.end; 
			}

			return playable; 
		}


		public ClipCaps clipCaps => ClipCaps.None; 

	}

}
