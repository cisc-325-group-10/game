using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // NOTE: must add the scenes to the build settings before this will
        //      work, (file -> build settings -> add open scenes (must have the
        //      scene open)
        string fastMathName = "FastMathMini-Game";
        string colourMemoryName = "ColourMemoryMini-Game";

        StartCoroutine(LoadScene(fastMathName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
