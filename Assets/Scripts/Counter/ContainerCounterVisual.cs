using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour {

    // Hendlanje animacije zato je podobno kot v PlayerAnimator
    private const string OPEN_CLOSE = "OpenClose";

    // polje za ContainerCounter, kateremu pripada ta visual
    [SerializeField] private ContainerCounter containerCounter;

    // referenca na Animator komponento
    private Animator animator;

    private void Awake() {
        // preko reference dobimo komponento
        animator = GetComponent<Animator>();
    }

    private void Start() {
        // dodamo listener
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    // zadeve se na podoben način hendlajo v Player in GameInput
    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e) {
        // Klic se izvaja preko stringa, zato ga definiramo zgoraj, da je manj možnosti za napako
        animator.SetTrigger(OPEN_CLOSE);
    }
}
