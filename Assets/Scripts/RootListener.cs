using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RootListener : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void trigger(PlacableCell root)
    {
        customizedTrigger(root);
    }

    protected abstract void customizedTrigger(PlacableCell root);
}
