using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// skripta za vizualni prikaz izbranega counterja, dodana na "Selected" v counterju
public class SelectedCounterVisual : MonoBehaviour {

    // polje, kamor dodamo counter, kateremu pripada ta Selected
    [SerializeField] private BaseCounter baseCounter;

    // to so objekti v Selected, ki se morajo prikazati, če je counter selected
    [SerializeField] private GameObject[] visualGameObjectArray;
    
    // ker kličemo instanco damo v Start, ker se tam definira v Awake
    private void Start() {
        // preko singleton instance dodamo listenerje za event, sprožen v SetSelectedCounter
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    // funkcije sprožene v listenerju, v tem primeru imajo poseben EventArgs class
    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        // preverimo, ali je izbran counter enak temu counterju
        // to delamo zato, ker poslušamo v vseh selected skriptah, zato se to sproži vsakič, ko player nastavlja selectedCounter
        if (e.selectedCounter == baseCounter) {
            Show();
        } else {
            Hide();
        }
    }


    // V obeh funkcija se sprehodimo čez vse objekte in jim ustrezno nastavimo aktivnost. To isto aktivnost se lahko nastavlja v editorju s kljukico zgoraj
    private void Show() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide() {
        foreach (GameObject visualGameObject in visualGameObjectArray) {
            visualGameObject.SetActive(false);
        }
    }
}
