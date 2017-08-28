using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MyUNetManager : NetworkManager
{
    public static MyUNetManager Instance;

    public GameObject uiRoot;


    private Button HostButton;
    private Button JoinGameButton;
    private Button DisNetButton;

    private Button OpenMenuButton;

    private InputField inputIp;

    private GameObject UI;


    public void Awake()
    {
        Instance = this;
    }

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
        uiRoot = Instantiate(Resources.Load<GameObject>("Init/UIRoot"));
        Init();

        DontDestroyOnLoad(uiRoot);
	}
	
    void Init()
    {
        UI = GameObject.Find("GameMenu");
        HostButton = GameObject.Find("CreatButton").GetComponent<Button>();
        JoinGameButton = GameObject.Find("JoinButton").GetComponent<Button>();
        //DisNetButton = GameObject.Find("Btn_DisNet").GetComponent<Button>();

        //OpenMenuButton = GameObject.Find("MenuButton").GetComponent<Button>();

        inputIp = GameObject.Find("InputIP").GetComponent<InputField>();
        inputIp.text = Network.player.ipAddress;

        HostButton.onClick.AddListener(CreatRoom);
        JoinGameButton.onClick.AddListener(JoinGame);
        //DisNetButton.onClick.AddListener(ExitGame);
        //OpenMenuButton.onClick.AddListener(SetUIActive);
    }


    private void CreatRoom()
    {
        //SetPort();
        StartHost();
        UI.SetActive(false);
    }

    private void JoinGame()
    {
        SetIp();
        SetPort();
        StartClient();
        UI.SetActive(false);

    }

    private void ExitGame()
    {
        NetworkManager.singleton.StopHost();
        NetworkManager.singleton.OnStopClient();
        //BattleManager.Instance.ExitBattle();

    }

    private void SetUIActive()
    {
        UI.SetActive(!UI.activeInHierarchy);
    }
    private void SetIp()
    {
        networkAddress = inputIp.text;
    }

    private void SetPort()
    {
        networkPort = 7777;
    }

    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
    }
    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage info = new NetworkMessage();
        info.chosenClass = conn.connectionId;
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(conn, 0, info);
    }

    //public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    //{
    //    NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
    //    int selectedClass = message.chosenClass;
    //    NetworkServer.AddPlayerForConnection(conn, CreatPlayer(selectedClass), playerControllerId);
    //}


    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        Debug.Log(conn.connectionId);
        //BattleManager.Instance.Init();
    }


    //public GameObject CreatPlayer(int rSelectedClass)
    //{
    //    string modelName = "";

    //    GameObject spawnPos = null;
    //    spawnPos = GameObject.Find("pos_A");

    //    switch (rSelectedClass)
    //    {
    //        case 0:
    //            modelName = "Player";
    //            //  spawnPos = GameObject.Find("pos_A");
    //            break;
    //        case 1:
    //            modelName = "Player";
    //            //  spawnPos = GameObject.Find("pos_B");
    //            break;
    //    }

    //    GameObject obj = DisplayObjectManager.Instance.CreatRole(modelName);

    //    if (spawnPos != null)
    //    {
    //        obj.transform.position = spawnPos.transform.position + rSelectedClass * new Vector3(5, 0, 0);
    //        obj.transform.rotation = spawnPos.transform.rotation;
    //    }

    //    return obj;
    //}

}
