using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class GameTestScript
{
    private CarController _carController;
    private GameObject _leftButton;
    private GameObject _rightButton;
    
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
        
        //게임속도 지정
        Time.timeScale = 3f;
        
        //씬로드하기
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
        
        //Start버튼 클릭 게임 실행
        startButton.GetComponent<Button>().onClick.Invoke();
        
        //플레이어 자동차 확인
        _carController = GameObject.Find("Car(Clone)").GetComponent<CarController>();
        Assert.IsNotNull(_carController,"Car is Null");
        
        //게임 제어 관련 버튼 확인
        _leftButton = GameObject.Find("LeftMoveButton");
        Assert.IsNotNull(_leftButton,"Left Button is Null");
        _rightButton = GameObject.Find("RightMoveButton");
        Assert.IsNotNull(_rightButton,"Right Button is Null");
        
        //가스의 등장위치 파악하기
        Vector3 leftPosition = new Vector3(-1f, 0.2f, -3f);
        Vector3 rightPosition = new Vector3(1f, 0.2f, -3f);
        Vector3 centerPosition = new Vector3(0f, 0.2f, -3f);

        float rayDistance = 10f; 
        Vector3 rayDirection = Vector3.forward;
        
        //플레이 시간
        float elapsedTime = 0f; 
        float targetTime = 10f;

        
        //반복
        while (gameManager.GameState == GameManager.State.Play)
        {
            RaycastHit hit;
            if (Physics.Raycast(leftPosition, rayDirection, out hit, rayDistance,
                    LayerMask.GetMask("Gas")))
            {
                Debug.Log("Left");
                MoveCar(hit.point);
            }
            else if (Physics.Raycast(rightPosition, rayDirection, out hit, rayDistance,
                         LayerMask.GetMask("Gas")))
            {
                Debug.Log("Right");
                MoveCar(hit.point);
            }
            else if (Physics.Raycast(centerPosition, rayDirection, out hit, rayDistance,
                         LayerMask.GetMask("Gas")))
            {
                Debug.Log("Center");
                MoveCar(hit.point);
            }
            else
            {
                Debug.Log("No Hit");
                MoveButtonUp(_leftButton);
                MoveButtonUp(_rightButton);
            }
            
            Debug.DrawRay(leftPosition, rayDirection, Color.red);
            Debug.DrawRay(rightPosition, rayDirection, Color.green);
            Debug.DrawRay(centerPosition, rayDirection, Color.blue);
            
            //시간체크
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        if (elapsedTime < targetTime)
        {
            Assert.Fail("Game Time is too short");
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

    private void MoveButtonUp(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerUpHandler);
    }
    private void MoveButtonDown(GameObject moveButton)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(moveButton, pointerEventData, ExecuteEvents.pointerDownHandler);
    }

    private void MoveCar(Vector3 targetPosition)
    {
        if (Mathf.Abs(targetPosition.x - _carController.transform.position.x) < 0.01f)
        {
            return;
        }
        
        if (targetPosition.x < _carController.transform.position.x)
        {
            MoveButtonDown(_leftButton);
            MoveButtonUp(_rightButton);
        }
        else if (targetPosition.x > _carController.transform.position.x)
        {
            MoveButtonDown(_rightButton);
            MoveButtonUp(_leftButton);
        }
        else
        {
            MoveButtonUp(_leftButton);
            MoveButtonUp(_rightButton);
        }
    }
}
