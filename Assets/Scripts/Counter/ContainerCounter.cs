using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter  {

    // ustvarimo event, ki se sproži, ko igralec "zagrabi" objekt
    public event EventHandler OnPlayerGrabbedObject;

    // definiramo polje, kjer nastavimo kateri kitchenObjectSO se spawna na tem counterju
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // override ker je funkcija definirana v parent classu
    // Interact se kliče v Playerju, na podlagi listenerja za game input
    public override void Interact(Player player) {
        // Nov kitcheObject se ustvari samo, če igralec nima kitchenObjecta
        if (!player.HasKitchenObject()) {
            // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            
            // Sprožimo event, ki bo v ContainerCounterVisual zagnal animacijo
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }

}
