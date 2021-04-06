using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fluffy.Timeline
{
    [TrackColor(1f, 0.8482758f, 0f)]
    [TrackClipType(typeof(VolumeControlClip))]
    [TrackBindingType(typeof(AudioSource))]
    public class VolumeControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<VolumeControlBehavior>.Create (graph, inputCount);
        }
    }
}
