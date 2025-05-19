using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ker smo klicali PerformInteractiveRebinding
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }
    
    // Za EventHandler je potrebno dodati using System;
    // Gre za standardne C# evente v obliki void SomeMethod(object sender, EventArgs e)
    // Je način, da ostale skripte lahko "poslušajo", če se nekaj zgodi
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    public event EventHandler OnBindingRebind;

    // za options spreminjanje tipk
    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause
    }

    // spremenljivka za shranjevanje PlayerInputActions
    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        // generiranje instance classa PlayerInputActions iz Input System package
        playerInputActions = new PlayerInputActions();

        // naložimo morebitne spremenjene nastavitve
        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        // kličemo Enable na action map Player (ustvarjen v editorju)
        playerInputActions.Player.Enable();

        // Pri "Movement" akciji poslušamo v Update funkciji v Playerju. V tem primeru pa se subscribamo na ta input event. To naredimo s +=
        // performed: When the input is actually considered done (e.g., key pressed)
        // started: When the input starts being triggered
        // canceled: When the input is released or interrupted
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy() {
        // ko zapustimo GameScene se objekt GameInput destroya, ne destroya pa se class PlayerInputActions();
        // zato pride do napake ko pridemo nazaj na GameScene, saj imam dva tovrstna classa in star še vedno posluša dogodke,vendar nima več funkcij
        // rešitvi sta dve, tukaj implementirani obe
        // unsubscribamo se od od funkcij:
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        // in destroyamo tudi PlayerInputActions
        playerInputActions.Dispose();
    }

    // Ko so akcije zgoraj sprožene, se kličeta spodnji funkciji

    // callback funkcija vrne podatke o eventu, zaradi definicije funkcije mora biti navedena
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        // Tole spodaj je isto kot če bi if-al ali je OnInteractAction null. V ifu bi lahko klical brez Invoke (OnInteractAction(this, EventArgs.Empty), to je zaradi vprašaja
        // V drugih skriptah lahko poslušamo ta event s funkcijo, ki mora imeti iste parametre
        // this: kdo je sprožil event
        // EventArgs.Empty: ni drugih informacij zato uporabimo placeholder
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    // funkcija za primerno ureditev inputov za premikanje
    public Vector2 GetMovementVectorNormalized() {
        // dostopamo akcije v zgoraj definirani instanci, v tem primeru akcijo Move v actionu mapu Player
        // ReadValue prebere value inputa, pričakujemo da je tipa Vector2. Oboje nastavimo v editorju, akcije so lahko tudi drugačnega tipa (npr. button)
        // Value je primeren za premikanje, za dolge pritiske. Button je za enkraten dogodek npr. skok ali interakcija
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        

        // Star način input kontrole, sedaj se uporablja PlayerInputActions preko Unity editorja
        // Vector2 inputVector = new Vector2(0, 0);
        // if (Input.GetKey(KeyCode.W)) {
        //     inputVector.y = +1;
        // }
        // if (Input.GetKey(KeyCode.S)) {
        //     inputVector.y = -1;
        // }
        // if (Input.GetKey(KeyCode.A)) {
        //     inputVector.x = -1;
        // }
        // if (Input.GetKey(KeyCode.D)) {
        //     inputVector.x = +1;
        // }

        // .normalized poskrbi za ustrezno hitrost v vse smeri. Če greš diagonalno bi se brez tega igralec premikal hitreje kot v direktne smeri. To se lahko nastavi tudi v editorju pri Processors
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                // kličemo vse tipke vezane na to akcijo in prvo spremenimo v ustrezen prikaz
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    // Action je C# delegate
    // uvoziti je potrebno System
    // ko kličemo to funkcijo, se izvede funkcijo, ki smo jo podali kot argument (poglej OptionsUI)
    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch(binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete((callback) => {
                // Debug.Log(callback.action.bindings[1].path);
                // Debug.Log(callback.action.bindings[1].overridePath);
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                // shranimo spremembe
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

}
