using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    // V editorju dodamo vse možne recepte v ta field
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    // Navadna Interact funkcija je enaka, kot pri ClearCounter, se pravi odlaganje in prijemanje, z enim dodatnim preverjanjem
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // preko playerja in njegovega kitchenObjecta preverimo, če je input ustrezen za rezanje
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
        } else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    } 

    // Funkcionalnost druge tipke, urejena v InteractAlternate actionu
    public override void InteractAlternate(Player player) {
        // Če counter ima objekt, ki je primeren za rezanje, ga destroyamo s klicem funkcije od KitchenObjecta in ustvarimo novega (narezanega)
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            // Pred tem najdemo ustrezen output. Preko kitchenObjecta na tem counterju dostopma do ustreznega SO in ga podamo kot argument
            KitchenObjectSO outputKitchenObjectSO = GetOuputForInput(GetKitchenObject().GetKitchenObjectSO());

            GetKitchenObject().DestroySelf();

            // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
        }
    }

    // preverjamo, ali živilo obstaja v arrayu receptov
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return true;
            }
        }
        return false;
    }

    // Funkcija, ki v arrayu najde primeren input in vrne ujemajoč output
    private KitchenObjectSO GetOuputForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO.output;
            }
        }
        return null;
    }

}
