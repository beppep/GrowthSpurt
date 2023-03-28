using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.SceneManagement;
using UnityEngine;

public abstract class Cell : MonoBehaviour
{
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public int depth;
    protected Vector2 gridPos;
    protected SpriteRenderer spriteRend;
    protected AudioSource audioSource;
    public AudioClip[] errorSounds;
    public CellState state;

    public enum CellState
    {
        locked,
        available,
        used
    }

    private void OnEnable()
    {
        spriteRend = transform.AddComponent<SpriteRenderer>();
        audioSource = transform.AddComponent<AudioSource>();
        transform.AddComponent<BoxCollider2D>().size = new Vector2(1,1);
        state = CellState.locked;
        spriteRend.sprite = lockedSprite;
    }

    public void setGridPos(Vector2 pos)
    {
        gridPos = pos;
    }

    public void setDepth(int d)
    {
        depth = d;
    }

    protected bool isUsed()
    {
        return state == CellState.used;
    }

    protected bool isAvailable()
    {
        return state == CellState.available;
    }

    public abstract bool use(CardController card, Vector4 entryPoints, Sprite sprite);

    public abstract void unlock(Vector4 entryPoint);


}
