using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.ParticleSystemTimeline
{

	// might be good to reference this: https://discussions.unity.com/t/particle-system-preview-scrubbing-by-script/717291/5

	public class ParticleSystemMixer : PlayableBehaviour 
	{
		// this variable keep track of if the particle system is emitting particle. 
		// This means that the bool is:
		// 	false when Stop is called on the particle system
		// 	true when Play is called on the particle system
		// This boolean is independent from ParticleSystem.isPlaying.
		// This is because ParticleSystem.isPlaying is true if particle are still active 
		// even if the ParticleSystem isn't emmitting. 
		private bool isEmitting = false;

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			// retrieve Particle System that is associated with the track 
			ParticleSystem ps = (ParticleSystem)playerData;
		
			// do not preform process frame is Particle System doesn't exist 
			if (ps == null) { return; }
		
			// find all the clips currently on the track
			int numberOfClips = playable.GetInputCount();
			float totalClipWeight = 0.0f;

			// Process the information about all these clips, related to the current frame
			for (int i = 0; i < numberOfClips; i++)
			{
				float clipWeight = playable.GetInputWeight(i);
				totalClipWeight += clipWeight;

				// for now I'm going to assume there is no overlap between playable clips
				// if the playable clip has a significant weight i.e it is at the current frame of the timeline,
				// find it's starting point and use that to determine the time of the ParticleSystem
				if (clipWeight >= 0.9)
				{
					Playable clipPlayableObject = playable.GetInput(i);
					float clipLocalTime = (float)clipPlayableObject.GetTime();
					uint currentSeed = ps.randomSeed;

					ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
					ps.randomSeed = currentSeed;
					ps.Simulate(clipLocalTime); // <= this current method is hella jank

					//ps.time = (float)clipPlayableObject.GetTime();
					//Debug.Log(ps.time);
				}
			}

			// Only have the particle system active if a clip exist on the current frame of the timeline.
			// The clips will define when the particle system plays
			if (totalClipWeight <= 0.01 && isEmitting)
			{
				Debug.Log("Particle System Disactivated " + totalClipWeight);
				ps.Stop();
				isEmitting = false;
			}
			else if (totalClipWeight > 0.01 && !isEmitting) 
			{
				Debug.Log("Particle System Activated " + totalClipWeight);
				ps.Play();
				isEmitting = true;
			}

		}
	}
}
