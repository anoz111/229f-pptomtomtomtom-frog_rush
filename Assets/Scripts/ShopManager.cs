using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class ShopManager : MonoBehaviour
{
    [Header("Currency UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI gemText;
    [SerializeField] private TextMeshProUGUI orbText;

    [Header("Player Level UI")]
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Key / Unlock (ใช้ COIN)")]
    [SerializeField] private int keyPrice = 10;
    [SerializeField] private int requiredLevel = 5;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private string nextStageSceneName = "MainStage 2";

    [Header("Upgrade Prices (ใช้ GEM)")]
    [SerializeField] private int heartUpgradePrice = 5;
    [SerializeField] private int speedUpgradePrice = 5;
    [SerializeField] private int jumpUpgradePrice = 5;

    [Header("Upgrade Amount")]
    [SerializeField] private int heartQuotaAmount = 1;
    [SerializeField] private float speedBonusAmount = 1f;
    [SerializeField] private float jumpBonusAmount = 2f;

    [Header("Orb Exchange")]
    [SerializeField] private int coinsPerOrb = 10;

    [Header("Popup รวม")]
    [SerializeField] private GameObject notEnoughPopup;
    [SerializeField] private TextMeshProUGUI popupMessageText;

    [Header("Popup Messages (แก้ได้ใน Inspector)")]
    [SerializeField] private string msgGMNotFound = "ไม่พบ GameManager\nเช็คว่ามี GameManager ในซีนแรกหรือยัง";
    [SerializeField] private string msgLevelNotEnough = "เลเวลไม่เพียงพอ\nต้องการเลเวลอย่างน้อย {0}";
    [SerializeField] private string msgCoinNotEnough = "เหรียญไม่เพียงพอ\nต้องใช้ {0} Coin";
    [SerializeField] private string msgGemNotEnough = "เพชรไม่เพียงพอ\nต้องใช้ {0} Gem";
    [SerializeField] private string msgOrbNotEnough = "Orb ไม่เพียงพอ";
    [SerializeField] private string msgSpendError = "เกิดข้อผิดพลาดในการหักสกุลเงิน";

    void OnEnable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnCurrencyChanged += RefreshCurrencyUI;
        GameManager.Instance.OnLevelChanged += RefreshLevelUI;
    }

    void OnDisable()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnCurrencyChanged -= RefreshCurrencyUI;
        GameManager.Instance.OnLevelChanged -= RefreshLevelUI;
    }

    void Start()
    {
        RefreshCurrencyUI();
        RefreshLevelUI();
        if (notEnoughPopup != null) notEnoughPopup.SetActive(false);
    }

    void RefreshCurrencyUI()
    {
        if (GameManager.Instance == null) return;
        if (coinText) coinText.text = "Coin: " + GameManager.Instance.Coins;
        if (gemText) gemText.text = "Gem: " + GameManager.Instance.Gems;
        if (orbText) orbText.text = "Orb: " + GameManager.Instance.Orbs;
    }

    void RefreshLevelUI()
    {
        if (GameManager.Instance == null || levelText == null) return;
        levelText.text = "Level: " + GameManager.Instance.Level;
    }

    // ✅ Helper ส่ง purchase_item
    void SendPurchaseEvent(string itemId, string currency, int price)
    {
        var gm = GameManager.Instance;
        var customEvent = new CustomEvent("purchase_item")
        {
            { "item_id",       itemId   },
            { "currency_used", currency },
            { "price",         price    },
            { "player_level",  gm != null ? gm.Level : 0 }
        };
        AnalyticsService.Instance.RecordEvent(customEvent);
        Debug.Log($"[Analytics] purchase_item → {itemId} ({price} {currency})");
    }

    public void BuyKey()
    {
        if (GameManager.Instance == null) { ShowPopup(msgGMNotFound); return; }

        int coins = GameManager.Instance.Coins;
        int lvl = GameManager.Instance.Level;

        if (lvl < requiredLevel) { ShowPopup(string.Format(msgLevelNotEnough, requiredLevel)); return; }
        if (coins < keyPrice) { ShowPopup(string.Format(msgCoinNotEnough, keyPrice)); return; }
        if (!GameManager.Instance.SpendCoins(keyPrice)) { ShowPopup(msgSpendError); return; }

        // ✅ Event: ซื้อกุญแจสำเร็จ
        SendPurchaseEvent("key", "coin", keyPrice);

        // ✅ Event: ยืนยันว่าเข้าด่าน 2 สำเร็จ (ส่งตรงนี้ เพราะรู้แน่ว่า unlock จริง)
        var gm = GameManager.Instance;
        var stageUnlockEvent = new CustomEvent("reach_level_2")
    {
        { "player_level", gm.Level                }, // หักแล้ว
        { "gems",         gm.Gems                 },
        { "orbs",         gm.Orbs                 }
    };
        AnalyticsService.Instance.RecordEvent(stageUnlockEvent);
        Debug.Log($"[Analytics] reach_level_2 → {nextStageSceneName}");

        if (lockIcon) lockIcon.SetActive(false);
        GameManager.Instance.UnlockKey();
        GameManager.Instance.ClearRunSnapshot();
        SceneManager.LoadScene(nextStageSceneName);
    }

    public void BuyHeartUpgrade()
    {
        if (!TrySpendGem(heartUpgradePrice)) return;
        GameManager.Instance.AddHeartQuota(heartQuotaAmount);
        SendPurchaseEvent("heart_upgrade", "gem", heartUpgradePrice); // ✅
        RefreshCurrencyUI();
    }

    public void BuySpeedUpgrade()
    {
        if (!TrySpendGem(speedUpgradePrice)) return;
        GameManager.Instance.AddSpeedBonus(speedBonusAmount);
        SendPurchaseEvent("speed_upgrade", "gem", speedUpgradePrice); // ✅
        RefreshCurrencyUI();
    }

    public void BuyJumpUpgrade()
    {
        if (!TrySpendGem(jumpUpgradePrice)) return;
        GameManager.Instance.AddJumpBonus(jumpBonusAmount);
        SendPurchaseEvent("jump_upgrade", "gem", jumpUpgradePrice); // ✅
        RefreshCurrencyUI();
    }

    bool TrySpendGem(int price)
    {
        if (GameManager.Instance == null) { ShowPopup(msgGMNotFound); return false; }
        if (GameManager.Instance.Gems < price) { ShowPopup(string.Format(msgGemNotEnough, price)); return false; }
        if (!GameManager.Instance.SpendGems(price)) { ShowPopup(msgSpendError); return false; }
        return true;
    }

    public void ConvertOrbToGem()
    {
        if (GameManager.Instance == null) { ShowPopup(msgGMNotFound); return; }
        if (!GameManager.Instance.SpendOrbs(1)) { ShowPopup(msgOrbNotEnough); return; }
        GameManager.Instance.AddGems(1);
        SendPurchaseEvent("orb_to_gem", "orb", 1); // ✅
        RefreshCurrencyUI();
    }

    public void ConvertOrbToCoins()
    {
        if (GameManager.Instance == null) { ShowPopup(msgGMNotFound); return; }
        if (!GameManager.Instance.SpendOrbs(1)) { ShowPopup(msgOrbNotEnough); return; }
        GameManager.Instance.AddCoins(coinsPerOrb);
        SendPurchaseEvent("orb_to_coins", "orb", 1); // ✅
        RefreshCurrencyUI();
    }

    void ShowPopup(string message)
    {
        if (popupMessageText) popupMessageText.text = message;
        if (notEnoughPopup) notEnoughPopup.SetActive(true);
    }

    public void ClosePopup() => notEnoughPopup?.SetActive(false);
    public void BackToStage() => SceneManager.LoadScene("MainStage");
}