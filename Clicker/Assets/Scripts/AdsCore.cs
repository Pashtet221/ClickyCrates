using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsCore : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool testmod = true;
    private string gameId = "4379855";

    private string video = "Interstitial_Android";
    private string rewardedVideo = "Rewarded_Android";
    // private string banner = "Banner_Android";

    private CoinsManager coinsManager;

    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testmod);

        coinsManager = FindObjectOfType<CoinsManager>();

        //  StartCoroutine(ShowBannerWhenInitialized());
        //  Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    }

    public static void ShowAdsVideo(string placementId)
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementId);
        }
        else
        {
            Debug.Log("Advertisement not ready!");
        }
    }

    //IEnumerator ShowBannerWhenInitialized()
    //{
    //    while (!Advertisement.isInitialized)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //    Advertisement.Banner.Show(banner);
    //}

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == rewardedVideo)
        {

        }
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            Debug.Log("+1");
            coinsManager.AddCoins(new Vector3(0, 1, 0), 35);
        }
        if (showResult == ShowResult.Skipped)
        {
            Debug.Log("-1");
        }
        if (showResult == ShowResult.Failed)
        {
            Debug.Log("Не сегодня");
        }
    }
}
