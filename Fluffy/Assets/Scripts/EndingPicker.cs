using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class EndingPicker : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner.AddCommandHandler<int>("SetEnding", SetEnding);
    }

    private void SetEnding(int ending)
    {
        foreach (Transform t in transform)
        {
            var obj = t.gameObject;
            obj.SetActive(obj.GetComponent<Polaroid>().belongsToEnding == ending);
        }
    }
}
