using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object (SO)
// za razlago glej KitchenObjectSO
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject {

    // Ponazarja dejanje, input so cela živila, output razrezana
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public int cuttingProgressMax;

}
