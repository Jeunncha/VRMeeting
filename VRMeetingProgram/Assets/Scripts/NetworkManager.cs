using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    //ChatManager chatManager;
    //public GameObject CM;
    public ChatManager CM;

    [Header("SideBar")]
    public GameObject SideBar;
    public InputField NickNameInput;

    [Header("SideBar2")]
    public GameObject SideBar2;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    //public Button PreviousBtn;
    //public Button NextBtn;

    [Header("Chat")]
    public GameObject Chat;
    public Text ListText;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;
    //public Text NickNameText;
    //public InputField ChatInput;


    List<RoomInfo> myList = new List<RoomInfo>();
    //int currentPage = 1, maxPage, multiple;

    #region ��������
    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        //NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        //NickNameText.color = PV.IsMine ? Color.green : Color.red;
        //CM = GameObject.Find("ChatManager");
    }

    void Update()
    {
        //Debug.Log(PV.IsMine);
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = PhotonNetwork.CountOfPlayers + "����";

        if(Chat.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Return)){
                NMSend();
            }
        }

    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(); //connect�� �ݹ�

    public override void OnJoinedLobby()
    {
        SideBar.SetActive(false);
        SideBar2.SetActive(true);
        Chat.SetActive(false);
        
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "�� ȯ���մϴ�";
        myList.Clear();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();      
    }

    public override void OnDisconnected(DisconnectCause cause) //disconnect�ݹ�
    {
        SideBar2.SetActive(false);
        Chat.SetActive(false);
        SideBar.SetActive(true);
    }
    #endregion

    #region �渮��Ʈ ����
    // ����ư -2 , ����ư -1 , �� ����
    public void MyListClick(int num)
    {
        //if (num == -2) --currentPage;
        //else if (num == -1) ++currentPage;
        //else
        PhotonNetwork.JoinRoom(myList[num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // �ִ�������
        //maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ������ư
        //PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        //NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        //multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (i < myList.Count) ? myList[i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (i < myList.Count) ? myList[i].PlayerCount + "/" + myList[i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        //Debug.Log(roomCount);
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region ��
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 3 });  //���������� ��������� onJoinedRoom����
        RoomInput.text = "";
        //RoomRenewal();
    }

    //public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        Chat.SetActive(true);
        SideBar2.SetActive(true);
        RoomRenewal();
        ChatInput.text = "";
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
        //MyListRenewal();
        //SideBar2.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    //public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    //player�� �濡 ���� �� ȣ��
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal(); //����� ���Դ� ������ �� �� �� ����
        InformRPC(newPlayer.NickName + "���� �����ϼ̽��ϴ�");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        InformRPC(otherPlayer.NickName + "���� �����ϼ̽��ϴ�");
    }

    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / ���� �� : " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + PhotonNetwork.CurrentRoom.MaxPlayers + "�ִ�";
    }
    #endregion


#region ä��
    public void NMSend()
    {
        
        //Debug.Log(ChatInput.text);
        //CM = GameObject.Find("ChatManager");
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if ((PhotonNetwork.PlayerList[i].NickName).Equals(PhotonNetwork.LocalPlayer.NickName))
            {
                //Debug.Log("??");
                //CM.GetComponent<ChatManager>().Send(ChatInput.text);
                CM.Send(ChatInput.text);
            }           
        }
        PV.RPC("ChatRPC", RpcTarget.Others, PhotonNetwork.NickName + " : " + ChatInput.text);
        //PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);

        //ChatRPC(ChatInput.text);
        ChatInput.text = "";
    }

    //public void 

    void InformRPC(string msg)
    {

        //CM.GetComponent<ChatManager>().Inform(msg);
        CM.Inform(msg);
    }

    [PunRPC]
    void ChatRPC(string msg)
    {
        //CM.GetComponent<ChatManager>().ChatTo(msg);
        CM.ChatTo(msg);
    }

/* 
    public void Send()
    {
        if (PV.IsMine)
        {
            PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        }
        *//*else
        {
            PV.RPC("ChatOtherRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
            //Debug.Log(PV.Owner.NickName);
        }
       
        ChatInput.text = "";
    }
    */


    /*[PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        string[] msgSp = msg.Split(':');
        ChatManager.GetComponent<ChatManager>().Chat(true, msgSp[1], msgSp[0], null);

        //Debug.Log(msgSp[0] + "??" + msgSp[1]);
        //string name = PhotonNetwork.NickName;
        //Debug.Log(name.Equals(msgSp[0]));
        //Debug.Log(msgSp[0].GetType().Name+ msgSp[0]);
        //Debug.Log(PhotonNetwork.NickName.GetType().Name+ PhotonNetwork.NickName);


        *//*        if (PV.IsMine)
                {
                    Debug.Log(msgSp[0] + "??" + msgSp[1]);
                }
                else
                {
                    Debug.Log(msgSp[0] + "!!" + msgSp[1]);
                }*//*

        //bool who = PV.IsMine ? true : false;
        //string name = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        //Debug.Log(PV.IsMine + name);
        //ChatManager.GetComponent<ChatManager>().Chat(who, msg, name, null);

        //chatManager.Chat(who, msg, name, null);

        //
        *//*
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }

    }*/

    
   /* [PunRPC]
    void ChatOtherRPC(string msg)
    {
        string[] msgSp = msg.Split(':');
        Debug.Log(msgSp[0] + "!!" + msgSp[1]);
    }*/
        #endregion



   }
