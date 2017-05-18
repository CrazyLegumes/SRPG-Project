using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputEvents<T> :EventArgs {

    public T info;
    public InputEvents()
    {
        info = default(T);
    }

    public InputEvents(T info)
    {
        this.info = info;
    }
}
