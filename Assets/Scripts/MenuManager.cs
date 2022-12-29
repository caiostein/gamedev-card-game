using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    public Image schellImage;

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

    private void Start()
    {
        // Start the coroutine
        StartCoroutine(ChangeSpriteAfterDelay());
    }

    public void OpenTutorialMenu(int activeScreenToSet)
    {
        this.gameObject.SetActive(true);
        activeScreen = (Enum.ActiveScreen)activeScreenToSet;
    }

    IEnumerator ChangeSpriteAfterDelay()
    {
        while (activeScreen.Equals(Enum.ActiveScreen.MENU))
        {
            schellImage.sprite = Resources.Load<Sprite>("schell_estetica");
            yield return new WaitForSeconds(1);
            schellImage.sprite = Resources.Load<Sprite>("schell_narrativa");
            yield return new WaitForSeconds(1);
            schellImage.sprite = Resources.Load<Sprite>("schell_tec");
            yield return new WaitForSeconds(1);
            schellImage.sprite = Resources.Load<Sprite>("schell_mec");
            yield return new WaitForSeconds(1);
        }
    }

}
