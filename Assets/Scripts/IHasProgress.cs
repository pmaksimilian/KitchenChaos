using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// interface za vse objekte, ki imajo progress bar
public interface IHasProgress {

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChange;
    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }
}
