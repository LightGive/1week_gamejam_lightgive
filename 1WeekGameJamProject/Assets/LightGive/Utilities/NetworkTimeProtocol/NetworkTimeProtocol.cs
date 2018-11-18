using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// NTPサーバから時間を取得する
/// </summary>
public static class NetworkTimeProtocol
{
	private static readonly DateTime BaseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	private static readonly string[] HostUrls = new string[]
	{
		"https://ntp-a1.nict.go.jp/cgi-bin/json",
		"https://ntp-b1.nict.go.jp/cgi-bin/json"
	};

	/// <summary>
	/// 現在の時間を取得
	/// </summary>
	/// <param name="_behaviour">コルーチンを開始するスクリプト</param>
	/// <param name="_callback">コールバック</param>
	public static void GetNow(MonoBehaviour _behaviour, Action<DateTime?> _callback)
	{
		_behaviour.StartCoroutine(_GetTime((c) =>
		{
			_callback(c);
		}));
	}

	/// <summary>
	/// 現在時間を取得するコルーチン
	/// </summary>
	/// <returns>現在時間</returns>
	/// <param name="_callback">現在時間を取得するコールバック</param>
	public static IEnumerator _GetTime(Action<DateTime?> _callback)
	{
		var now = (DateTime.UtcNow - BaseDateTime).TotalSeconds;
		var query = string.Format("?{0:F3}", now);
		var url = HostUrls[UnityEngine.Random.Range(0, HostUrls.Length)] + query;

		using (var request = UnityWebRequest.Get(url))
		{
			request.SetRequestHeader("Content-type", "application/json");
			yield return request.SendWebRequest();
			if (request.isNetworkError)
			{
				_callback(null);
			}
			else
			{
				var json = request.downloadHandler.text;
				var response = JsonUtility.FromJson<NictResponse>(json);
				_callback(BaseDateTime.ToLocalTime().AddSeconds(response.st));
			}
		}
	}

	#region "Inner class"
	public class NictResponse
	{
		public string id;
		public double it;
		public double st;
		public int leap;
		public long next;
		public int step;
	}
	#endregion

}