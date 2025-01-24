using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void GameTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        
        SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
        yield return WaitForSceneLoad();
        
        //필수오브젝트확인
        var gameManagerObj = GameObject.Find("GameManager");
        Assert.IsNotNull(gameManagerObj,"GameManager Obj is Null");
        
        yield return null;
    }

    private IEnumerator WaitForSceneLoad()
    {
        while (SceneManager.GetActiveScene().buildIndex > 0)
        {
            yield return null;
        }
    }
}
