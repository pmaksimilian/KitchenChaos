using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// skripta, dodana KitchenObject prefabom in je preko njega povezana z SO
public class KitchenObject : MonoBehaviour {
    
    // definiramo polje za SO in spodaj dodamo getter za dostop
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // polje za starša tega kitchenObjecta in spodaj getter
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }

    // Nastavitev parenta od tega kitchenObjecta
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        // Če ga predhodno ima, ga pobrišemo
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        // Nastavimo referenco preko paramsa
        this.kitchenObjectParent = kitchenObjectParent;
        
        // Preverimo, če nov parent slučajno že ima kitchenObject
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("Counter already has object");
        }

        // Kličemo funkcijo. ki uredi kitchenObject tudi na parentu
        kitchenObjectParent.SetKitchenObject(this);

        // "transform" je Transform komponenta tega objekta. Definira lokacijo, velikost in rotacijo objekta 
        // Nastavitev transform.parent naredi trenutni objekt child od enega drugega Transforma. V tem primeru bo kitchenObject postal otrok svojega parenta tudi v transform smislu, se pravi bo prilepljen nanj in se bo vrtel z njim.
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransfom();
        // Nastavimo še relativno pozicijo tega objekta na 0, da ne pride do čudnega obnašanja. Njegovo pozicijo ureja parent.
        transform.localPosition = Vector3.zero;
    }

    // Funkcija, ki uniči trenutni objekt. Pred tem ga tudi odstrani iz parenta
    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();

        // "gameObject" je dedovan iz "MonoBehavour" in je referenca na GameObject na katerega je naložena ta skripta, v tem primeru "KitchenObject"
        Destroy(gameObject);
    }


    // V tem primeru gre za metodo static, kar pomeni, da pripada direktno classu KitchenObject in ne posamezni instanci. Zato se jo tudi kliče preko imena classa torej KitchenObject.SpawnKitchenObject()
    // Funkcija ustvari nov KitchenObject. Za to potrebuje argumenta katerega in kje
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {

        // Podamo referenco na prefab (a ready-made GameObject) in s funkcijo "Instantiate" kloniramo ta prefab v sceno. To vrne Transform komponento.
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);

        // Preko transforma dobimo komponento. S tem imamo referenco tudi na skripto, ne zgolj na objekt
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        // Kličemo našo metodo, ki nastavi parenta od objekta
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

}
