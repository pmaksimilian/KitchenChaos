using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour {

    // referenca na glavni objekt
    [SerializeField] private PlatesCounter platesCounter;

    // referenca na CounterTop, da vemo kam naložiti krožnik
    [SerializeField] private Transform counterTopPoint;

    // referenca na prefab, ki ga naložimo
    // to je samo visual, ne dejanski objekt!
    [SerializeField] private Transform plateVisualPrefab;

    // lista plate visualov (rabimo jo za ustrezen prikaz)
    private List<GameObject> plateVisualGameObjectList;

    private void Awake() {
        // inicializiramo List
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start() {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e) {
        // ob eventu ustvarimo nov visual na counter topu
        Transform plateVisualTranform = Instantiate(plateVisualPrefab, counterTopPoint);

        // pohendlamo offset pri novih krožnikih
        float plateOffsetY = .1f;
        plateVisualTranform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        // dodamo zadnji objekt v List
        plateVisualGameObjectList.Add(plateVisualTranform.gameObject);
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e) {
        // dostopamo do zadnjega plate-a na listi
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];

        // odstranimo ga iz liste in destroyamo
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

}
