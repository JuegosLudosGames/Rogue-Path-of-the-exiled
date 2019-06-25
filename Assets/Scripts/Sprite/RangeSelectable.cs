using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeSelectable : MonoBehaviour {

    public GameObject TextCount;
    public GameObject activePanel;
    public RangeSelectable[] selectors;

    //ammo type
    public Transform projectile;

    //ammo count
    public int startingCount = 0;
    public int Count = 0;
    public int MaxCount = 0;

    public float powerMultiplier = 5f;

    private bool selected;

    public bool isSelected {
        get
        {
            return selected;
        }

        set
        {
            activePanel.gameObject.SetActive(value);
            selected = value;
        }
    }

    private void Start()
    {
        addAmmo(startingCount);
    }

    public bool canConsume(int amount) {
        return (amount <= Count);
    }

    public void consume(int amount) {
        Count -= amount;
        if (Count < 0)
            Count = 0;
        updateCheck();
    }

    public void addAmmo(int amount) {
        Count += amount;
        if (Count > MaxCount)
            Count = MaxCount;
        updateCheck();
    }

    public bool isFull() {
        return (Count >= MaxCount);
    }

    void updateCheck() {
        TextCount.GetComponent<Text>().text = Count.ToString();
    }
         
}
