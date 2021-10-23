using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;
    //References
    [Header ("UI references")]
	[SerializeField] TMP_Text[] coinUIText;

    [SerializeField] GameObject animatedCoinPrefab;
	[SerializeField] Transform target;

	[Space]
	[Header ("Available coins : (coins to pool)")]
	[SerializeField] public int maxCoins;
	Queue<GameObject> coinsQueue = new Queue<GameObject> ();


	[Space]
	[Header ("Animation settings")]
	[SerializeField] [Range (0.5f, 3f)] float minAnimDuration;
	[SerializeField] [Range (0.9f, 6f)] float maxAnimDuration;

	[SerializeField] Ease easeType;
	[SerializeField] float spread;

	Vector3 targetPosition;


    //private int _c = 0;

    //public int Coins
    //{
    //    get { return _c; }
    //    set
    //    {
    //        _c = value;

    //        for (int i = 0; i < coinUIText.Length; i++)
    //        {
    //            coinUIText[i].text = Coins.ToString();
    //        }
    //    }

    //}


    void Awake ()
	{
        targetPosition = target.position;

		PrepareCoins ();

            instance = this;
      
        if (PlayerPrefs.HasKey("SavingCoins"))
        {
          //  Coins = PlayerPrefs.GetInt("SavingCoins");
        }
    }


    void Start()
    {
        UpdateAllCoinsUIText();
    }

    void Update()
    {
        UpdateAllCoinsUIText();
    }

    void PrepareCoins ()
	{
		GameObject coin;
		for (int i = 0; i < maxCoins; i++) {
			coin = Instantiate (animatedCoinPrefab);
			coin.transform.parent = transform;
			coin.SetActive (false);
			coinsQueue.Enqueue (coin);
		}
	}

	public void Animate (Vector3 collectedCoinPosition, int amount)
	{
		for (int i = 0; i < amount; i++) {
			//check if there's coins in the pool
			if (coinsQueue.Count > 0) {
				//extract a coin from the pool
				GameObject coin = coinsQueue.Dequeue ();
				coin.SetActive (true);

				//move coin to the collected coin pos
				coin.transform.position = collectedCoinPosition + new Vector3 (Random.Range (-spread, spread), 0f, 0f);

				//animate coin to target position
				float duration = Random.Range (minAnimDuration, maxAnimDuration);
				coin.transform.DOMove (targetPosition, duration)
				.SetEase (easeType)
				.OnComplete (() => {
					//executes whenever coin reach target position
					coin.SetActive (false);
					coinsQueue.Enqueue (coin);

				//	Coins++;
				});
			}
		}
	}

	public void AddCoins (Vector3 collectedCoinPosition, int amount)
	{
		Animate (collectedCoinPosition, amount);
        GameDataManager.AddCoins(amount);
        instance.SaveCoins();
        SaveCoins();
    }

    public void SaveCoins()
    {
      //  PlayerPrefs.SetInt("SavingCoins", Coins);
    }

    //public void UseCoins(int amount)
    //{
    //    Coins -= amount;
    //}

    //public bool HasEnoughCoins(int amount)
    //{
    //    return (Coins >= amount);
    //}

    public void UpdateAllCoinsUIText()
    {
        for (int i = 0; i < coinUIText.Length; i++)
        {
            SetCoinsText(coinUIText[i], GameDataManager.GetCoins());
        }
    }

    void SetCoinsText(TMP_Text textMesh, int value)
    {
        textMesh.text = value.ToString();
    }

    int GetFirstDigitFromNumber(int num)
    {
        return int.Parse(num.ToString()[0].ToString());
    }
}
