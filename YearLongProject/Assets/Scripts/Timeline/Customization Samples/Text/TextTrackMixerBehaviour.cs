#if TEXT_TRACK_REQUIRES_TEXTMESH_PRO

using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Samples
{
    // The runtime instance of a the TextTrack. It is responsible for blending and setting the final data
    // on the Text binding
    public class TextTrackMixerBehaviour : PlayableBehaviour
    {
        private Color m_DefaultColor;
        private float m_DefaultFontSize;
        private string m_DefaultText;

        private TMP_Text m_TrackBinding;

        // Called every frame that the timeline is evaluated. ProcessFrame is invoked after its' inputs.
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            SetDefaults(playerData as TMP_Text);
            if (m_TrackBinding == null)
            {
                return;
            }

            int inputCount = playable.GetInputCount();

            Color blendedColor = Color.clear;
            var blendedFontSize = 0f;
            var totalWeight = 0f;
            var greatestWeight = 0f;
            string text = m_DefaultText;

            for (var i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<TextPlayableBehaviour>)playable.GetInput(i);
                TextPlayableBehaviour input = inputPlayable.GetBehaviour();

                blendedColor += input.color * inputWeight;
                blendedFontSize += input.fontSize * inputWeight;
                totalWeight += inputWeight;

                // use the text with the highest weight
                if (inputWeight > greatestWeight)
                {
                    text = input.text;
                    greatestWeight = inputWeight;
                }
            }

            // blend to the default values
            m_TrackBinding.color = Color.Lerp(m_DefaultColor, blendedColor, totalWeight);
            m_TrackBinding.fontSize = Mathf.RoundToInt(Mathf.Lerp(m_DefaultFontSize, blendedFontSize, totalWeight));
            m_TrackBinding.text = text;
        }

        // Invoked when the playable graph is destroyed, typically when PlayableDirector.Stop is called or the timeline
        // is complete.
        public override void OnPlayableDestroy(Playable playable)
        {
            RestoreDefaults();
        }

        private void SetDefaults(TMP_Text text)
        {
            if (text == m_TrackBinding)
            {
                return;
            }

            RestoreDefaults();

            m_TrackBinding = text;
            if (m_TrackBinding != null)
            {
                m_DefaultColor = m_TrackBinding.color;
                m_DefaultFontSize = m_TrackBinding.fontSize;
                m_DefaultText = m_TrackBinding.text;
            }
        }

        private void RestoreDefaults()
        {
            if (m_TrackBinding == null)
            {
                return;
            }

            m_TrackBinding.color = m_DefaultColor;
            m_TrackBinding.fontSize = m_DefaultFontSize;
            m_TrackBinding.text = m_DefaultText;
        }
    }
}

#endif