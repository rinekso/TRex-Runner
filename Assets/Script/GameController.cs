using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [Header("Character")]
    [SerializeField]
    GameObject _TRex;
    Animator _TRexAnimator;
    public float _velocity = 3f;
    public float _jumpHeight = 2f;
    public float _jumpTime = 1f;
 
    [SerializeField]
    Vector3 _gravity = new Vector3(0f,-.6f,0f);
    Vector3 _gravityNormal = new Vector3(0f,-.6f,0f);
    Vector3 _gravityDown = new Vector3(0f,-.8f,0f);
    [SerializeField]
    CharacterController _controller;
    float _currentJumpTime = 0f;
 
    AnimationCurve _jumpCurve = new AnimationCurve(new Keyframe[5]{
 
        new Keyframe(0f  ,0f),
        new Keyframe(0.2f,.9f),
        new Keyframe(.5f  ,1f),
        new Keyframe(.7f  ,.8f),
        new Keyframe(1f  ,0f)
 
    });

    [Header("Obstacle")]
    [SerializeField]
    GameObject[] _prefabsObstacles;

    bool _isStart = false;
    float _timer = 0;
    [SerializeField]
    TextMeshPro _scoreText, _hiScoreText, _pushText, _gameOverText;
    public bool IsStart{
        get { return _isStart;}
    }
    float _iterator = 2;
    float _spawnTimer{
        get { return Random.RandomRange(2,4); }
    }
    [SerializeField]
    Transform _obstacleSpawn;
    int _currentScore;
    [SerializeField]
    bool DeleteKey = false;
    [SerializeField]
    BoxCollider collider;
    [SerializeField]
    GameObject _iconMove;
    [SerializeField]
    AlwaysFace _buttonStart;
    Vector3 _lastPos = Vector3.zero;
    [SerializeField]
    AudioTrigger audioImpact;
    void Awake(){
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _jumpCurve.SmoothTangents(1,.2f);
        _jumpCurve.SmoothTangents(2,.2f);
        _jumpCurve.SmoothTangents(3,.2f);
        _TRexAnimator = _TRex.GetComponent<Animator>();
        if(DeleteKey) PlayerPrefs.DeleteKey("HiScore");
        _hiScoreText.text = "Hi "+ (PlayerPrefs.HasKey("HiScore") ? PlayerPrefs.GetInt("HiScore").ToString("00000") : "00000");
    }
    void GameInit(){
        ObstacleScript[] obstacles = GameObject.FindObjectsOfType<ObstacleScript>(); 
        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i].gameObject);
        }

        // collider.enabled = false;
        GetComponentInChildren<HandGrabInteractable>().Disable();
        _iconMove.SetActive(false);
        _pushText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _currentScore = 0;
        _iterator = 1;
        _scoreText.text = "Score 00000";
        _timer = 0;
    }

    Vector3 Gravity{
 
        get{
            Ray ray = new Ray
            {
                origin = _controller.transform.position,
                direction = Vector3.down
            };
 

            if (Input.GetKeyUp(KeyCode.Space)) FinishJump();

            Debug.DrawRay(ray.origin,ray.direction,Color.green);

            print("on floor "+Physics.Raycast(ray,1f));
            //On floor
            if(Physics.Raycast(ray,_controller.height*.3f)){
                //Jump
                if(Input.GetKeyDown(KeyCode.Space)){
                    InitJump();
                }
            }
 
            //Jump time
            if(this._currentJumpTime > 0f){
 
                float height = this._jumpCurve.Evaluate((this._jumpTime - this._currentJumpTime)/this._jumpTime);
                this._currentJumpTime -= Time.deltaTime;
                if((this._jumpTime - this._currentJumpTime)/this._jumpTime > .6) _TRexAnimator.SetBool("Jump",false);
                return (  Vector3.up
                        * (this._jumpCurve.Evaluate((this._jumpTime - this._currentJumpTime)/this._jumpTime) - height )
                        * this._jumpHeight);
 
            }
 
            return this._gravity * Time.deltaTime;
 
        }
 
    }
    public void InitJump(){
        Ray ray = new Ray
        {
            origin = _controller.transform.position,
            direction = Vector3.down
        };
        if(Physics.Raycast(ray,_controller.height*.3f)){
            this._currentJumpTime = this._jumpTime;
            print("jump");
            _TRexAnimator.SetBool("Jump",true);

            if(!_isStart){
                GameInit();
                _isStart = true;
                _buttonStart.enabled = false;
            }
        }
    }
    public void FinishJump(){
        if(this._currentJumpTime > 0){
            this._currentJumpTime = 0;
            _TRexAnimator.SetBool("Jump",false);
        }
    }
    GameObject currentObstacle = null;
    public GameObject CurrentObstacle{
        set{
            currentObstacle = value;
        }
    }
     // Update is called once per frame
    void Update()
    {
        _controller.Move(  Gravity
                        + this.transform.up
                        * this._velocity
                        * Time.deltaTime);

        if(_isStart){
            _timer += Time.deltaTime;
            _currentScore = (int)(Round(_timer,1)*10);
            _scoreText.text = "Score "+(Round(_timer,1)*10).ToString("00000");

            // Spawn Obstacles
            if(currentObstacle == null && _timer > _iterator){
                // _iterator += _spawnTimer;
                currentObstacle = Instantiate(_prefabsObstacles[Random.Range(0,_prefabsObstacles.Length)],_obstacleSpawn);
                float standarSpeed = .5f;
                if(_currentScore/100 > 12){
                    currentObstacle.GetComponent<ObstacleScript>()._speed = standarSpeed*3f;
                }else if(_currentScore/100 > 8){
                    currentObstacle.GetComponent<ObstacleScript>()._speed = standarSpeed*2f;
                }else if(_currentScore/100 > 4){
                    currentObstacle.GetComponent<ObstacleScript>()._speed = standarSpeed*1.5f;
                }
            }
        }else{
            _controller.transform.localPosition = Vector3.zero;
            if(_buttonStart) _buttonStart.enabled = true;
        }
    }
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
    public void GameOver(){
        // collider.enabled = true;
        audioImpact.PlayAudio();
        _lastPos = _controller.transform.localPosition;
        GetComponentInChildren<HandGrabInteractable>().Enable();
        _iconMove.SetActive(true);
        _pushText.gameObject.SetActive(true);
        _isStart = false;
        _gameOverText.gameObject.SetActive(true);
        _TRexAnimator.SetTrigger("End");

        if(_currentScore > PlayerPrefs.GetInt("HiScore")){
            PlayerPrefs.SetInt("HiScore", _currentScore);
            _hiScoreText.text = "HI "+_currentScore.ToString("00000");
        }

        ObstacleScript[] obstacles = GameObject.FindObjectsOfType<ObstacleScript>(); 
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i]._speed = 0;
        }
    }
}
