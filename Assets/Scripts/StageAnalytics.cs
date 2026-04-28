using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class StageAnalytics : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainStage 2") return;

        var gm = GameManager.Instance;

        // ✅ Event 1: ผู้เล่นเข้าด่าน 2 ได้สำเร็จ (หลังซื้อกุญแจ)
        var enterEvent = new CustomEvent("stage_entered")
        {
            { "stage_id",     "MainStage 2"                 },
            { "player_level", gm != null ? gm.Level : 0    },
            { "coins",        gm != null ? gm.Coins : 0    },
            { "gems",         gm != null ? gm.Gems  : 0    },
            { "orbs",         gm != null ? gm.Orbs  : 0    }
        };
        AnalyticsService.Instance.RecordEvent(enterEvent);
        Debug.Log("[Analytics] stage_entered → MainStage 2 sent");

        // ✅ Event 2: pass_checkpoint (เดิม)
        var checkpointEvent = new CustomEvent("pass_checkpoint")
        {
            { "checkpoint_id", "MainStage2_cleared"          },
            { "player_level",  gm != null ? gm.Level : 0    },
            { "coins",         gm != null ? gm.Coins : 0    },
            { "gems",          gm != null ? gm.Gems  : 0    },
            { "orbs",          gm != null ? gm.Orbs  : 0    }
        };
        AnalyticsService.Instance.RecordEvent(checkpointEvent);
        Debug.Log("[Analytics] pass_checkpoint → MainStage2_cleared sent");
    }
}