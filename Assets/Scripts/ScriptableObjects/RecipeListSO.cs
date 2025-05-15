using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ker rabimo samo enega, ga lahko zakomentiramo, potem ko smo ga ustvarili
// [CreateAssetMenu()]
public class RecipeListSO : ScriptableObject {
    
    // seznam vseh receptov, iz katerih se potem lahko spawnajo v DeliveryManagerju
    // na ta način imamo seznam vseh na enem mestu, če bi ga rabili še kje drugje
    public List<RecipeSO> recipeSOList;

}
