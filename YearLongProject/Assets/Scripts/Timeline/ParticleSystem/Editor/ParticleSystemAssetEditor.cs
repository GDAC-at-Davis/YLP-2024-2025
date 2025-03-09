using Timeline.ParticleSystemTimeline;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(ParticleSystemAsset))]
public class ParticleSystemAssetEditor : ClipEditor 
{

	// The refresh in both of these functions is necessary
	// The timeline need to keep track of changes to all the clips

	public override void OnClipChanged(TimelineClip clip)
	{
		TimelineEditor.Refresh(RefreshReason.ContentsModified);
	}

	public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
	{
		TimelineEditor.Refresh(RefreshReason.ContentsAddedOrRemoved);
	}
}
