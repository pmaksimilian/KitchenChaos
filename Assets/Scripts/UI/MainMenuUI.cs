using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// uvozimo za SceneManager
using UnityEngine.SceneManagement;

// uvozimo za Button
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });

        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        // reset timeScale, ker če pridemo sem iz game pause potem bi še vedno vse ostalo pri miru
        Time.timeScale = 1f;
    }
}
