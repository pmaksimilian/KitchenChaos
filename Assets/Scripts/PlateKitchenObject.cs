using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    // seznam objektov, ki gredo lahko na krožnik
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    // List objektov, ki bodo na tem krožniku
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        // preverimo ali je tak SO sploh lahko dodan na krožnik
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        }
        // preverimo ali je tak SO že na krožniku
        if (kitchenObjectSOList.Contains(kitchenObjectSO)) {
            return false;
        } else {
            kitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }

}
