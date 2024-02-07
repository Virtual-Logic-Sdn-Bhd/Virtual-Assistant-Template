using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] characterList;
    private int index;

    private Vector3 cameraPosition;
    private Quaternion cameraRotation;

    private void Start()
    {
        index = PlayerPrefs.GetInt("CharacterSelected");

        // Store the camera's initial position and rotation
        cameraPosition = new Vector3(
            PlayerPrefs.GetFloat("CameraPosX", Camera.main.transform.position.x),
            PlayerPrefs.GetFloat("CameraPosY", Camera.main.transform.position.y),
            PlayerPrefs.GetFloat("CameraPosZ", Camera.main.transform.position.z)
        );

        cameraRotation = Quaternion.Euler(
            PlayerPrefs.GetFloat("CameraRotX", Camera.main.transform.rotation.eulerAngles.x),
            PlayerPrefs.GetFloat("CameraRotY", Camera.main.transform.rotation.eulerAngles.y),
            PlayerPrefs.GetFloat("CameraRotZ", Camera.main.transform.rotation.eulerAngles.z)
        );

        characterList = new GameObject[transform.childCount];

        // Fill the array with our models
        for (int i = 0; i < transform.childCount; i++)
            characterList[i] = transform.GetChild(i).gameObject;

        // We toggle off the renderer
        foreach (GameObject go in characterList)
            go.SetActive(false);

        // We toggle the selected character
        if (characterList[index])
            characterList[index].SetActive(true);

        // Set the camera's position and rotation
        Camera.main.transform.position = cameraPosition;
        Camera.main.transform.rotation = cameraRotation;
    }

    public void ToggleLeft()
    {
        // Toggle off the current model
        characterList[index].SetActive(false);

        index--;

        if (index < 0)
            index = characterList.Length - 1;

        // Toggle on the new model
        characterList[index].SetActive(true);
    }

    public void ToggleRight()
    {
        // Toggle off the current model
        characterList[index].SetActive(false);

        index++;

        if (index == characterList.Length)
            index = 0;

        // Toggle on the new model
        characterList[index].SetActive(true);
    }

    public void SelectButton()
    {
        PlayerPrefs.SetInt("CharacterSelected", index);
        SceneManager.LoadScene("MainScene");
    }

    public void ChangeButton()
    {
        // Save the camera's position and rotation in PlayerPrefs
        PlayerPrefs.SetFloat("CameraPosX", Camera.main.transform.position.x);
        PlayerPrefs.SetFloat("CameraPosY", Camera.main.transform.position.y);
        PlayerPrefs.SetFloat("CameraPosZ", Camera.main.transform.position.z);
        PlayerPrefs.SetFloat("CameraRotX", Camera.main.transform.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("CameraRotY", Camera.main.transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("CameraRotZ", Camera.main.transform.rotation.eulerAngles.z);

        SceneManager.LoadScene("CharacterSelection");
    }

    private void OnApplicationQuit()
    {
        // Clear all PlayerPrefs data when the application quits
        PlayerPrefs.DeleteAll();
    }
}
