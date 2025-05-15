using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    // hendlanje eventov
    private void Start() {
        DeliveryManager.Instance.OnRecipeListChange += DeliveryManager_OnRecipeListChange;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeListChange(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        // počistimo obstoječe
        foreach (Transform child in container) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        // gremo čez seznam čakajočih reception in jim kreiramo templejte
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }

}
