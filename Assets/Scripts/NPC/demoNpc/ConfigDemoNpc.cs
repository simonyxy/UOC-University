
using System.Diagnostics;
using System.Collections.Generic;

public class ConfigDemoNpc : NpcconfigBase
{   
    public override void Init(){
        
        addDiagByTypeAndFriend(playerTiggerType.playerUp , 10 , playerUp10);
    }

    private static ConfigDemoNpc  _instance;
    public static ConfigDemoNpc Instance{
        get{
            if(_instance == null)
            {
                _instance = new ConfigDemoNpc();
                return _instance;
            }
            else{
                return _instance;
            }
        }
    }

    public List<string> playerUp10  = new List<string>()
    {
        "我他妈认识你吗？\n跟我打招呼？",
        "傻逼把这\n个人",
        "谁啊这？",
        "跟你很熟吗傻逼",
        "滚蛋行不行",
        "这个人好像叫\n杨惠敏，沙雕",
        "干嘛呢？",
        "?????",
    };
    
    public List<string> playerUp20  = new List<string>();
    public List<string> playerUp30  = new List<string>();
    public List<string> playerUp40  = new List<string>();
    public List<string> playerUp50  = new List<string>();
    public List<string> playerUp60  = new List<string>();
    public List<string> playerUp70  = new List<string>();
    public List<string> playerUp80  = new List<string>();
    public List<string> playerUp90  = new List<string>();
    public List<string> playerUp100 = new List<string>();
    
    
    public List<string> playerDown10  = new List<string>()
    {
        "玩家选中下边，当前友好值不到10，随机数0",
        "玩家选中下边，当前友好值不到10，随机数1",
        "玩家选中下边，当前友好值不到10，随机数2",
        "玩家选中下边，当前友好值不到10，随机数3",
        "玩家选中下边，当前友好值不到10，随机数4",
        "玩家选中下边，当前友好值不到10，随机数5",
        "玩家选中下边，当前友好值不到10，随机数6",
        "玩家选中下边，当前友好值不到10，随机数7",
    };
    
    public List<string> playerDown20  = new List<string>();
    public List<string> playerDown30  = new List<string>();
    public List<string> playerDown40  = new List<string>();
    public List<string> playerDown50  = new List<string>();
    public List<string> playerDown60  = new List<string>();
    public List<string> playerDown70  = new List<string>();
    public List<string> playerDown80  = new List<string>();
    public List<string> playerDown90  = new List<string>();
    public List<string> playerDown100 = new List<string>();

    
    public List<string> playerRight10  = new List<string>()
    {
        "玩家选中右边，当前友好值不到10，随机数0",
        "玩家选中右边，当前友好值不到10，随机数1",
        "玩家选中右边，当前友好值不到10，随机数2",
        "玩家选中右边，当前友好值不到10，随机数3",
        "玩家选中右边，当前友好值不到10，随机数4",
        "玩家选中右边，当前友好值不到10，随机数5",
        "玩家选中右边，当前友好值不到10，随机数6",
    };
    public List<string> playerRight20  = new List<string>()
    {
        
        "玩家选中右边，当前友好值不到20，随机数0",
        "玩家选中右边，当前友好值不到20，随机数1",
        "玩家选中右边，当前友好值不到20，随机数2",
        "玩家选中右边，当前友好值不到20，随机数3",
        "玩家选中右边，当前友好值不到20，随机数4",
        "玩家选中右边，当前友好值不到20，随机数5",
        "玩家选中右边，当前友好值不到20，随机数6",
    };
    public List<string> playerRight30  = new List<string>();
    public List<string> playerRight40  = new List<string>();
    public List<string> playerRight50  = new List<string>();
    public List<string> playerRight60  = new List<string>();
    public List<string> playerRight70  = new List<string>();
    public List<string> playerRight80  = new List<string>();
    public List<string> playerRight90  = new List<string>();
    public List<string> playerRight100 = new List<string>();

    
    public List<string> playerLeft10  = new List<string>()
    {
        "玩家选中左边，当前友好值不到20，随机数0",
        "玩家选中左边，当前友好值不到20，随机数1",
        "玩家选中左边，当前友好值不到20，随机数2",
        "玩家选中左边，当前友好值不到20，随机数3",
        "玩家选中左边，当前友好值不到20，随机数4",
        "玩家选中左边，当前友好值不到20，随机数5",
        "玩家选中左边，当前友好值不到20，随机数6",
    };
    public List<string> playerLeft20  = new List<string>();
    public List<string> playerLeft30  = new List<string>();
    public List<string> playerLeft40  = new List<string>();
    public List<string> playerLeft50  = new List<string>();
    public List<string> playerLeft60  = new List<string>();
    public List<string> playerLeft70  = new List<string>();
    public List<string> playerLeft80  = new List<string>();
    public List<string> playerLeft90  = new List<string>();
    public List<string> playerLeft100 = new List<string>();


    
}

