using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCell : PlacableCell
{
    public CardManager cardManagerPrefab;
    int power;
    List<Vector4> roots;

    // Start is called before the first frame update
    void Start()
    {
        cardManagerPrefab = FindObjectOfType<CardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addRoots(List<Vector4> roots)
    {
        this.roots = roots;
    }

    protected override void handleUse()
    {
        FindObjectOfType<CardManager>().addSpecificCards(roots);
        //cardManagerPrefab.addCards(power);
    }

    public void setPower(int p)
    {
        power = p;
    }
}
