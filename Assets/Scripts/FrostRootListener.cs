using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrostRootListener : RootListener
{
    private List<RootTimer> rootTimers = new List<RootTimer>();
    public RootTimer rootTimerPrefab;
    public int turns;
    public Canvas rootTimerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        rootTimerCanvas = Instantiate<Canvas>(rootTimerCanvas);
        rootTimerCanvas.name = "RootTimerCanvas";
        rootTimerCanvas.GetComponent<RectTransform>().position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void customizedTrigger(PlacableCell root)
    {
        //Vector3 rootPos = root.transform.position;
        foreach (RootTimer timer in rootTimers) {
            timer.tick();
        }
        RootTimer rootTimer = Instantiate<RootTimer>(rootTimerPrefab);
        rootTimer.transform.parent = rootTimerCanvas.transform;
        rootTimer.initiate(root, turns, rootTimerCanvas.scaleFactor);
        rootTimers.Add(rootTimer);
        /*GameObject go = new GameObject();
        go.AddComponent<SpriteRenderer>();
        go.transform.parent = root.transform;
        go.transform.localPosition = new Vector3(0, 0, 0);
        */
    }
}
