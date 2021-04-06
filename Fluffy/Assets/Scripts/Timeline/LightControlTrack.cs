using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fluffy.Timeline
{
    [TrackClipType(typeof(LightControlAsset))]
    [TrackBindingType(typeof(Light2D))]
    public class LightControlTrack: TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<LightControlMixerBehaviour>.Create(graph, inputCount);
        }
    }
}
