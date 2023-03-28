using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class RootTimer : MonoBehaviour
{
    private PlacableCell root;
    private int turn;
    private TextMeshPro text;
    public Font font;
    float targetFont = 5;
    

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.AddComponent<TextMeshPro>();
        text.alignment = TextAlignmentOptions.Midline;
        text.fontSize = 7;
        text.fontStyle = FontStyles.Bold;
        text.text = turn.ToString();
        //text.alignment = TextAnchor.MiddleCenter;
        //text.text = turn.ToString();
        //text.font = font;
    }

    // Update is called once per frame
    void Update()
    {
        if (!root)
            return;
        GetComponent<RectTransform>().position = root.transform.position;//FindObjectOfType<Camera>().WorldToScreenPoint(root.transform.position);
        float fontDiff = targetFont - text.fontSize;
        text.fontSize += fontDiff * Time.deltaTime * 5;
    }

    public void initiate(PlacableCell root, int turns, float scaleFactor)
    {   
        turn = turns;
        this.root = root;
    }


    public void tick()
    {
        if (turn == 0)
            return;
        turn--;
        text.text = turn.ToString();
        text.fontSize = 7;
        if (turn == 0)
        {
            root.freeze();
            Destroy(gameObject);
        }
            
    }
}
