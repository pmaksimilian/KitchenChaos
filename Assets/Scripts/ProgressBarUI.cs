using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {

    [SerializeField] private CuttingCounter cuttingCounter;

    // referenca na Image zato potrebujemo using UnityEngine.UI;
    [SerializeField] private Image barImage;


    // poslušanje eventa s custom args, glej SelectedCounterVisual
    private void Start() {
        cuttingCounter.OnProgressChange += CuttingCounter_OnProgressChange;
        barImage.fillAmount = 0f;

        Hide();
    }

    // CuttingCounter se piše z veliko ker gre za nested class, torej class v CuttingCounter classu
    private void CuttingCounter_OnProgressChange(object sender, CuttingCounter.OnProgressChangedEventArgs e) {
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
