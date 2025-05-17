using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// static eventi in ostali podatko se ne destroyajo skupaj z objekti, zato je to potrebno urediti ročno
// problem nastane ko gremo iz igre v main menu in potem spet v igro, static podatki se začnejo podvajat
// izvajati se mora na MainMenuScene
public class ResetStaticDataManager : MonoBehaviour {

    private void Awake() {
        BaseCounter.ResetStaticData();
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }

}
