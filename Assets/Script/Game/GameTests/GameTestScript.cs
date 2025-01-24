using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

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
        
        var gameManager = gameManagerObj.GetComponent<GameManager>();
        Assert.IsNotNull(gameManager,"GameManager is Null");
        
        //Start 버튼확인
        var startButton = GameObject.Find("Start Button");
        Assert.IsNotNull(startButton,"Start Button is Null");
        
        //Start버튼 클릭
        startButton.GetComponent<Button>().onClick.Invoke();
        
        //게임 제어 관련 버튼 확인
        var leftButton = GameObject.Find("LeftMoveButton");
        Assert.IsNotNull(leftButton,"Left Button is Null");
        var rightButton = GameObject.Find("RightMoveButton");
        Assert.IsNotNull(rightButton,"Right Button is Null");
        
        //가스의 등장위치 파악하기
        Vector3 leftPosition = new Vector3(-1f, 0.2f, -3f);
        Vector3 rightPosition = new Vector3(1f, 0.2f, -3f);
        Vector3 centerPosition = new Vector3(0f, 0.2f, -3f);

        float rayDistance = 10f; 
        Vector3 rayDirection = Vector3.forward;
        
        //반복
        while (gameManager.GameState == GameManager.State.Play)
        {
            RaycastHit hit;
            if (Physics.Raycast(leftPosition, rayDirection, out hit, rayDistance,
                    LayerMask.GetMask("Gas")))
            {
                Debug.Log("Left");
            }
            else if (Physics.Raycast(rightPosition, rayDirection, out hit, rayDistance,
                         LayerMask.GetMask("Gas")))
            {
                Debug.Log("Right");
            }
            else if (Physics.Raycast(centerPosition, rayDirection, out hit, rayDistance,
                         LayerMask.GetMask("Gas")))
            {
                Debug.Log("Center");
            }
            else
            {
                Debug.Log("No Hit");
            }
            
            Debug.DrawRay(leftPosition, rayDirection, Color.red);
            Debug.DrawRay(rightPosition, rayDirection, Color.green);
            Debug.DrawRay(centerPosition, rayDirection, Color.blue);
            
            yield return null;
        }
        
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
