using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// S tem urejamo inheritance, vsi ostali counterji bodo podaljšek tega
public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    // za zvok ustvarimo static event, da se naredi samo en na class, ne za vsako instanco
    public static event EventHandler OnAnyObjectPlacedHere;
    // funkcija za reset static podatkov, klicana iz ResetStaticDataManager
    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    // definiramo preprost Transfom za pult, samo točka na katero se stvari nalagajo
    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    // virtual definiramo, ko hočemo, da jih child class-i overridajo
    // lahko bi uporabili abstract in bi bilo enako kot pri interfacu
    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact()");
    }
    public virtual void InteractAlternate(Player player) {
        //Debug.LogError("BaseCounter.InteractAlternate()");
    }


    /* FUNKCIJE ZA INTERFACE IKitchenObjectParent */

    // točka oz. transform, na kateri bo postavljen kitchenObject
    public Transform GetKitchenObjectFollowTransfom() {
        return counterTopPoint;
    }

    // setter za kitchenObject na counterju
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    // getter za kitchenObject na counterju
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    // izbris kitchenObjecta
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    // vrnjen bool, če ima kitchenObject
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

}
