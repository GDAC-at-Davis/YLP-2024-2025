using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.ParticleSystemTimeline
{

	// might be good to reference this: https://discussions.unity.com/t/particle-system-preview-scrubbing-by-script/717291/5

	public class ParticleSystemMixer : PlayableBehaviour 
	{
	
		// The sorting algorithm for clips 
		// This sort them from earliest start time to latest start time
		private int SortClips(Playable a, Playable b)
		{
			ParticleSystemBehaviour psb_a = ((ScriptPlayable<ParticleSystemBehaviour>)a).GetBehaviour();	
			double aStart = psb_a.owningClip.start;//psb_a.startTime;

			ParticleSystemBehaviour psb_b = ((ScriptPlayable<ParticleSystemBehaviour>)b).GetBehaviour();	
			double bStart = psb_b.owningClip.start;//psb_b.startTime;	
			
			if (aStart < bStart)
			{
				return -1;
			}
			else if (aStart > bStart)
			{
				return 1;
			}
			else 
			{
				return 0;
			}
		}
		
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			// retrieve Particle System that is associated with the track 
			ParticleSystem ps = (ParticleSystem)playerData;

			// do not preform process frame is Particle System doesn't exist 
			if (ps == null) { return; }
	
			// clear previous particles emitted by system
			var em = ps.emission; 
			em.enabled = false;	
			
			uint currentSeed = ps.randomSeed;
			ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ps.randomSeed = currentSeed;
			ps.Play();

			// get the current time of this clip on the timeline 
			double currentTime = playable.GetTime();

			// find all the clips currently on the track
			int numberOfClips = playable.GetInputCount();

			// create a priority queue that sorts all the playable clips by their start time 
			// This means the clips with the earliest start time can be accessed first
			
			//var clipQueue = new PriorityQueue< Playable, double >();
			List<Playable> clips = new List<Playable>();
			for (int i = 0; i < numberOfClips; i++)
			{
				Playable curr = playable.GetInput(i);
				clips.Add(curr); //clipQueue.Enqueue(curr, curr.GetTime());
			}
			clips.Sort(SortClips);
			

			// go through all the clip that proceed the current time on the timeline.
			// Simulate the particles based on the time that passes
			for (int i = 0; i < numberOfClips; i++)
			{
				Playable currClip = clips[i];
				ParticleSystemBehaviour psb = ((ScriptPlayable<ParticleSystemBehaviour>)currClip).GetBehaviour();	
				double currClipBegin = psb.owningClip.start; //psb.startTime;
				double currClipEnd = psb.owningClip.end; // psb.endTime;
				
				if (ps.main.loop == false)
				{
					ps.Stop();
					ps.Play();
				}

				if (currentTime < currClipBegin) // don't do anything if clip is after current time on timeline
				{
					//Debug.Log("before clip " + i + " : " + currentTime + " " + currClipBegin );
					break;
				}
				else if (currentTime < currClipEnd) // Simulate part of a clip until the current time 
				{
					em.enabled = true;
					ps.Simulate((float)(currentTime - currClipBegin), true, false);
					
					//Debug.Log("mid clip: " + currentTime + " _ " + currClipEnd);
					
					break;
				}
				else 
				{
					em.enabled = true; 
					ps.Simulate((float)(currClipEnd - currClipBegin), true, false); // simulate full clip 
				
					em.enabled = false;

					// simulate time between clips	
					if (i + 1 < numberOfClips)
					{
						Playable nextClip = clips[i + 1];
						ParticleSystemBehaviour psb2 = ((ScriptPlayable<ParticleSystemBehaviour>)nextClip).GetBehaviour();	
						double nextClipBegin = psb2.owningClip.start;//psb2.startTime;
						if (currentTime > nextClipBegin)
						{
							ps.Simulate((float)(nextClipBegin - currClipEnd), true, false);
							continue; 
						}
					}

					ps.Simulate((float)(currentTime - currClipEnd), true, false);
					break;
				}
			}
		}
	}
}
