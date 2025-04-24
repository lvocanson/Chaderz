using Tanks.Complete;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
	[SerializeField] Material _shellMat, _grassMat;
	[SerializeField] Material _shellVignetteMat;
	private int _playerCount;
	private TankManager[] _tanks;

	List<GameObject> _shells = new();
	Vector4 _pos01 = new(float.NaN, float.NaN, float.NaN, float.NaN);
	Vector4 _pos23 = new(float.NaN, float.NaN, float.NaN, float.NaN);

	Texture2D clone;

	public static MaterialManager Instance;
	private void Awake() => Instance = this;

	private void Start()
	{
		clone =new(4, 1);
	}
	private void Update()
	{
		_shellMat.SetFloat("_UnscaledTime", Time.unscaledTime);
		_shellMat.SetFloat("_TimeScale", Time.timeScale);

		_shellVignetteMat.SetVector("_Pos01", _pos01);
		_shellVignetteMat.SetVector("_Pos23", _pos23);
		_shellVignetteMat.SetFloat("_ScreenWidth", Screen.width);
		_shellVignetteMat.SetFloat("_ScreenHeight", Screen.height);

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


	public void UpdateShellPosition(GameObject go, bool active)
	{
		int i = _shells.IndexOf(go);

		if (i == -1)
		{
			if (!active)
				// Not found and not active
				return;

			// Not found but active
			_shells.Add(go);
			i = _shells.Count - 1;
		}
		else if (!active)
		{
			// Found but not active
			_shells.RemoveAt(i);
			UpdateShellVector(i, new(float.NaN, float.NaN));
		}
		else
		{
			// Found and active
			Vector2 pos = Camera.main.WorldToScreenPoint(go.transform.position);
			UpdateShellVector(i, pos);
		}
	}

	private void UpdateShellVector(int index, Vector2 pos)
	{
		switch (index)
		{
			case 0: _pos01.x = pos.x; _pos01.y = pos.y; break;
			case 1: _pos01.z = pos.x; _pos01.w = pos.y; break;
			case 2: _pos23.x = pos.x; _pos23.y = pos.y; break;
			case 3: _pos23.z = pos.x; _pos23.w = pos.y; break;
		}
	}
}
