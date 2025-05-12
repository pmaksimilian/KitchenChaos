using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    // "struct" je poseben value type, ki lahko grupira različne spremenljivke
    // podoben kot "class" le da ni referenca ampak se kopira in ima boljši performance
    // primeren za manjše objekte
    // če ga hočemo videti v editorju pred njega damo [Serializable] za kar potrebujemo "using System"
    [Serializable]
    public struct KitchenObjectSO_GameObject {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }
    // v tem primeru smo to ustvarili za povezavo med kitchenObjectSO, ki se bo dodal na krožnik in gameObjectom v PlateCompleteVisual, torej med posameznimi childi tega visuala

    
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    [SerializeField] private List<KitchenObjectSO_GameObject> KitchenObjectSOGameObjectList;

    // poslušamo krožnikov event za dodajanje sestavin
    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        // nastavimo vse gameObjecte na neaktivne
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in KitchenObjectSOGameObjectList) {
            kitchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        // sprehodimo se čez povezave v zgoraj kreiranem Listu in primerni gameObject is tega visuala nastavimo na aktivnea
        foreach (KitchenObjectSO_GameObject kitchenObjectSOGameObject in KitchenObjectSOGameObjectList) {
            if (kitchenObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO) {
                kitchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
