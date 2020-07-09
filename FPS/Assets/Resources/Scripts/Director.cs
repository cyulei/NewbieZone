
public class Director : System.Object
{
    private static Director _instance;             //导演类的实例
    public PlayerController CurrentPlayerController { get; set; }    // 控制玩家行为
    public BulletFactory CurrentBulletFactory { get; set; }          // 子弹工厂
    public UserInput CurrentUserInput { get; set; }                  // 用户输入
    public WeaponsManager CurrentWeaponsManager { get; set; }        // 武器管理
    public HealthManagemer CurrentHealthManagemer { get; set; }      // 血量管理
    public AttackExtraEffectTool CurrentAttackExtraEffectTool { get; set; }      // 子弹额外效果
    public SceneController CurrentSceneController { get; set; }      // 场景控制器
    public UIController CurrentUIController { get; set; }            // UI控制器
    public static Director GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Director();
        }
        return _instance;
    }
}
