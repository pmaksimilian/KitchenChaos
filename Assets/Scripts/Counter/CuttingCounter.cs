using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

    // ustvarimo event za progress (podrobnejša razlaga v Player)
    // kličemo tip argumentov, ki je definiran v IHasProgress
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChange;

    // event za sproženje animacije
    public event EventHandler OnCut;

    // za zvok ustvarimo static event, da se naredi samo en na class, ne za vsako instanco (čeprav bo v resnici delal isto kot OnCut)
    public static event EventHandler OnAnyCut;

    // V editorju dodamo vse možne recepte v ta field
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    // Zaradi logike večih korakov (več pritiskov za rezanje)
    private int cuttingProgress;

    // Navadna Interact funkcija je enaka, kot pri ClearCounter, se pravi odlaganje in prijemanje, z enim dodatnim preverjanjem
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // preko playerja in njegovega kitchenObjecta preverimo, če je input ustrezen za rezanje
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // nastavimo progress, ki ga povečamo ob vsakem dejanju in sprožimo event
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
        } else {
            if (player.HasKitchenObject()) {
                // preverimo ali je playerjev objekt krožnik
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Torej v primeru, če na counterju je objekt in če igralec ima krožnik, ta objekt dodamo v List na objektu krožnika in ga tukaj destroyamo
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    } 

    // Funkcionalnost druge tipke, urejena v InteractAlternate actionu
    public override void InteractAlternate(Player player) {
        // Če counter ima objekt, ki je primeren za rezanje, ga destroyamo s klicem funkcije od KitchenObjecta in ustvarimo novega (narezanega)
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            // Logika za več korakov rezanja s sprožitvijo eventa
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            OnAnyCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            // rezanje zaključimo šele, ko je izvedeno dovolj korakov
            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {

                // Najdemo ustrezen output. Preko kitchenObjecta na tem counterju dostopamo do ustreznega SO in ga podamo kot argument
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                GetKitchenObject().DestroySelf();

                // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }

        }
    }

    // preverjamo, ali živilo obstaja v arrayu receptov
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    // Funkcija, ki v arrayu najde primeren input in vrne ujemajoč output
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        } else {
            return null;
        }
    }

    // Poenotena funkcija, ki vrne cel najden SO
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}
