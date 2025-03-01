using System;
using System.Collections.Generic;
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
		//private bool isEmitting = false;
		
		private int SortClips(Playable a, Playable b)
		{
			if (a.GetTime() < b.GetTime())
			{
				return -1;
			}
			else if (a.GetTime() > b.GetTime())
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
				double currClipEnd = currClip.GetTime() + currClip.GetDuration();

				if (currentTime < currClip.GetTime()) // don't do anything if clip is after current time on timeline
				{
					break;
				}
				else if (currentTime < currClipEnd) // Simulate part of a clip until the current time 
				{
					em.enabled = true;
					ps.Simulate((float)(currentTime - currClip.GetTime()));
					
					Debug.Log("" + currentTime + " _ " + currClip.GetTime());
					
					break;
				}
				else 
				{
					em.enabled = true; 
					ps.Simulate((float)(currClip.GetDuration())); // simulate full clip 
				
					em.enabled = false;
					// simulate time between clips 
					if ((i + 1 < numberOfClips) && (currentTime > clips[i + 1].GetTime()))
					{
						ps.Simulate((float)(clips[i + 1].GetTime() - currClipEnd));
					}
					// simulate until current time  
					else 
					{
						ps.Simulate((float)(currentTime - currClipEnd));
						break;
					}
				}
			}

			/*Playable currClip; <== R.I.P priority queue code
			 //bool stillHasClips = clipQueue.TryDequeue(out currClip, out _);
			Playable nextClip;

			while (stillHasClips)
			{
				double currClipEnd = currClip.GetTime() + currClip.GetDuration();

				if (currentTime < currClip.GetTime())
				{
					break; 
				}
				else if (currentTime < currClipEnd)
				{
					em.enabled = true; 
					ps.Simulate((float)(currentTime - currClip.GetTime()));
				}
				else 
					em.enabled = true; 
					ps.Simulate((float)(currClip.GetDuration()));
					
					//stillHasClips = clipQueue.Dequeue(out nextClip, out _);
					em.enabled = false;
					if (stillHasClips && (currentTime > nextClip.GetTime()))
					{
						ps.Simulate((float)(nextClip.GetTime() - currClipEnd));
						currClip = nextClip;
					}
					else 
					{
						ps.Simulate((float)(currentTime - currClipEnd));
						break;
					}
			}*/

		}

		/*
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
		*/
	}
}
