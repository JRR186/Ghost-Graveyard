using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject healthText;
    public GameObject enemiesLeftText;


    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle healthToggle;
    [SerializeField] Toggle enemyToggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
        if (!PlayerPrefs.HasKey("healthToggle"))
        {
            PlayerPrefs.SetInt("healthToggle", healthToggle.isOn ? 1 : 0);
            Load();
        }
        else
        {
            Load();
       }
        if (!PlayerPrefs.HasKey("enemyToggle"))
        {
            PlayerPrefs.SetInt("enemyToggle", enemyToggle.isOn ? 1 : 0);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void DisplayHealth()
    {
        healthText.SetActive(true);
    }

    public void DisplayEnemy()
    {
        enemiesLeftText.SetActive(true);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        healthToggle.isOn = PlayerPrefs.GetInt("healthToggle", 0) == 1;
        enemyToggle.isOn = PlayerPrefs.GetInt("enemyToggle", 0) == 1;

    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
        PlayerPrefs.SetInt("healthToggle", healthToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("enemyToggle", enemyToggle.isOn ? 1 : 0);

    }
}
