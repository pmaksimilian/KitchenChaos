using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    // ustvarimo enum in polje v editorju (ki bo selectbox)
    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] private Mode mode;
    
    // LateUpdate se izvede po Update
    // uporabno pri premikih, npr. če menjaš pogled po določenem dogodku, ki se nekje zgodi na Update
    private void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                // transform.LookAt => this transform gleda proti navedenemu transformu ali točki
                // Camera.main.transform => transform glavne kamere
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                // da dobimo smer od kamere od pozicije tega transforma odštejemo pozicijo transforma glavne kamere
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                // da objekt gleda v nasprotno smer potem njegovi poziciji prištejemo smer od kamere
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                // namesto da je objekt obrnjen direktno proti kameri, ima samo sprednjo stran obrnjeno v isto smer kot kamera
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                // še obrnjen primer
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
