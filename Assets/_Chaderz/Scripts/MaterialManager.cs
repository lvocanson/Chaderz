using System;
using Tanks.Complete;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] Material _shellMat, _grassMat;
    private int _playerCount;
    private TankManager[] _tanks;
    
    Texture2D clone;

    private void Start()
    {
        clone =new(4, 1);
    }
    private void Update()
    {
        _shellMat.SetFloat("_UnscaledTime", Time.unscaledTime);
        _shellMat.SetFloat("_TimeScale", Time.timeScale);
        UpdateGrass();
    }

    private void OnDestroy()
    {
        // Reset (avoid git changes)
        _shellMat.SetFloat("_UnscaledTime", 0f);
        _shellMat.SetFloat("_TimeScale", 0f);
        _grassMat.SetTexture("_playerData", null);
    }

    internal void SetupGrass(int PlayerCount, TankManager[] m_SpawnPoints)
    {
        _playerCount = PlayerCount;
        _tanks = m_SpawnPoints;

        for (int i = 0; i < PlayerCount; i++)
        {
            Vector3 pos = _tanks[i].m_Instance.transform.position;
            pos += new Vector3(100, 100, 100);
            clone.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z));
        }
        clone.Apply();
        _grassMat.SetTexture("_playerData", clone);

        _grassMat.EnableKeyword("_PLAYERS_A");
        _grassMat.EnableKeyword("_PLAYERS_B");
        if (_playerCount > 2)
            _grassMat.EnableKeyword("_PLAYERS_C");
        if (_playerCount > 3)
            _grassMat.EnableKeyword("_PLAYERS_D");
    }

    internal void UpdateGrass()
    {
        if (_playerCount == 0) return;
        for (int i = 0; i < _playerCount; i++)
        {
            Vector3 pos = _tanks[i].m_Instance.transform.position;
            pos += new Vector3(100, 100, 100);
            pos = pos / 150;
            clone.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z));
        }
        clone.Apply();
        _grassMat.SetTexture("_playerData", clone);
    }
}
