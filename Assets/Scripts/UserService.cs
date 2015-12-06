using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
public class UserService : Singleton<UserService> {
	public string userName;
	public string userEmail;
	public string urlServidor;
	public string metaData;

	int highscore;

	public void Awake() {
		Application.ExternalCall("enviarDadosJogo", "Login Service");
	}
	public void SetUserData(string info) {
		string[] dados = info.Split(';');
		userName = dados[0];
		userEmail = dados[1];
		urlServidor = dados[2];
	}
	public void SetHighScore(int score){
		highscore = score;
	}
	public void CallSendScore(){
		StartCoroutine("sendScore");
	}

	public void CallgetScore(){
		StartCoroutine("getScore");
	}

	public IEnumerator sendScore() {
		string json = "{\"gameName\": \"vilaempreendedor\", \"highscore\": "+ highscore + ", \"metaData\": \"\", \"user\": {\"userName\": \"" + userName + "\", \"userEmail\": \"" + userEmail + "\"} }";
		Dictionary<string, string> hash = new Dictionary<string, string>();
		hash["Content-Type"] = "application/json";
		byte[] pData = Encoding.UTF8.GetBytes(json.ToCharArray());
		WWW w = new WWW(urlServidor + "/rest/game/sendhighscore", pData, hash);

		while (!w.isDone) {
			yield return null;
		}
		if (w.error != null || w.text != null) {
			Application.ExternalCall("sendScoreCallback", w.error != null ? w.error : w.text);
		}
		Debug.Log(w.error);
	}

	public IEnumerator getScore() {
		//string mjson = "{\"user\": {\"userName\": \"Nome do usuário armazenado no jogo\",\"userEmail\": \"Email do usuário armazenado no jogo\"},\"gameName\": \"Nome do jogo conforme tabela abaixo\",\"highscore\": 0,\"metaData\": \"configurações do jogo em Base64\",\"result\": {\"code\": 0,\"description\": \"\",\"detail\": \"\"}}";
		string json = "{\"gameName\": \"" + GameLog.kGameName + "\", \"highscore\": "+ highscore + ", \"metaData\": \"" + metaData + "\", \"user\": {\"userName\": \"" + userName + "\", \"userEmail\": \"" + userEmail + "\"} }";
		Dictionary<string, string> hash = new Dictionary<string, string>();
		hash["Content-Type"] = "application/json";
		byte[] pData = Encoding.UTF8.GetBytes(json.ToCharArray());
		WWW w = new WWW(urlServidor + "/rest/game/gethighscore", pData, hash);
	
		while (!w.isDone) {
			yield return null;
		}

		if (w.error != null || w.text != null) {
			Application.ExternalCall("getScoreCallback", w.error != null ? w.error : w.text);
		}
		Debug.Log(w.error);

		string result = Encoding.UTF8.GetString (w.bytes);
		List<string> parsed_result = result.Split (',').ToList();
		int metaData_index = parsed_result.FindIndex (x => x == "metaData:") + 1;
		metaData = parsed_result[metaData_index];
		
	}




}