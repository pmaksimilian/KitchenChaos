using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To je scriptable object (SO)
// Perfektni za primere, ko imam več instanc istega tipa (npr. več  sestavin ali orožja)
// SO je samo za shranjevanje podatkov, ki je lahko uporabljen na več različnih objektih, ne obstaja dejansko v sceni

// to ni MonoBehaviour
// CreateAssetMenu ga doda v menu v editorju
[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject {
    
    // definiramo polja, ki jih rabimo, ki so lahko public

    // ta objekt bo imel prefab, sprite in ime
    public Transform prefab;
    public Sprite sprite;
    public string objectName;

    // preko editorja nato dodajamo različne SO katere povežemo z ustreznimi prefabi
}
