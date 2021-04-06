using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Playables;

namespace Fluffy.Timeline
{
    public class VolumeControlMixerBehaviour : PlayableBehaviour
    {
        float defaultVolume;

        AudioSource m_TrackBinding;
        bool m_FirstFrameHappened;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            var m_TrackBinding = playerData as Light2D;
            if (!m_TrackBinding) return;

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
                var inputPlayable = (ScriptPlayable<VolumeControlBehavior>) playable.GetInput(i);
                var inputBehavior = inputPlayable.GetBehaviour();

                finalColor += inputBehavior.color * inputWeight;
            }

            m_TrackBinding.intensity = finalIntensity;
            m_TrackBinding.color = finalColor;
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            m_FirstFrameHappened = false;

            if (m_TrackBinding == null)
                return;

            m_TrackBinding.volume = defaultVolume;
        }
    }
}