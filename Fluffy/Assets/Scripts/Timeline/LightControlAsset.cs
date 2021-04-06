using UnityEngine;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    public class LightControlAsset : PlayableAsset
    {
        public Color color;
        public float intensity;

        public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<LightControlBehavior>.Create(graph);
      
            var lightControlBehaviour = playable.GetBehaviour();
            lightControlBehaviour.color = color;
            lightControlBehaviour.intensity = intensity;
 
            return playable;
        }
    }
}
