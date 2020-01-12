using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

	public float score;
	bool finished;
	static ScoreController instance;
	public float gravityScoreMultiplier;
	public float rocketDivider;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		finished = false;
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeScale > 0){
			if(!GameObject.Find("Controllers").GetComponent<TimeCounter>().IsCounting() && GameObject.Find("Controllers").GetComponent<TimeCounter>().GetCurrentTimePlaying() > 0.5f){
				finished = true;
				switch(GetComponent<EndGameTrigger>().gameMode){
				case GameModes.CatchAndLand:
					CatchAndLand catchNLand = new CatchAndLand();
					catchNLand.Score();
					break;

				case GameModes.AgainstTime:
					AgainstTime againstTime = new AgainstTime();
					againstTime.Score();
					break;
				}
			}
		}
	}

	public static bool Finished(){
		return instance.finished;
	}
}


class CatchAndLand{
	public void Score(){
		bool hasDied = (!GameObject.FindWithTag("Player").GetComponent<RocketBase>().HasFlag() && GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetCurrentFuel() == 0.0f);
		int timesHit = GameObject.FindWithTag("Player").GetComponent<RocketBase>().HowManyTimesHitGround();
		float velocities = GameObject.FindWithTag("Player").GetComponent<RocketBase>().TotalHitVelocities();
		
		float score = 0;
		if(velocities != -1)
			score = CalculateScore(timesHit, velocities);
		else
			score = -1;
		
		if(hasDied)
			score = -1;
		
		score = float.Parse(score.ToString("####0.0"));
		
		GameObject.Find("Controllers").GetComponent<HUD>().SetAllTextsOff();
		GameObject.Find("Score Show").GetComponent<Text>().text = "" + ((score != -1) ? score.ToString() : "N/A");
		float highScore = TestIfHighScored(score);
		GameObject.Find("Highscore Show").GetComponent<Text>().text = "" + ((highScore > 0.1f) ? highScore.ToString("####0.0") : "N/A");
	}
	
	float TestIfHighScored(float score_){
		float r = PlayerPrefs.GetFloat("Highscore_CatchAndLand");
		
		if(score_ != -1){
			if(score_ > r || r < 1){
				score_ = float.Parse(score_.ToString("####0.0"));
				PlayerPrefs.SetFloat("Highscore_CatchAndLand", score_);
				r = score_;
			}
		}
		
		return r;
	}
	
	float CalculateScore(int timesHitGround, float totalVelocity){
		timesHitGround *= 25;
		totalVelocity *= 35;
		float med = (timesHitGround + Mathf.Abs(totalVelocity)) / 2;
		//Turn how greater the better
		med = 1/med;
		med *= 10000;

		med /= 5;

		med /= GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetProjectedTo();
		med *= -Physics2D.gravity.y;
		return med;
	}
}


class AgainstTime{	
	public void Score(){
		bool hasDied = (!GameObject.FindWithTag("Player").GetComponent<RocketBase>().HasFlag() && GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetCurrentFuel() == 0.0f);
		float time = GameObject.Find("Controllers").GetComponent<TimeCounter>().GetCurrentTimePlaying();
		float velocities = GameObject.FindWithTag("Player").GetComponent<RocketBase>().TotalHitVelocities();
		
		float score = 0;
		if(velocities != -1)
			score = time;
		else
			score = -1;

		if(hasDied)
			score = -1;
		
		score = float.Parse(score.ToString("####0.0"));

		score /= GameObject.FindWithTag("Player").GetComponent<RocketBase>().GetProjectedTo();
		score *= -Physics2D.gravity.y;
		
		GameObject.Find("Controllers").GetComponent<HUD>().SetAllTextsOff();
		GameObject.Find("Score Show").GetComponent<Text>().text = "" + ((score != -1) ? (score.ToString() + " s") : "N/A");
		float highScore = TestIfHighScored(score);
		GameObject.Find("Highscore Show").GetComponent<Text>().text = "" + ((highScore < 999f) ? (highScore.ToString("####0.0") + " s") : "N/A");
	}
	
	float TestIfHighScored(float score){
		float r = (PlayerPrefs.GetFloat("Highscore_AgainstTime") >= 1f) ? PlayerPrefs.GetFloat("Highscore_AgainstTime") : 999.0f;
		if(score > -1.0f){
			if(score < r){
				score = float.Parse(score.ToString("####0.0"));
				r = score;
				PlayerPrefs.SetFloat("Highscore_AgainstTime", r);
			}
		}
		return r;
	}
}
