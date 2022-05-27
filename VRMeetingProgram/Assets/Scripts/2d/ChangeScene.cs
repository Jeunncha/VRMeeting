using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public string playfabId;
    
    //public GameObject MemberPanel;
    
    #region �� ��ȯ
    public void LoadNextScene(string scene)
    {

        // �񵿱������� Scene�� �ҷ����� ���� Coroutine�� ����Ѵ�.
        StartCoroutine(LoadMyAsyncScene(scene));
    }

    IEnumerator LoadMyAsyncScene(string scene)
    {
        // AsyncOperation�� ���� Scene Load ������ �� �� �ִ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Scene�� �ҷ����� ���� �Ϸ�Ǹ�, AsyncOperation�� isDone ���°� �ȴ�.
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    #endregion

    #region ��ư Ŭ��
    public void MemberClick()
    {
        //MemberPanel.SetActive(true);
        LoadNextScene("MainScene"); //�̰��ϸ� ���������� ��� �÷����տ��� �޾ƿ��Ե�
    }

    public void ChatClick()
    {
        LoadNextScene("ChatScene");
        
    }

    public void SettingClick()
    {
        LoadNextScene("CustomizeScene");       
    }
    #endregion

    #region ����
    /*public void SaveStr(string val)
    {
        PlayerPrefs.SetString("userId", val);
        
        //PlayerPrefs.SetString
        SetData(val);       
    }*/

    /*public void SaveInt(int val)
    {
        PlayerPrefs.SetInt("userChar", val);
    }*/

    /*
    public string Load(string key)
    {
        string myID = PlayerPrefs.GetString(key);
        return myID;
    }
    #endregion

    public int SetData(string myID)
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };

        PlayFabClientAPI.GetUserData(request, 
            (result) => {
            userName = result.Data["name"].Value;              
            userRole = result.Data["role"].Value;
            userTeam = result.Data["team"].Value;
            },
            (error) => print("������ �ҷ����� ����")
        );
       
        
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) =>
            {
                foreach (var eachStat in result.Statistics)
                {
                    switch (eachStat.StatisticName)
                    {
                        case "customize": customize = eachStat.Value; print(customize); break;

                    }
                }
            },
            (error) => { print("�� �ҷ����� ����"); }
         );

        return 0;
       
    }
    */

    /*public int getCustomize()
    {
        return customize;
    }
    public string getName()
    {
        return userName;
    }
    public string getRole()
    {
        return userRole;
    }
    public string getTeam()
    {
        return userTeam;
    }
    */

    /*
   public void GetStat()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (result) => Callback(),
            (error) => { print("�� �ҷ����� ����"); }
         );       
    }
    void Callback()
    {

    }
    */
    #endregion
}
