using UnityEngine;
using UnityEngine.Playables;

public class Sequencer : MonoBehaviour
{
    [SerializeField] private AudioSource backgroundAudio;

    [Header("Timelines")] [SerializeField] private PlayableDirector intro;
    
    private void Awake()
    {
        SetInitialState();
    }

    private void SetInitialState()
    {
        
    }
}
