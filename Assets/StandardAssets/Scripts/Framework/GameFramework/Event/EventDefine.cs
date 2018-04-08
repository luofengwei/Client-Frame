using UnityEngine;
using System.Collections;

public enum ESysEvent 
{
    TestClientEvent,
    NetDisconnected,

    EnterCreateRole,
    EnterHall,
    EnterRoom,

    LoadingStart,
    LoadingEnd,
    GameStart,//这个枚举Lua中用int值 7 访问了,不能修改
    GameEnd,//这个枚举Lua中用int值 8 访问了,不能修改
    GameOffense,//这个枚举Lua中用int值 9 访问了,不能修改
    JoinPlayer,//这个枚举Lua中用int值 10 访问了,不能修改

    EventMoveTest,      //测试转发 

    ShootOut,
    ShootEffect,
    ShootBadEffect,
    BlockShot,      //盖帽成功特效播放事件
    BeBlocked,      //被盖帽成功事件
    TapBall,  
    BallOut,
    NicePass,
    BackboardSucc,
    HitRim,
    ReTipOff,
    DunkHitRim,
    DunkSucc,
    BlockSucc,//盖帽成功音效播放事件
    StealSucc,
    CreateBallIllusion,
    DestroyBallIllusion,
    Alleyoop,    //空接
    Buzzerbeater,//全场最后时刻压哨进球
    Hurryup,     //全场最后10秒
    Tipout,     //分球
    Check,      //晃到人
    FadewayShoot,    //后仰投篮
    HookShoot,    //勾手投篮
    Intercept,   //自然断
    Violation,   //进攻超时
    Turnover,   //要防守了

    FallDownEffect,     //倒地和扑球
    ThrowBallEffect,    //丢球
    ButtonFeedEffect,//大按键特效
    ButtonMediumEffect,//中按键特效
    ButtonSmallEffect,//小按键特效
    HallPanelBtnEffect,

    //BasketBallShadow,
    //BallEffect,       //球轨迹特效
    //ReboundEffect,    //抢篮板特效
    //BallSpark,        //球闪光特效
    DisConnectTag,    //断线特效    
   
    ReboundSuccessC, //中锋抢到篮板的特效

    //11.24添加特效   起跳、落地、手接球、相撞抱球、上篮起跳、灌篮新增
    Toground_smoke,
    Catchball,
    Tohit,
    Layup,
    Dunk_Success_1,   

    CDTime,         //CD倒计时声音
    CDOver,         //CD结束的声音       
    //BeginTrail,
    //EndTrail,

    BeginRotaion,
    EndRotation,
    RoleMove,
    RoleStop,
    BallPickUp,
    BeginBulletTime,
    EndBulletTime,

    DunkTargetPos,
    LayupPos,
    CameraPlay,
    //HasBallEvent,   

    HandShank,
    HandShankEnd,

    JoystickAxis,   
    //BasketBallTouchGround,
    AnimationLogicEvent,        //动画  
    Addition,//加时赛 

    OFFLine,                    //掉线
    Reconnected,                //进程重连状态
    ServiceClose,               //服务器关闭

    // 相机镜头的一系列触发事件
    CameraFollowInit, // 相机重新回到跟随自己，初始化位置 

    //比赛结束胜利失败动画
    GameEndVictory,
    GameEndFail,

    PlayParticalAnimation,
    PlayParticalLoopAnimation,


    CameraPlayBack,
    GameSwitchState
}


/// <summary>
/// 触发事件定义
/// 因为涉及到美术的编辑，值定好之后不可变更
/// </summary>
public enum ETriggerEvent
{
    TriggerEvtStart = 40000,
    TestPressAlpha7ToTrigger = 40001,   //测试事件，按键7触发
    TestPressAlpha8ToTrigger = 40002,   //测试事件，按键8触发
    //OperMistake = 40004,            //操作失误的时候产生
    //BeginPlay = 40005,              //玩家在待机状态下操作正确时产生
    //ComboLevelChange = 40006,       //玩家的Combo等级改变时
    //GameStart = 40010,              //游戏开始时触发
    //ComboRateChange = 40011,        //Combo数占所有combo数的百分比，每改变0.05触发一次   
    ShowTimeBegin = 40012,            //ShowTime开始
    ShowTimeEnd = 40013,              //ShowTime结束
    ComboEffect = 40014,        //ComboEffect
    VoiceOneTeacherTurn = 40015,    //一个导师转身效果
    VoiceAllTeacherTurn = 40016,       //所有导师转身效果
    EffectGameEnd = 40017,           //唱歌结束   关闭所有角色的特效   

    Behaviour = 40018,              //行为的触发
    BehaviourEffect = 40019,        //由行为代理触发的角色特效
    MicrophoneBegin = 40020,        //打开麦克风
    MicrophoneEnd = 40021,          //关闭麦克风
    //-----------------------------------------------------------
    //根据歌曲编辑的事件触发，在SongArtEditor中编辑触发时间
    SongArtPartStart = 41000,
    ClimaxBegin = 41001,            //高潮开始
    ClimaxEnd = 41002,              //高潮结束

    SongArtPartEnd,
    //-----------------------------------------------------------
}

