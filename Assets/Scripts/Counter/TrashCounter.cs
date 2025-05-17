using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter {

    // za zvok ustvarimo static event, da se naredi samo en na class, ne za vsako instanco
    public static event EventHandler OnAnyObjectTrashed;
    // funkcija za reset static podatkov, klicana iz ResetStaticDataManager
    // new ker ista funkcija že obstaja na BaseCounter
    new public static void ResetStaticData() {
        OnAnyObjectTrashed = null;
    }

    // samo preprost izbris kitchen objecta, če ga igralec ima
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            player.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }

}
