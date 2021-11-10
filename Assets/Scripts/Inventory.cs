using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject rewardPrefab;
    public Transform listParent;
    public AppController appController;
    private List<RewardData> rewards = new List<RewardData>();

    // Start is called before the first frame update
    public void FillRewards()
    {
        rewards.Clear();
        string db = PlayerPrefs.GetString("DB");
        if (db.Length < 2) return;
        string[] parts = db.Split('-'); //R1+Q1
        print("Rewards Found: " + parts.Length);
        for (int i = 0; i < parts.Length; i++)
        {
            string[] subParts = parts[i].Split('+');
            print("Name: " + subParts[0] + "Amount: " + subParts[1]);
            rewards.Add(new RewardData(subParts[0], subParts[1] == "" ? 0 : int.Parse(subParts[1])));
        }
    }

    public string GetReward()
    {
        FillRewards();
        int totalRew = 0;
        for (int i = 0; i < rewards.Count; i++)
        {
            totalRew += rewards[i].amount;
        }

        if (totalRew == 0) return "-1";

        string[] possibleRewards = new string[totalRew];
        int rewIndex = 0;
        for (int i = 0; i < rewards.Count; i++)
        {
            for (int j = 0; j < rewards[i].amount; j++)
            {
                possibleRewards[rewIndex] = rewards[i].title;
                rewIndex += 1;
            }
        }

        string finalReward = possibleRewards[Random.Range(0, totalRew)];

        for (int i = 0; i < rewards.Count; i++)
        {
            if (finalReward == rewards[i].title)
            {
                rewards[i].amount -= 1;
            }
        }


        string newDB = "";
        for (int i = 0; i < rewards.Count; i++)
        { 
            newDB = newDB + rewards[i].title + "+" + rewards[i].amount;
            if (i < rewards.Count - 1)
            {
                newDB = newDB + "-";
            }
        }
        print("New DB " + newDB);
        PlayerPrefs.SetString("DB", newDB);


        return finalReward;
    }

    public void InstantiateRewards()
    {
        
        for (int i = 0; i < rewards.Count; i++)
        {
            GameObject newReward = GameObject.Instantiate(rewardPrefab, listParent);
            newReward.GetComponent<Reward>().FillInfo(rewards[i], this);
            //newReward.GetComponent<Reward>()/
        }
    }

    public void SaveChanges()
    {
        string newDB = "";
        for (int i = 0; i < listParent.childCount; i++)
        {
            Reward reward = listParent.GetChild(i).GetComponent<Reward>();
            newDB = newDB + reward.title.text + "+" + reward.amount.text;
            if (i < listParent.childCount - 1)
            {
                newDB = newDB + "-";
            }
        }
        print("New DB " + newDB);
        PlayerPrefs.SetString("DB", newDB);
    }

    public void AddItem()
    {
        GameObject newReward = GameObject.Instantiate(rewardPrefab, listParent);
        Reward reward = newReward.GetComponent<Reward>();
        reward.title.text = "Nombre del premio";
        reward.amount.text = "0";
    }

    public void RemoveItem()
    {
        SaveChanges();
    }

    public void BackToMainScreen()
    { 
        SaveChanges();
        listParent.DetachChildren();
        StartCoroutine(appController.CloseInventoryWorker());
    }

    public string PullReward()
    {
        return "reward";
    }
    
}