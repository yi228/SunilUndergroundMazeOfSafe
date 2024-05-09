using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public float LerpTime = 0.5f;
    float timer = 0f;
    public Vector3 prev, next;
    public void UpdateCamera()
    {
        switch (Managers.Scene.CurrentScene)
        {
            case Define.Scene.InGame:
                StartCoroutine(CoFollowPlayer());
                break;
            case Define.Scene.OutGame:
                break;
            case Define.Scene.StartMenu:
                break;
            default:
                break;
        }
    }
    IEnumerator CoFollowPlayer()
    {
        while (timer < LerpTime)
        {
            timer += Time.deltaTime;

            if (LerpTime == 0)
            {
                transform.position = next;
                yield break;
            }

            Vector3 pos = new Vector3(Mathf.Lerp(prev.x, next.x, timer / LerpTime), Mathf.Lerp(prev.y, next.y, timer / LerpTime), -10);
            transform.position = pos;
            yield return null;
        }
        transform.position = new Vector3(next.x, next.y, -10);
        timer = 0f;
    }
    public Tilemap tilemap;
    void Start()
    {
        SetCameraSize();
    }
    void SetCameraSize()
    {
        tilemap = Managers.Room.Player.Room.transform.GetChild(0).GetComponent<Tilemap>();
        // 1. 타일맵 경계 찾기
        BoundsInt cellBounds = tilemap.cellBounds;
        Vector3 min = tilemap.CellToWorld(new Vector3Int(cellBounds.xMin, cellBounds.yMin, cellBounds.zMin));
        Vector3 max = tilemap.CellToWorld(new Vector3Int(cellBounds.xMax, cellBounds.yMax, cellBounds.zMax));
        Bounds bounds = new Bounds((min + max) / 2, max - min);

        // 2. Tilemap 중심 위치를 찾기
        Vector3 tilemapCenter = bounds.center;

        // 3. 카메라를 Tilemap 중심으로 이동
        transform.position = new Vector3(tilemapCenter.x, tilemapCenter.y, transform.position.z);

        // 4. 카메라 크기를 조절
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = bounds.size.y / 2 * differenceInSize;
        }
    }
}