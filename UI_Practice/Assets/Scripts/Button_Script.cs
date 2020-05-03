using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Script : MonoBehaviour
{
    public GameObject LoginForm;
    public GameObject MenuButton;
    public GameObject Menu;
    bool menuOpenCheck;
    bool muteCheck;
    int currentRadio;


    [SerializeField] private InputField Name_Input;
    [SerializeField] private InputField Password_Input;

    [SerializeField] private Text CurrentSound;

    // Start is called before the first frame update
    void Start()
    {
        MenuButton.SetActive(false);
        Menu.SetActive(false);
        menuOpenCheck = false;
        muteCheck = true;
        currentRadio = 1;
        CurrentSound.text = "Current Sound : None";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoginCheck()
    {
        if (Name_Input.text == "KimHJ" && Password_Input.text == "rlaguswls")
        {
            Debug.Log("로그인 성공");
            MenuButton.SetActive(true);
            LoginForm.SetActive(false);
        }
        else
        {
            Debug.Log("로그인 실패");
        }
    }

    public void OpenMenu()
    {
        menuOpenCheck = !menuOpenCheck;
        if (menuOpenCheck)
        {
            Menu.SetActive(true);
        }
        else
        {
            Menu.SetActive(false);
        }
    }

    public void MenuSelect_Info()
    {
        Menu.transform.GetChild(0).gameObject.SetActive(true);
        Menu.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void MenuSelect_Sound()
    {
        Menu.transform.GetChild(1).gameObject.SetActive(true);
        Menu.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SelectRadio_1()
    {
        currentRadio = 1;
        if (muteCheck)
            return;

        CurrentSound.text = "Current Sound : Radio_1";
    }

    public void SelectRadio_2()
    {
        currentRadio = 2;
        if (muteCheck)
            return;

        CurrentSound.text = "Current Sound : Radio_2";
    }

    public void SoundMute()
    {
        muteCheck = !muteCheck;

        if(muteCheck)
            CurrentSound.text = "Current Sound : None";
        else
        {
            if(currentRadio == 1)
                CurrentSound.text = "Current Sound : Radio_1";
            if (currentRadio == 2)
                CurrentSound.text = "Current Sound : Radio_2";
        }
    }
}
