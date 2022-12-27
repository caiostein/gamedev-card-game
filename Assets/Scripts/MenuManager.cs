using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;

    private Enum.ActiveScreen activeScreen;

    public void BackFromTutorial()
    {
        this.gameObject.SetActive(false);

        switch (activeScreen)
        {
            case Enum.ActiveScreen.MENU:
                mainMenu.SetActive(true);
                break;
            case Enum.ActiveScreen.DIALOG:
                playMenu.SetActive(true);
                break;
        }   
    }

    public void OpenTutorialMenu(int activeScreenToSet)
    {
        this.gameObject.SetActive(true);
        activeScreen = (Enum.ActiveScreen)activeScreenToSet;
    }
}
