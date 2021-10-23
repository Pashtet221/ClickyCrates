using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionsBetweenScenes : MonoBehaviour
{
    public GameObject shop;

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void CloseShop()
    {
        shop.SetActive(false);
    }

    public void OpenShop()
    {
        shop.SetActive(true);
    }
}
