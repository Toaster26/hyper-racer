using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject roadPrefab;
    
    //UI 관련
    [SerializeField] MoveButton leftMoveButton;
    [SerializeField] MoveButton rightMoveButton;
    [SerializeField] TMP_Text gasText;

    //
    private CarController _carController;
    
    //도로 오브젝트 풀
    private Queue<GameObject> _roadPool = new Queue<GameObject>();
    private int _roadPoolSize = 3;
    
    //도로 이동
    private List<GameObject> _activeRoads = new List<GameObject>();
    
    // 상태
    public enum State { Start, Play, End }
    public State GameState { get; private set; } = State.Start;

    //싱글턴
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    private void Start()
    {
        //도로 옵젝 풀
        InitializeRoadPool();
        
        //게임 상태를 Start로 변경
        GameState = State.Start;
        
        //게임 시작
        StartGame();
    }

    private void Update()
    {
        foreach (var activeRoad in _activeRoads)
        {
            activeRoad.transform.Translate(Vector3.back * Time.deltaTime);
        }
        //Gas정보 출력
        if (_carController != null) gasText.text = _carController.Gas.ToString();
    }

    private void StartGame()
    {
        //도로 생성
        SpawnRoad(Vector3.zero);
        
        //자동차 생성
        _carController = Instantiate(carPrefab, new Vector3(0, 0, -4f), Quaternion.identity)
            .GetComponent<CarController>();
        
        //left, right MoveButton에 자동차 컨트롤 기능 적용
        leftMoveButton.OnMoveButtonDown += () =>
        {
            _carController.Move(-1f);
        };
        rightMoveButton.OnMoveButtonDown += () =>
        {
            _carController.Move(1f);
        };
        
        //게임 상태를 Play로 변경
        GameState = State.Play;
    }

    #region Road Create and Management

    /// <summary>
    /// 도로 오브젝트 풀 초기화
    /// </summary>
    private void InitializeRoadPool()
    {
        for (int i = 0; i < _roadPoolSize; i++)
        {
            GameObject road = Instantiate(roadPrefab);
            road.SetActive(false);
            _roadPool.Enqueue(road);
        }
    }
    /// <summary>
    /// 도로 오브젝트 풀에서 불러와 배치하는 함수
    /// </summary>
    public void SpawnRoad(Vector3 position)
    {
        if (_roadPool.Count > 0)
        {
            GameObject road = _roadPool.Dequeue();
            road.transform.position = position;
            road.SetActive(true);
            
            //활성화 된 길을 움직이기 위해 List저장
            _activeRoads.Add(road);
        }
        else
        {
            GameObject road = Instantiate(roadPrefab, position, Quaternion.identity);
            _activeRoads.Add(road);
        }
    }
    
    public void DestroyRoad(GameObject road)
    {
        road.SetActive(false);
        _activeRoads.Remove(road);
        _roadPool.Enqueue(road);
    }

    #endregion

    
}
