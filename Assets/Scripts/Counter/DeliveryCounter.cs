using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {

    public override void Interact (Player player) {
        // Preverimo ali player ima objekt in ali je ta objekt krožnik
        if (player.HasKitchenObject() && player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {

            // preko instance (singleton) kličemo funkcijo v DeliveryManager
            // lahko bi dodali SerializeField ampak ker bo DeliveryManager samo en, samo naredili kar instanco
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

            player.GetKitchenObject().DestroySelf();
        }
    }

    // za puščico in efekt zgoraj si poglej Shaderje in Shader Graph

}
