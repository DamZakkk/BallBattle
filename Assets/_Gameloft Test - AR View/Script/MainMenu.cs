using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject declinedNotif;
    bool isRequestingPermission;

    public void StartButton()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            SceneManager.LoadScene("AR Scene");
        }
        else
        {
            isRequestingPermission = true;
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isRequestingPermission)
        {
            isRequestingPermission = false;
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                SceneManager.LoadScene("AR Scene");
            }
            else
            {
                declinedNotif.SetActive(true);
            }
        }
    }

}