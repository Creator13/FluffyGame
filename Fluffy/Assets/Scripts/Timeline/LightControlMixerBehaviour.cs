using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    public class LightControlMixerBehaviour : PlayableBehaviour
    {
        private Color lastColor;
        private float lastIntensity;
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var trackBinding = playerData as Light2D;
            if (!trackBinding) return;

            var inputCount = playable.GetInputCount();

            if (inputCount == 0)
            {
                return;
            }
            
            var finalIntensity = 0f;
            var finalColor = Color.black;

            for (var i = 0; i < inputCount; i++)
            {
                var inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<LightControlBehavior>) playable.GetInput(i);
                var inputBehavior = inputPlayable.GetBehaviour();

                finalIntensity += inputBehavior.intensity * inputWeight;
                finalColor += inputBehavior.color * inputWeight;
            }

            trackBinding.intensity = finalIntensity;
            trackBinding.color = finalColor;
        }
    }
}