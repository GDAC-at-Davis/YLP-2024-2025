using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.ParticleSystemTimeline
{

	public class ParticleSystemAsset : PlayableAsset 
	{	
		public ParticleSystemBehaviour template;
		
		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return ScriptPlayable<ParticleSystemBehaviour>.Create(graph, template);
		}

	}

}
