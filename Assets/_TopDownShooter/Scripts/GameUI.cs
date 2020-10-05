using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TopDownShooter
{
    public class GameUI : MonoBehaviour
    {
        public Image fadePlane;
        public GameObject gameOverUI;
        public float fadeTime = 1;

        public RectTransform newWaveBanner;
        public Text newWaveTitle;
        public Text newWaveEnemyCount;

        Spawner spawner;

        // Start is called before the first frame update
        void Start()
        {
            FindObjectOfType<Player>().OnDeath += OnGameOver;
        }

        private void Awake()
        {
            spawner = FindObjectOfType<Spawner>();
            spawner.OnNewWave += OnNewWave;
        }

        void OnNewWave(int waveNumber)
        {
            string[] numbers = { "One", "Two", "Three", "Four", "Five" };
            newWaveTitle.text = "- Wave " + numbers[waveNumber - 1] + " -";
            string enemyCountString = ((spawner.waves[waveNumber - 1].infinite) ? "Infinite" : "" + spawner.waves[waveNumber - 1].enemyCount);
            newWaveEnemyCount.text = "Enemies: " + enemyCountString;

            StopCoroutine("AnimateNewWaveBanner");
            StartCoroutine("AnimateNewWaveBanner");
        }

        IEnumerator AnimateNewWaveBanner()
        {
            float delayTime = 1.5f;
            float speed = 3f;
            float animatePercent = 0;
            float bannerHeight = 0;
            int dir = 1;

            float endDelayTime = Time.time + 1/speed + delayTime; //because why yield when you can yeet

            while (animatePercent >= 0) //Once the percent is below 0, the banner is off the screen and we can finish the animation
            {
                animatePercent += Time.deltaTime * speed * dir;

                if(animatePercent >= 1){
                    animatePercent = 1;
                    if(Time.time > endDelayTime){
                        dir = -1;
                    }
                }

                newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-280, bannerHeight, animatePercent);
                yield return null;
            }
        }

        void OnGameOver()
        {
            Cursor.visible = true;
            StartCoroutine(Fade(Color.clear, Color.black, fadeTime));
            gameOverUI.SetActive(true);
        }

        IEnumerator Fade(Color from, Color to, float time)
        {
            float speed = 1 / time;
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * speed;
                fadePlane.color = Color.Lerp(from, to, percent);
                yield return null;
            }
        }

        //UI Input
        public void StartNewGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
