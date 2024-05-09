using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PortalController : MonoBehaviour
{
    PostProcessProfile _ppProfile;
    Vignette _vignette;
    Grain _grain;
    float _teleportTime = 2.5f;
    float _currentTime = 0f;
    float _vigIntensity = 0f;
    float _grainIntensity = 0f;
    float _originIntensity = 0f;
    bool _inPortal = false;
    bool _usePortal = false;
    public float CurrentTime
    {
        get { return _currentTime; }
        set 
        {
            _currentTime = Mathf.Clamp(value, 0, _teleportTime);
            if (CurrentTime >= _teleportTime)
            {
                if (_usePortal == false)
                {
                    _usePortal = true;
                    _vignette.center.value = new Vector2(10, 10);
                    if (Managers.Room.StageLevel <= 3)
                    {
                        Managers.Game.SetPlayerStat();
                        Managers.Scene.ReloadCurrentScene();
                    }
                    else
                        Managers.Scene.LoadScene("Epilogue");
                }
            }
        }
    }
    void Start()
    {
        if (_ppProfile == null)
        {
            _ppProfile = GameObject.Find("PP Volume").GetComponent<PostProcessVolume>().profile;
            _ppProfile.TryGetSettings<Vignette>(out _vignette);
            _ppProfile.TryGetSettings<Grain>(out _grain);
        }
    }
    void Update()
    {
        switch (_inPortal)
        {
            case true:
                EnterPortal();
                break;
            case false:
                OutPortal();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _inPortal = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _inPortal = false;
            _originIntensity = _grainIntensity;
        }
    }
    void EnterPortal()
    {
        CurrentTime += Time.deltaTime;
        // vignette 강도 조절
        _vigIntensity = Mathf.Lerp(0, 1, CurrentTime / _teleportTime);
        _vignette.intensity.value = _vigIntensity;
        // grain 강도 조절
        _grainIntensity = Mathf.Lerp(0, 1, CurrentTime / _teleportTime);
        _grain.intensity.value = _grainIntensity;
    }
    void OutPortal()
    {
        CurrentTime -= Time.deltaTime;
        // vignette 강도 조절
        _vigIntensity = Mathf.Lerp(_originIntensity, 0, (_teleportTime - CurrentTime) / _teleportTime);
        _vignette.intensity.value = _vigIntensity;
        // grain 강도 조절
        _grainIntensity = Mathf.Lerp(_originIntensity, 0, (_teleportTime - CurrentTime) / _teleportTime);
        _grain.intensity.value = _grainIntensity;
    }
}
