using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {

    // Hendlanje animacije zato je podobno kot v PlayerAnimator
    private const string CUT = "Cut";

    // polje za CuttingCounter, kateremu pripada ta visual
    [SerializeField] private CuttingCounter cuttingCounter;

    // referenca na Animator komponento
    private Animator animator;

    private void Awake() {
        // preko reference dobimo komponento
        animator = GetComponent<Animator>();
    }

    private void Start() {
        // dodamo listener
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    // zadeve se na podoben način hendlajo v Player in GameInput
    private void CuttingCounter_OnCut(object sender, System.EventArgs e) {
        // Klic se izvaja preko stringa, zato ga definiramo zgoraj, da je manj možnosti za napako
        animator.SetTrigger(CUT);
    }
}
