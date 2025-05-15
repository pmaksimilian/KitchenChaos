using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    // singleton, ker bo samo en DeliveryManager
    // da lahko dostopodamo do njega iz DeliveryCounter
    public static DeliveryManager Instance {get; private set;}

    // seznam vseh receptov
    [SerializeField] private RecipeListSO recipeListSO;

    // seznam jedi, na katere čakajo gosti
    private List<RecipeSO> waitingRecipeSOList;

    // preprost timer
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;

    private int waitingRecipesMax = 4;

    private void Awake() {
        Instance = this;

        // inicializiramo seznam
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f) {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // receptov je lahko največ waitingRecipesMax
            if (waitingRecipeSOList.Count < waitingRecipesMax) {
                // pokličemo random recept iz seznama vseh receptov in ga dodamo na seznam čakajočih
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count)];
                Debug.Log(waitingRecipeSO);
                waitingRecipeSOList.Add(waitingRecipeSO);
            }

        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        // sprehodimo se čez vse čakajoče recepte
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            // preverimo ali ima prinešen krožnik enako sestavin kot trenutni recept
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                // nastavimo flag za najden recept
                bool plateContentsMatchesRecipe = true;

                // sprehodimo se čez vse sestavine v receptu
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    // nastavimo flag za najdeno sestavino
                    bool ingredientFound = false;

                    // sprehodimo se čez vse sestavine na krožniku
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            // preverimo ali sta sestavini isti
                            ingredientFound = true;
                            break;
                        }
                    }

                    // če sestavine nismo našli
                    if (!ingredientFound) {
                        plateContentsMatchesRecipe = false;
                        break;
                    }
                }

                // če smo našli recept lahko prekinemo z izvajanjem for zanke
                if (plateContentsMatchesRecipe) {
                    Debug.Log("Player delivered the recipe");
                    // odstranimo recept iz seznama čakajočih
                    waitingRecipeSOList.RemoveAt(i);
                    return;
                }
            }
        }

        // Če pridemo do sem, recepta nismo našli
        Debug.Log("Player did NOT delivered the correct recipe");
    }

}
