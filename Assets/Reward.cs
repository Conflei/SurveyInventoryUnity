using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public InputField title;
    public InputField amount;

    private Inventory parentScript;
    

    public void FillInfo(RewardData data, Inventory parentScript)
    {
        this.title.text = data.title;
        this.amount.text = data.amount.ToString();
        this.parentScript = parentScript;
    }

    public void FillInfo(string title, string amount, Inventory parentScript)
    {
        this.title.text = title;
        this.amount.text = amount;
        this.parentScript = parentScript;
    }

    public void Delete()
    {
        Destroy(this.gameObject);
        this.transform.parent = null;
    }
}

public class RewardData
{

    public RewardData(string title, int amount)
    {
        this.title = title;
        this.amount = amount;
    }

    public string title;
    public int amount;
}
