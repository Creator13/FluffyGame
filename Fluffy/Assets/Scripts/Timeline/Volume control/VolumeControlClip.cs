using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Fluffy.Timeline
{
    [Serializable]
    public class VolumeControlClip : PlayableAsset, ITimelineClipAsset
    {
        public VolumeControlBehavior template;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<LightControlBehavior>.Create(graph);
            return playable;
        }
    }
}
