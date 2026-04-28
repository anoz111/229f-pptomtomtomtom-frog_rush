using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class ShopPortal : MonoBehaviour
{
    [SerializeField] private string shopSceneName = "ShopScene";
    private bool sent = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !sent)
        {
            sent = true;

            var gm = GameManager.Instance;
            var customEvent = new CustomEvent("enter_shop")
            {
                { "coins", gm != null ? gm.Coins : 0 },
                { "gems",  gm != null ? gm.Gems  : 0 },
                { "orbs",  gm != null ? gm.Orbs  : 0 }
            };
            AnalyticsService.Instance.RecordEvent(customEvent);

            SceneManager.LoadScene(shopSceneName);
        }
    }
}