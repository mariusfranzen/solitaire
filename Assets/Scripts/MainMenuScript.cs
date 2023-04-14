using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public float AnimationTime = 1f;
    public AudioMixer AudioMixer;
    public GameObject MainMenu;
    public GameObject SettingsMenu;
    public Slider MusicSlider;
    public Slider EffectsSlider;
    public TMP_Dropdown CardBackDropdown;
    public TMP_Dropdown BackgroundDropdown;

    void Start()
    {
        LoadSettings();
    }

    void LoadSettings()
    {
        SetVolumeEffects(PlayerPrefs.GetFloat("effectsVolume"));
        SetVolumeMusic(PlayerPrefs.GetFloat("musicVolume"));
        SetCardBack(PlayerPrefs.GetInt("cardBack"));
        SetBackground(PlayerPrefs.GetInt("background"));

        EffectsSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("effectsVolume"));
        MusicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musicVolume"));
        CardBackDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("cardBack"));
        BackgroundDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("background"));
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetVolumeMusic(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
        AudioMixer.SetFloat("MusicVolume", Mathf.Log(volume) * 20);
    }

    public void SetVolumeEffects(float volume)
    {
        PlayerPrefs.SetFloat("effectsVolume", volume);
        AudioMixer.SetFloat("EffectsVolume", Mathf.Log(volume) * 20);
    }

    public void SetCardBack(int option)
    {
        PlayerPrefs.SetInt("cardBack", option);
    }

    public void SetBackground(int option)
    {
        PlayerPrefs.SetInt("background", option);
    }

    public void GoToSettings()
    {
        StartCoroutine(SlideMainMenu(-2000));
    }

    public void GoToMainMenu()
    {
        PlayerPrefs.Save();
        StartCoroutine(SlideMainMenu(2000));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator SlideMainMenu(int distance)
    {
        Vector3 mainStartingPos = MainMenu.transform.position;
        Vector3 mainFinalPos = MainMenu.transform.position + (MainMenu.transform.right * distance);

        Vector3 settingsStartingPos = SettingsMenu.transform.position;
        Vector3 settingsFinalPos = SettingsMenu.transform.position + (SettingsMenu.transform.right * distance);

        float elapsedTime = 0;

        while (elapsedTime < AnimationTime)
        {
            MainMenu.transform.position = Vector3.Lerp(mainStartingPos, mainFinalPos, (elapsedTime / AnimationTime));
            SettingsMenu.transform.position = Vector3.Lerp(settingsStartingPos, settingsFinalPos, (elapsedTime / AnimationTime));
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
    }
}
