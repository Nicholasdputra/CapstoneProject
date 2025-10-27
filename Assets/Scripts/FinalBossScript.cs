using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalBossScript : ClickableEntity, IClickable
{
    private Canvas objectsCanvas;
    public string miniBossName;
    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private TextMeshProUGUI miniBossNameText;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private GameObject timerBarObject;
    [SerializeField] private Image timerBar;

    public int timer;
    public int timeLimit;
    public Coroutine timerCoroutine;

    public int soulEssenceGain;
    public int dreamEssenceGain;
    public int humanSoulGain;
        
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        
    }

    public override void OnClick()
    {
        
    }

    public void OnHover()
    {
        
    }

    public void OnUnhover()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public override void HandleDestroy()
    {
        throw new System.NotImplementedException();
    }
}
