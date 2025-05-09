using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    // [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    // override ker je funkcija definirana v parent classu
    public override void Interact(Player player) {

        // preverimo HasKitchenObject, ki je definiran v parent classu
        if(!HasKitchenObject()) {

            // če nima, preverimo, če ga ima player
            if (player.HasKitchenObject()) {

                // če ima, preko playerja dostopamo do tega kitchenObjecta in kličemo funkcijo za nastavitev parenta
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            if (!player.HasKitchenObject()) {
                // če nima, kličemo funkcijo v temu counterju in mu podamo playerja
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        // v obeh primerih kličemo enako funkcijo, samo drugače dostopamo do kitchenObjecta in mu podamo drugačen parameter
    }

}
