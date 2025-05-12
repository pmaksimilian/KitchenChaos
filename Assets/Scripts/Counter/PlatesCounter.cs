using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    // nastavimo timer, glede na katerega se naloži nov krožnik
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    // maksimalno število naloženih krožnikov
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;
        Debug.Log(spawnPlateTimer);
        if (spawnPlateTimer > spawnPlateTimerMax) {
            // ko mine timer, ga nastavimo na 0 in preverimo, če lahko naložimo nov krožnik
            spawnPlateTimer = 0f;

            // tukaj pohendlamo samo število naloženih krožnikov, vizualni prikaz je v ločeni skripti
            // ne bomo dejansko naložili krožnika kot kitchen objecta, samo vizualni prikaz, spawna se, ko ga igralec zagrabi
            if (platesSpawnedAmount < platesSpawnedAmountMax) {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        // Nov kitcheObject se ustvari samo, če igralec nima kitchenObjecta
        if (!player.HasKitchenObject()) {
            if (platesSpawnedAmount  > 0) {
                platesSpawnedAmount--;

                // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }

}
