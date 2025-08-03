using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShameSO", menuName = "Scriptable Objects/ShameSO")]
public class ShameSO : ScriptableObject
{
    [SerializeField] string shameType;
    [SerializeField] int weightValue;
    [SerializeField] List<string> messages;

    public string ShameType { get; }
    public int WeightValue { get; set; }

    public List<string> Messages { get; }
    
}
