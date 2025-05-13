using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    // ustvarjanje eventa, da je bila sestavina (in katera) dodana na ta krožnik
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }

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

            // sprožimo tudi event
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return kitchenObjectSOList;
    }
}
