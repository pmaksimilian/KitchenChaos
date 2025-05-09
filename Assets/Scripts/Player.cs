using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour je razred v Unity component
// je interface IKitchenObjectParent torej mora imeti vse zahteve
// class je lahko podaljšek samo enega drugega classa, pri interfacu pa te omejitve ni
public class Player : MonoBehaviour, IKitchenObjectParent {

    // Singleton pattern
    // Poskrbi, da obstaja le ena instanca tega classa in omogoči globalni dostop
    // v osnovi bi lahko bila {get; set;} ampak tako smo set nastavili na private
    public static Player Instance { get; private set; }

    // če hočemo pošiljati dodatne podatke naredimo event na tak način
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    // in tako definiramo kateri podatki se pošljejo
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    // [SerializeField] poskrbi, da se tudi private polja prikažejo v editorju. Public polja se prikažejo tudi brez tega. To je polje za hitrost premikanja.
    [SerializeField] private float moveSpeed = 7f;

    // referenca na GameInput skripto
    [SerializeField] private GameInput gameInput;

    // referenca na LayerMask za raycastHit pri interakciji s counterji
    [SerializeField] private LayerMask countersLayerMask;

    // referenca na Transform za pozicioniranje KitchenObjecta
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;

    // spremenljivka za zadnjo smer dejanskega premikanja, da je možna interakcija
    private Vector3 lastInteractDir;

    // izbran counter, če obstaja
    private BaseCounter selectedCounter;

    // kitchenObject, če ga player ima
    private KitchenObject kitchenObject;

    // Unity funkcija "Awake" se kliče najprej, pred "Start"
    private void Awake() {
        // nastavimo singleton instanco, z log preverjanjem, če pride do napake
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance.");
        }
        Instance = this;
    }

    // Event listenerje moramo dodati na Start, saj se morajo sprožiti kasneje 
    private void Start() {
        // event listenerji za funkcije v skripti GameInput, dodajo se na enak način kot tam
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    // funkcije morajo imeti enake argumente kot tiste, katere poslušajo
    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        // Preverimo, če je obstaja counter, s katerim imamo lahko interakcijo (to se nastavlja v HandleInteractions)
        if (selectedCounter != null) {

            // Interakcije so definirane v posameznih counterjih, saj so lahko različne glede na vrsto counterja
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    // Unity funkcija "Update" se kliče vsak frame, primerna je za premikanje in poslušanje inputov
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    // getter za dostop do isWalking, ker je private
    public bool IsWalking() {
        return isWalking;
    }

    // Handlanje interakcij, torej v primeru, ko smo dovolj blizu in usmerjeni v objekt, da moramo prikazati možno interakcijo
    private void HandleInteractions() {
        // do tega pride pri premikanju zato pridobimo smer premikanja enako kot v HandleMovement
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Moramo nastaviti zadnjo smer premikanja, sicer interakcija ne bo možna, ko bomo stali pri miru
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        // sprožimo žarek z Physics.Raycast(začetna pozicija, smer, informacije o tem kar smo zadel, max razdalja žarka, layer mask navaja kateri objekti štejejo pri preverbi)
        // layer nastavimo pri counter objektu, lahko dodajamo nove layerje
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {

            // raycastHit je vrnjen morebitni zadetek, preverimo če je komponenta vrste BaseCounter
            // TryGetComponent je podobno kot GetComponent, samo da že opravi null check
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {

                // če smo naleteli na counter, ki ni enak že nastavljenemu, ga nastavimo
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }

            // če smo naleteli na object ki ni BaseCounter ali pa objekta sploh ni, to ustrezno nastavimo 
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    // Handlamo premikanje, torej po pritiskih na gumbe v katero smer igralec premika in rotira
    private void HandleMovement() {
        // kličemo funkcijo iz GameInput, ki vrne Vector2
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // Spremenimo v Vector3, ker gre za 3D svet. Y v 3D je gor/dol zato je 0f
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Najprej preverimo, ali do premika sploh lahko pride
        // Izračun kako daleč se premaknemo, glede na frame-rate, pomnoženo z moveSpeed, da lahko ročno spreminjamo
        float moveDistance = Time.deltaTime * moveSpeed;

        // Velikost igralca (to predhodno "zmerimo")
        float playerRadius = .7f;
        float playerHeight = 2f;

        // Physics.CapsuleCast sproži kapsulo in vrne informacijo o morebitnih zadetkih. Argumenti:
        // 1. point1: center spodnjega roba kapsule
        // 2. point2: center zgornjega roba kapsule (Vector3.up pomeni Vector3(0, 1, 0))
        // 3. radius: radius kapsule
        // 4. direction: smer premikanje
        // 5. maxDistance: največja razdalja premika
        // Če vrne "false" pomeni, da nismo zadeli ničesar v podani razdalji premika
        bool canMove = !Physics.CapsuleCast(
                            transform.position,
                            transform.position + Vector3.up * playerHeight,
                            playerRadius,
                            moveDir,
                            moveDistance
                        );

        // Zgornji test ne omogoča premikanja diagonalno kadar smo obrnjeni proti prepreki, zato so potrebni nadaljnji if stavki
        if (!canMove) {
            // Najprej preverimo samo premikanje po X osi
            // Normaliziramo, da se izenači hitrost z normalnim premikanjem (v diagonalnem premikanju je namreč manjša od 1)
            // moveDir.x != 0 preveri, če smo se sploh poizkušali premikati v to smer
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            // Če se lahko premikamo, spremenimo smer premikanja.
            if (canMove) {
                moveDir = moveDirX;
            } else {
                // Če ne, preverimo še os Z, na enak način
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        // Po vseh testih izvedemo premik, če do njega lahko pride. Pozicija objekta dodamo razdaljo v določeno smer.
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        // nastavimo isWalking glede na smer premikanja (Vector3.zero pomeni Vector3(0,0,0))
        isWalking = moveDir != Vector3.zero;

        // Urejamo rotacijo objekta:
        // transform.forward je trenutna sprednja stran objekta (kam gleda)
        // .Slerp poskrbi ze gladko premikanje (prvi argument je kam trenutno gleda, drugi kam se mora obrniti, tretji pa hitrost premikanja).
        // Time.deltaTime makes it frame-rate independent (smooth across all frame rates).
        // ker je samo deltaTime bilo prepočasno smo pohitrili z rotateSpeed
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    // Funkcija, ki nastavlja izbrani counter
    private void SetSelectedCounter(BaseCounter selectedCounter) {
        // this.selectedCounter je tisti, definiran v classu, medtem ko je drugi ta v parametrih
        // če nimamo podvojenega poimenovanja potem this. ni potreben
        this.selectedCounter = selectedCounter;

        // sprožimo event definiran zgoraj, z definiranim EventArgs preko katerega pošljemo podatke (primer podobnega sprožanja so Interact v GameInputu)
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }


    /* FUNKCIJE ZA INTERFACE IKitchenObjectParent */

    // točka oz. transform, na kateri bo postavljen kitchenObject
    public Transform GetKitchenObjectFollowTransfom() {
        return kitchenObjectHoldPoint;
    }

    // setter za kitchenObject na playerju
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    // getter za kitchenObject na tem obplayerjujektu
    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    // izbris kitchenObjecta
    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    // vrnjen bool, če ima kitchenObject
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
