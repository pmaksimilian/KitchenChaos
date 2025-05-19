using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    // ustvarimo event za progress
    // kličemo tip argumentov, ki je definiran v IHasProgress
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChange;

    // ustvarimo event, da sporočimo v katerem stanju je pečica
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    
    // enum je zato, da nadziramo kakšne so možnosti in je manj možnosti za napake
    // mora biti public ker je zgoraj vključen kot public v EventArgs
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {
        // Karkoli se izvaja samo, če objekt ima kitchen object
        if (HasKitchenObject()) {
            // različna dejanja glede na state objekta
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    // preprost timer
                    fryingTimer += Time.deltaTime;

                    // sprožimo event za progressBar
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    // kuhanje zaključimo, ko je timer večji od nastavljenega
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        GetKitchenObject().DestroySelf();

                        // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        // ko je pečeno, spremenimo state, ponastavimo timer in naložimo burning recipe
                        state = State.Fried;
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        // sprožimo event ua state change, ki ga poslušajo visualsi
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        
                    }
                    break;
                case State.Fried:
                    // preprost timer
                    burningTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
                    });

                    // kuhanje zaključimo, ko je timer večji od nastavljenega
                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        GetKitchenObject().DestroySelf();

                        // Kličemo funkcijo v "KitchenObject" skripti. Ker je static jo kličemo preko imena classa
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        // sprožimo event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        // na koncu samo skrijemo progress bar, ker ni več potreben
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    // Navadna Interact funkcija je enaka, kot pri ClearCounter, se pravi odlaganje in prijemanje, z enim dodatnim preverjanjem
    public override void Interact(Player player) {
        if(!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                // preko playerja in njegovega kitchenObjecta preverimo, če je input ustrezen za ta pečenje
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // nastavimo recept, da lahko do njega dostopamo tudi v drugih funkcijah
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    // ko objekt dobi kitchen object se ustrezno nastavi state in ponastavi timer
                    state = State.Frying;
                    fryingTimer = 0f;

                    // sprožimo event
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = state
                    });

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
        } else {
            if (player.HasKitchenObject()) {
                // preverimo ali je ta playerjev objekt krožnik
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Torej v primeru, če na counterju je objekt in če igralec ima krožnik, ta objekt dodamo v List na objektu krožnika in ga tukaj destroyamo
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();

                        // ponovimo isto logiko kot je spodaj, kjer player direktno pobere objekt
                        state = State.Idle;

                        // sprožimo event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });

                        // če player pobere objekt se progress bar ponastavi
                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                }
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                // sprožimo event
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = state
                });

                // če player pobere objekt se progress bar ponastavi
                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
        }
    }

    // preverjamo, ali živilo obstaja v arrayu receptov
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    // Funkcija, ki v arrayu najde primeren input in vrne ujemajoč output
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        } else {
            return null;
        }
    }

    // Poenotena funkcija, ki vrne cel najden SO
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried() {
        return state == State.Fried;
    }
}
