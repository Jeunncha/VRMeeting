using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    public InputField EmailInput, PasswordInput, UsernameInput;

    public InputField EInput, PWInput, NInput, IdInput, RoleInput;
    public string myID;
    public GameObject AccountPanel;
    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email = EmailInput.text, Password = PasswordInput.text };       
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);     
    }


    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email = EInput.text, Password = PWInput.text, Username = IdInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
       
    }

    void OnLoginSuccess(LoginResult result) {
        print("�α��� ����");
        myID = result.PlayFabId;
    }


    void OnLoginFailure(PlayFabError error) => print("�α��� ����");

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        print("ȸ������ ����");
        SetData(NInput.text, IdInput.text, RoleInput.text);
        AccountPanel.SetActive(false);
    }

    void OnRegisterFailure(PlayFabError error) => print("ȸ������ ����");

    public void SetData(string name, string id, string role)
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "name", name }, { "id", id }, { "role", role } } };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("������ ���� ����"), (error) => print("������ ���� ����"));
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = myID };
        PlayFabClientAPI.GetUserData(request, (result) => {
            foreach (var eachData in result.Data) //LogText.text += eachData.Key + " : " + eachData.Value.Value + "\n"; 
                print(eachData.Key + ":" + eachData.Value.Value);
        }, 
            (error) => print("������ �ҷ����� ����")
            );
    }

}
