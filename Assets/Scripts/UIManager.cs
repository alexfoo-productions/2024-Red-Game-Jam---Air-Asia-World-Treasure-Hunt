using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] AudioSource AudioPlayer;

    [Header("UI GameObjects")]
    [SerializeField] GameObject MainUIScreen;
    [SerializeField] GameObject SplashScreen;
    [SerializeField] GameObject HelpScreen;
    [SerializeField] GameObject HighScoreScreen;
    [SerializeField] GameObject DailyRewardsScreen;
    [SerializeField] GameObject CardManager;

    [Header("Managers")]
    ObjectPreviewerControls objControl;

    [Header("Audio")]
    [SerializeField] AudioClip sfxFly;
    [SerializeField] AudioClip sfxButton;
    public void playFlySFX()
    {
        AudioPlayer.PlayOneShot(sfxFly);
    }

    public void playButton()
    {
        AudioPlayer.PlayOneShot(sfxButton);
    }

    public void TakeOff()
    {
        SplashScreen.SetActive(false);

        MainUIScreen.SetActive(true);
        HelpScreen.SetActive(true );
        CardManager.SetActive(true );
    }

    public void TopTen(bool _ShowHide)
    {
        HighScoreScreen.SetActive(_ShowHide);
        playButton();
    }

    public void Daily(bool _ShowHide)
    {
        DailyRewardsScreen.SetActive(_ShowHide);
        playButton();
    }

    public void Info(bool _ShowHide)
    {
        HelpScreen.SetActive(_ShowHide);
        playButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
