using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardController cardPrefab;
    public AudioSource audioSource;
    public AudioClip[] drawCardSounds;
    public Sprite[] rootSprites;
    public Vector4[] entryPointsList;
    public GameObject slots;
    
    List<CardController> cardList = new List<CardController>();
    int cardsToAdd;
    List<Vector4> cardsToAdd2 = new List<Vector4>();
    float cardTimer;
    float cardDrawTime = 0.03f;
    int initNumOfCards = 10;
    int maximumNumOfCards = 10;
    Dictionary<Vector4, Sprite> rootSpriteDict;


    // Start is called before the first frame update
    void Start()
    {
        slots.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, aspectRatioOffset());
        audioSource = GetComponent<AudioSource>();
        cardDrawTime = 0.03f;
        //createRootSpriteDict();
        //addCards(initNumOfCards);
    }

    // Update is called once per frame
    void Update()
    {
        if(cardsToAdd2.Count > 0)
        {
            if (cardList.Count == maximumNumOfCards)
            {
                cardsToAdd2 = new List<Vector4>();
                return;
            }

            if (cardTimer > cardDrawTime)
            {
                addCard(cardsToAdd2[0]);
                cardsToAdd2.RemoveAt(0);
                cardTimer = 0;
                cardDrawTime = Mathf.Min(0.2f, cardDrawTime + 0.03f);
            }

            cardTimer += Time.deltaTime;
        }
        /*
        if (cardsToAdd > 0)
        {
            if (cardList.Count == maximumNumOfCards)
            {
                cardsToAdd = 0;
                return;
            }
            
            if(cardTimer > cardDrawTime)
            {
                addCard();
                cardsToAdd--;
                cardTimer = 0;
                cardDrawTime = Mathf.Min(0.2f, cardDrawTime + 0.03f);
            }

            cardTimer += Time.deltaTime;
                
        }
        */

    }

    void createRootSpriteDict()
    {
        rootSpriteDict = new Dictionary<Vector4, Sprite>();
        for(int i = 0; i < entryPointsList.Length; i++)
        {
            rootSpriteDict.Add(entryPointsList[i], rootSprites[i]);
        }
    }

    public void init()
    {
        removeAllCards();
        createRootSpriteDict();
        //addCards(initNumOfCards);
        addSpecificCards(FindObjectOfType<Grid>().getStartHand());
    }

    public void addSpecificCards(List<Vector4> vectors)
    {
        foreach(Vector4 v4 in vectors)
        {
            cardsToAdd2.Add(v4);
        }
            
    }

    public void addCards(int numOfCards)
    {
        cardsToAdd = numOfCards;
    }

    public void addCard(Vector4 entryPoints)
    {
        CardController newCard = Instantiate<CardController>(cardPrefab);
        newCard.setEntryPoints(entryPoints);
        newCard.setCardSprite(rootSpriteDict[entryPoints]);
        cardList.Add(newCard);
        newCard.transform.SetParent(transform.parent);
        visualizeCards();
        int i = Random.Range(0, drawCardSounds.Length - 1);
        playSound(drawCardSounds[i]);
    }

    public void addCard()
    {
        CardController newCard = Instantiate<CardController>(cardPrefab);
        cardList.Add(newCard);
        newCard.transform.SetParent(transform.parent);
        visualizeCards();
        int i = Random.Range(0, drawCardSounds.Length - 1);
        playSound(drawCardSounds[i]);
    }

    void visualizeCards()
    {
        for(int i = 0; i < cardList.Count; i++)
        {
            CardController card = (CardController) cardList[i];
            float cutoff = maximumNumOfCards / 2;
            float j = i;
            if (i >= cutoff)
                j -= cutoff;
            float yPlacement = Mathf.FloorToInt(i / cutoff);
            float xPlacement = Mathf.FloorToInt(j+1 - cutoff / 2);

            if(cutoff % 2 == 0)
                xPlacement += 0.5f;
            card.setTargetPosition(new Vector3(-(xPlacement * 125 + xPlacement * 10), -600 + yPlacement * 175 + aspectRatioOffset(), 0));

        }
    }

    float aspectRatioOffset()
    {
        float aspectRatio = 9f * Screen.height / Screen.width + 0.1f;
        //print(aspectRatio);
        if (aspectRatio >= 19.5f)
            return -50;
        if (aspectRatio >= 18.5f)
            return -15;
        if (aspectRatio >= 18)
            return 0; //TTRRUUE
        if (aspectRatio >= 16)
            return 70;
        if (aspectRatio >= 15)
            return 100;
        if (aspectRatio >= 14)
            return 130;
        if (aspectRatio >= 12.5f)
            return 175;
        if (aspectRatio >= 12)
            return 190;
        return 0;
    }

    public void removeCard(CardController card)
    {
        cardList.Remove(card);
        visualizeCards();
    }

    public void playSound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void removeAllCards()
    {
        while(cardList.Count > 0)
        {
            CardController card = cardList[0];
            cardList.Remove(card);
            Destroy(card.gameObject);
        }
        visualizeCards();
    }
}
