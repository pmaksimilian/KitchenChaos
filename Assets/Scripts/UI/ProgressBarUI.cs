using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    // ko smo zamenjali prvotni CuttingCounter za interface je bila koda taka, vendar se ta field ne prikaže v unity editorju, ker to ne dela za interface, zato je rešitev drugačna
    // [SerializeField] private IHasProgress hasProgress;

    // naredimo splošen SerializeField in interface shranimo posebej
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;

    // referenca na Image zato potrebujemo using UnityEngine.UI;
    [SerializeField] private Image barImage;


    private void Start() {
        // nadaljevanje logike od zgoraj, nastavimo še interface preko GameObjecta
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError("Game Object " + hasProgressGameObject + " does not have a component that implements IHasProgress");
        }

        // poslušanje eventa s custom args, glej SelectedCounterVisual
        hasProgress.OnProgressChange += HasProgress_OnProgressChange;
        barImage.fillAmount = 0f;

        Hide();
    }

    // IHasProgress se piše z veliko ker gre za nested class, torej class v IHasProgress classu
    private void HasProgress_OnProgressChange(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        barImage.fillAmount = e.progressNormalized;

        // bar je prikazan samo v vmesnih stopnjah
        if(e.progressNormalized == 0f ||e.progressNormalized == 1f) {
            Hide();
        } else {
            Show();
        }
    }

    // preprosti funkciji za skrivanje in prikazovanje tega objekta (gameObject)
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
