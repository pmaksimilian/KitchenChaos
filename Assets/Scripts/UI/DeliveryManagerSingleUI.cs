using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// uvozimo za TextMeshProUGUI
using TMPro;

public class DeliveryManagerSingleUI : MonoBehaviour {

    // ta vrsta je za UI, če je tekst v igri je TextMeshPro
    [SerializeField] private TextMeshProUGUI recipeNameText;

    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSO) {
        recipeNameText.text = recipeSO.recipeName;

        // počistimo obstoječe
        foreach (Transform child in iconContainer) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList) {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }

}
