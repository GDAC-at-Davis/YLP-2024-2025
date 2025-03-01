using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.ParticleSystemTimeline
{

	public class ParticleSystemAsset : PlayableAsset 
	{	
		public ParticleSystemBehaviour template;
		public TimelineClip owningClip;
		
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<ParticleSystemBehaviour>.Create(graph, template);

			ParticleSystemBehaviour behaviour = playable.GetBehaviour();

			if (owningClip != null)
			{
				behaviour.startTime = owningClip.start;
				behaviour.endTime = owningClip.end; 
			}

			return playable; 
		}

	}

}
