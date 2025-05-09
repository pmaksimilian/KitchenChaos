using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    // Navadna Interact funkcija je enaka, kot pri ClearCounter, se pravi odlaganje in prijemanje
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    } 

    // Funkcionalnost druge tipke, urejena v InteractAlternate actionu
    public override void InteractAlternate(Player player) {
        if (HasKitchenObject()) {
            // Če counter ima objekt, ga destroyamo s klicem funkcije od KitchenObjecta in ustvarimo novega (narezanega)
            GetKitchenObject().DestroySelf();

            // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }

}
