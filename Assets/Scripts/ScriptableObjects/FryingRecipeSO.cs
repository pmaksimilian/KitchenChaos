using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scriptable object (SO)
// za razlago glej KitchenObjectSO
[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject {

    // Ponazarja dejanje, input so surova živila, output spečena
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTimerMax;

}
