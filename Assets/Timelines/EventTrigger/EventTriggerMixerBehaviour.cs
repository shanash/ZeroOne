using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EventTriggerMixerBehaviour : PlayableBehaviour
{
    //// NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    //public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    //{
    //    int inputCount = playable.GetInputCount ();

    //    for (int i = 0; i < inputCount; i++)
    //    {
    //        float inputWeight = playable.GetInputWeight(i);
    //        ScriptPlayable<EventTriggerBehaviour> inputPlayable = (ScriptPlayable<EventTriggerBehaviour>)playable.GetInput(i);
    //        EventTriggerBehaviour input = inputPlayable.GetBehaviour ();
            
    //        // Use the above variables to process each frame of this playable.
    //        if (inputWeight > 0f)
    //        {

    //        }
            
    //    }
    //}

    //public override void OnBehaviourPlay(Playable playable, FrameData info)
    //{
    //    base.OnBehaviourPlay(playable, info);
    //}
}
