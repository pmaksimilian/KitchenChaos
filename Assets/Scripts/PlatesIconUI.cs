using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// skripta, ki bo hendlala logiko IconTemplate
// slednjega nismo dali v prefab zato, ker se uporablja samo tukaj
// postavitev ikon je urejena z Grid Layout Group (obstajata tudi horizontalna in vertikalna opcija)
// ozadje ikone je urejeno s krogom, ki je uvožen preko paketa 2D Sprite

public class PlatesIconUI : MonoBehaviour {

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    private void Awake() {
        // na začetku skrijemo template
        iconTemplate.gameObject.SetActive(false);
    }

    // poslušamo, kdaj je dodana sestavina
    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        // destroyamo vse prejšnje ikone, razn template-a
        foreach(Transform child in transform) {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }

        // kreiramo nove ikone in jim dodamo ustrezne sprite
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
            Transform iconTransform = Instantiate(iconTemplate, transform);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<PlateIconsSingleUI>().SetKitchenObjectSO(kitchenObjectSO);
        }
    }

}
