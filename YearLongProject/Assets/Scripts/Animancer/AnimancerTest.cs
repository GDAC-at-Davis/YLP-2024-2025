using Animancer;
using UnityEngine;

public class AnimancerTest : MonoBehaviour
{
    [SerializeField]
    private AnimancerComponent _animancerComponent;

    [SerializeField]
    private TransitionAsset _test1;

    [SerializeField]
    private TransitionAsset _test2;

    [SerializeField]
    private float _param;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _animancerComponent.Play(_test1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _animancerComponent.Play(_test2);
        }

        var mixer = _test1.Transition as LinearMixerTransition;

        if (mixer != null && mixer.State != null)
        {
            mixer.State.Parameter = _param;
        }
    }
}