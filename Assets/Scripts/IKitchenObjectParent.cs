using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface
// Definicija skupnih pravil za vse classe. Primerna, ko hočemo, da različni classi vsebujejo isto logiko.
// Npr. tako player kot counter imata lahko kitchenObject. Primerno bi bilo tudi za različne primere, s katerimi imaš lahko interakcijo

public interface IKitchenObjectParent {

    // vse definirane lastnosti morajo biti v vseh classih, ki so del tega
    // lahko ima definirane funkcije in property-je, ne pa fieldov

    // točka oz. transform, na kateri bo postavljen kitchenObject
    public Transform GetKitchenObjectFollowTransfom();

    // setter za kitchenObject na tem objektu
    public void SetKitchenObject(KitchenObject kitchenObject);

    // getter za kitchenObject na tem objektu
    public KitchenObject GetKitchenObject();

    // izbris kitchenObjecta
    public void ClearKitchenObject();

    // vrnjen bool, če ima kitchenObject
    public bool HasKitchenObject();
}
