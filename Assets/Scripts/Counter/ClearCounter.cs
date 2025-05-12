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
            // če na counterju je objekt, preverimo, ali ga ima tudi player
            if (player.HasKitchenObject()) {
                // preverimo ali je ta playerjev objekt krožnik
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Torej v primeru, če na counterju je primeren objekt in če igralec ima krožnik, ta objekt dodamo v List na objektu krožnika in ga tukaj destroyamo
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                } else {
                    // player ima objekt, ki ni krožnik
                    // preverimo, ali je objekt na counterju krožnik
                    // pri "out" ne rabimo še enkrat definirat spremenljivke, ker smo jo že zgoraj
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        // Torej v primeru, če na counterju je krožnik in če igralec ima objekt, ki paše na krožnik, ta objekt dodamo v List na objektu krožnika in ga destroyamo pri playerju
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else {
                // če nima, kličemo funkcijo v temu counterju in mu podamo playerja
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        // v obeh primerih kličemo enako funkcijo, samo drugače dostopamo do kitchenObjecta in mu podamo drugačen parameter
    }

}
