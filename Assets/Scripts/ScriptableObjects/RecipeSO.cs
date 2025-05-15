using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject {

    // SO za recepte, torej preprost objekt z imenom in seznamom živil
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;

}
