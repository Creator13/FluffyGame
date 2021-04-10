using System.Collections;
using System.Collections.Generic;
using Fluffy;
using UnityEngine;

public class SetMixNMatch : MonoBehaviour
{
    [SerializeField] private FluffyProperties fluffyProperties;
    [SerializeField] private PlushyMixNMatch fluffy;
    
    private void Awake()
    {
        fluffyProperties.Updated += SetFluffy;
    }

    private void Start()
    {
        SetFluffy();
    }

    private void SetFluffy()
    {
        fluffy.SetAccessory(fluffyProperties.Accessory);
        fluffy.SetBaseColor(fluffyProperties.Color);
        fluffy.SetPattern(fluffyProperties.Pattern);
    }

    private void OnDisable()
    {
        fluffyProperties.Updated -= SetFluffy;
    }

    private void OnDestroy()
    {
        fluffyProperties.Updated -= SetFluffy;
    }
}
