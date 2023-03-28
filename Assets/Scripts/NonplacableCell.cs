using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NonplacableCell : Cell
{

    public override void unlock(Vector4 entryPoint)
    {
        spriteRend.sprite = unlockedSprite;
        state = CellState.used;
    }

    public override bool use(CardController card, Vector4 entryPoints, Sprite sprite)
    {
        //audioSource.clip = errorSounds[Random.Range(0, errorSounds.Length - 1)];
        //audioSource.Play();
        return false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
