using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    public static GameInput Instance { get; private set; }
    
    // Za EventHandler je potrebno dodati using System;
    // Gre za standardne C# evente v obliki void SomeMethod(object sender, EventArgs e)
    // Je način, da ostale skripte lahko "poslušajo", če se nekaj zgodi
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    // spremenljivka za shranjevanje PlayerInputActions
    private PlayerInputActions playerInputActions;

    private void Awake() {
        Instance = this;

        // generiranje instance classa PlayerInputActions iz Input System package
        playerInputActions = new PlayerInputActions();

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

}
