using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurningUI : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnProgressChange += stoveCounter_OnProgressChange;

        Hide();
    }

    private void stoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        float burnShowProgressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        if(show) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

}
