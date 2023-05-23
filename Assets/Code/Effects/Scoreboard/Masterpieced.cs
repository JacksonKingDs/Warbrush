using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Masterpieced : MonoBehaviour
{
    #region Fields
    public Camerashake cameraShaker;
    public ParticleSystem confetti;
    public PixelExplosionManager pixelManager;

    public Text title;
    public Text review;
    public Sprite banana;
    public Image laurelLeft;
    public Image laurelRight;
    public Image[] P1Stars;
    public Image[] P2Stars;
    public Image[] P3Stars;
    public Image[] P4Stars;
    public Text[] Attack;
    public Text[] Acc;
    public Text[] Kills;
    public GameObject returnToMainText;

    public GameObject pf_blinkers;

    //Reference
    GM gm;
    Animator anim;
    RectTransform trans;
    List<Image[]> stars; //the 4 player's stars combined

    //STATS
    int winnerIndex;
    bool campaignWon;

    //Expanding 
    bool expanding = false;
    float maxSize = 1.57f;
    float expandSpeed = 10f;
    float size = 0f;

    //Blink
    int blinkTimes = 5;
    float blinkInterval = 0.5f;

    bool finished;

    //PHRASES
    string[] titlePhrases =
    {
        "RIVETING", "MASTERPIECE", "MAGNETIC", "AVANT GARDE", "PRICELESS", "PENSIVE",
        "PLESURE", "REAL ART", "MODERN ART", "SENSUAL", "IT'S OKAY", "HAUNTING", "HYPNOTIC",
        "SPEECHLESS", "INVITING", "SEDUCTIVE", "PROVOCATIVE", "MONUMENTAL", "ENORMOUS",
        "GIGANTIC", "STUPENDOUS", "TERRIBLE", "DEMOCRACY", "STRIKING", "ENDURING",
        "HISTORIC", "MYSTERIOUS", "STRANGE", "PRESIDENT", "CABBAGE", "CEASELESS",
        "THEORETICAL", "WANTING", "TRANQUIL", "ABUNDANT", "OPINIONATED", "ALOOF",
        "WONDEROUS", "RABBIT", "ABRASIVE", "WAITED", "WHISPERING", "CULTURED",
        "BIZARRE", "CAMOUFLAGE", "IMAGINATIVE","FUTURISTIC","OCEANIC",
        "SELECTIVE","NAUGHTY","PSYCHOTIC","SYSTEMIC","CHILDLIKE","ABSURD","HORSES",
        "ELECTRIC","RELIEVED","OVERJOYED","DERANGED","GUILTLESS","INFAMOUS","GUTSY",
        "SLAP ME","COHERENT","SPIRITUAL","ORBITAL","INDIGENOUS","DONKEY","ETHEREAL",
        "SHIVER","TRIBAL","POLARIZING","I CRIED","MY EYES","UTTER HYPE",
        "TYRANNICAL"
    };

    string[] reviewPhrase_won =
    {
        "\"Inspired change and transformed lives\"",
        "\"The painting of this generation and next gestation\"", "\"I was born to see this\"",
        "\"Unlike anything I have seen in the past\"", "\"Very authentic\"",
        "\"Nominated by BAFTA for Oscar and Uber\"", "\"A colorful and inquisitive mess\"",        
        "\"Visceral thrills on an epic scale\"",        "\"This is too much\"",
        "\"A colorful and inquisitive visual essay\"", "\"A piece of raw power\"",
        "\"Heartbreaking and then warmed what was broken\"",
        "\"A truly special experience\"", "\"This is what I dreamed about\"",
        "\"I'm yet again impressed by these masters\"", "\"Invigoratingly intelligent\"",
        "\"A work of great artistry and passion\"", "\"Exhilarating, electrifying, essential oil\"",
        "\"They made peace afterwards\"", "\"Shook the world\"", 
        "\"A coherent proposition of coordinated ideas\"",
        "\"Deconstructed a digestive intellectual framework\"", "\"An imagination of alternative existence of thought forms\"",
        "\"Beauty emerged from violence\"", "\"Captured the Zeitgeist doggy dog\"",
        "\"A snapshot of the human condition\"", "\"A hyperrealistic rainbow\"",
        "\"A demonstration of compositional intelligence\"",
        "\"Outsourced compositional intelligence\"", "\"Revolutionized freedom\"",
        "\"A renewed perspective on life\"",
        "\"A brief but embarrassing silence ensued\"", "\"Gave me a renewed perspective on life\"",
        "\"I really really understood\"", "\"It made me hungry\"", "\"Just like a spring roll\"",
        "\"An unflinching performance\"", "\"Intimate and beautifully drawn\"",
        "\"Battle-of-the-wills took a turn for the sinister\"",
        "\"A loveletter across time\"", "\"Rearranged the inconceivable\"", "\"An aforementioned hitherto\"",
        "\"Something of great vis a vis\"", "\"Took me back to when I was five\"",
        "\"When is it too much? Where do we draw the line?\"", "\"Who is art, which is crab\"",
        "\"A reflection for what form represents to our generation's voiceless\"",
        "\"When love is lost, we have art\"",
        "\"This is a piece about New York and London\"",
        "\"A portrait of our failing mental states\"",
        "\"It captured the anger that is prevalent among cities and citizens\"",
        "\"Overwhelming impression of depth and branching path\"", "\"Animals at work\"",
        "\"Disrupting the figurative traditions\"",
        "\"Cross-disciplinary and drawing collective attention\"",
        "\"Beauty emerged from violence\"", "\"I can't speak for the poem\"",
        "\"Knowledge is fallible and history is fabricated\"",
        "\"The end of separation between nature and culture\"",
        "\"Endless streaks of pleasure and despair\"", "\"An orbiting satellite of love\"",
        "\"A yardstick against which to measure human grief\"", "\"Struggling to figure out the plot\"",
        "\"Removed reality and detached from afar\"", 
        "\"A subversive series of abstract, colorful catastrophies\"",
        "\"Charging ahead in new territories\"", "\"Barbaric by design\"",
        "\"Very deep\"", "\"Truth-telling of the imaginative\"",
        "\"Altered the histories in which it participates and the audiences that receive it\"",
        "\"On exhibition in all galleries across country this summer\"",
        "\"Transformed the canvas into a concert hall\"",
        "\"Far moved from the airbrushed, CGI reality of Hollywood\"",
        "\"A reminder of the fact that culture belongs to, and can be made by, anyone\"",
        "\"Beware of binary narratives\"", "\"Reflections of the temporal frontiers of the world\"",
        "\"Absurd and needless\"",  "\"TSM!TSM!\"",
    };

    string[] reviewPhrase_loss =
    {
        "\"I know you are hurt\"",
        "\"The cameras picked up on that\"", "\"Your plays made me weep\"",
         "\"This loss aggrevated my self-love issues\"", 
         "\"I'm in pain and don't wish to hear the truth\"",
        "\"Not trying to be harsh but you really could've done better\"",
        "\"I feel your pain, dear one\"", 
         "\"Boundlessly lost\"", "\"They won't stop hurting me\"",
        "\"If I ever witness what disaster looks like\"", "I'm not surprised tbh",
        "Just, just go home", "Kinda bad if you ask me", "\"Took the color out of my eyes\"",
        "\"No laurels, you get bananas instead\"",
        "\"What did I just watch\"", "\"Demoted to Grass 5\""
};
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        //Reference
        gm = GM.instance;
        trans = GetComponent<RectTransform>();
        anim = GetComponent<Animator>();
        
        //Initialize

        stars = new List<Image[]> { P1Stars, P2Stars, P3Stars, P4Stars };
        for (int i = 0; i < 4; i++)
        {
            Attack[i].text = "";
            Acc[i].text = "";
            Kills[i].text = "";
        }
    }

    void Update () 
	{
        ////Debug
        //if (Input.GetKeyDown(KeyCode.P))
        //    StartExpand(0);

        if (finished)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || InputManager.Instance.AnyStart_Down || InputManager.Instance.AnyB_Down)
            {
                Debug.Log("quitting");
                if (GM.gameMode != GameMode.Campaign)
                {
                    FightSceneManager.instance.LevelCompleted();
                }
                else
                {
                    FightSceneManager.instance.LevelCompleted(campaignWon);
                }
            }
        }
	}
    #endregion

    #region Public
    public void StartExpand(int index)
    {
        review.text = reviewPhrase_won[Random.Range(0, reviewPhrase_won.Length)];
        

        //StartCoroutine(ShowBlinkers());
        if (GM.gameMode == GameMode.Coop_Arcade)
        {
            title.text = "WAVE " + EnemyManager.instance.wave;
            PlayConfettis();
        }
        else if (GM.gameMode == GameMode.Campaign)
        {
            if (campaignWon)
            {
                title.text = "LEVEL WON";
                PlayConfettis();

                if (GM.campaignMapIndex >= 39)
                {
                    title.text = "COMPLETE!";
                }

                campaignFailed = false;
            }
            else
            {
                campaignFailed = true;
                laurelLeft.sprite = banana;
                laurelRight.sprite = banana;
                title.text = "YOU LOST...";
                review.text = reviewPhrase_loss[Random.Range(0, reviewPhrase_loss.Length)];
            }
        }
        else
        {
            PlayConfettis();
            title.text = titlePhrases[Random.Range(0, titlePhrases.Length)];
        }

        //Animation
        anim.Play("WinExpand");
        expanding = true;
        Color winColor = Color.white;

        //Double check winner index is valid
        if (!campaignFailed)
        {
            if (index >= 0 && index < 4)
            {
                winnerIndex = index;
                winColor = gm.pallet_1_brown.Tank[winnerIndex];
            }
            else
            {
                winnerIndex = -1;
                Debug.Log("Index doesn't exist");
            }
        }
        else
        {
            winnerIndex = -1;
            Debug.Log("Campaign lost. Setting winner index to -1");
        }

        //Set winner color
       
        laurelLeft.color = winColor;
        laurelRight.color = winColor;
        title.color = winColor;

        StartCoroutine(DisplayPlayerStats());
    }

    void PlayConfettis ()
    {
        confetti.Play();
        pixelManager.Explode();
    }

    bool campaignFailed = false;

    IEnumerator DisplayPlayerStats()
    {
        yield return new WaitForSeconds(2.5f);
        //Display stats for all active players
        int[] attack = { 0, 0, 0, 0 };
        int[] accuracy = { 0, 0, 0, 0 };
        int[] kills = { 0, 0, 0, 0 };

        //Highlight highest sub-score
        int highestAttackNum = 0;
        int highestAttackIndex = 0;
        int highestAccuracyNum = 0;
        int highestAccuracyIndex = 0;
        int highestKillNum = 0;
        int highestKillIndex = 0;

        finished = true;
        for (int i = 0; i < 4; i++)
        {
            if (gm.playerType[i] == PlayerTypes.INACTIVE)
                continue;

            //Calculate Score
            attack[i] = FightSceneManager.attacks[i];
            int landed = FightSceneManager.landed[i];
            if (attack[i] <= 0 || landed <= 0)
            {
                //Debug.Log("attack " + attack[i] + " landed " + landed + "accuracy[i] " + accuracy[i]);
                accuracy[i] = 0;
            }
            else
            {
                accuracy[i] = (int)(((float)landed / (float)attack[i]) * 100f);
                //Debug.Log("attack " + attack[i] + " landed " + landed + "accuracy[i] " + accuracy[i]);
            }

            kills[i] = FightSceneManager.kills[i];
            int score = 1;

            if (GM.gameMode == GameMode.Coop_Arcade)
            {
                score = score + (int)((float)kills[i] / 10f);
            }
            else if (GM.gameMode == GameMode.Coop_Torch)
            {
                score = score + (int)((float)kills[i] / 8f);
            }
            else
            {
                score = score + (int)((float)kills[i] / 2f);
            }

            score = Mathf.Clamp(score, 1, 4);
            if (winnerIndex == i)
            {
                score = 5;
            }

            StartCoroutine(ShowStats(i, attack[i], accuracy[i], kills[i], score));

            //Calculate highest sub-score
            if (attack[i] > highestAttackNum)
            {
                highestAttackNum = attack[i];
                highestAttackIndex = i;
            }
            if (accuracy[i] > highestAccuracyNum)
            {
                highestAccuracyNum = accuracy[i];
                highestAccuracyIndex = i;
            }
            if (kills[i] > highestKillNum)
            {
                highestKillNum = kills[i];
                highestKillIndex = i;
            }
        }

        Attack[highestAttackIndex].color = gm.pallet_1_brown.Tank[highestAttackIndex];
        Acc[highestAccuracyIndex].color = gm.pallet_1_brown.Tank[highestAccuracyIndex];
        Kills[highestKillIndex].color = gm.pallet_1_brown.Tank[highestKillIndex];
    }

    public void CampaignWon (int winnerIndex)
    {
        campaignWon = true;
        StartExpand(winnerIndex);
    }

    public void CampaignLost(int winnerIndex)
    {
        campaignWon = false;
        StartExpand(winnerIndex);
    }
    #endregion
    
    IEnumerator ShowStarScore (int index, int starScore)
    {
        //Debug.Log("p " + index + ", score " + starScore);
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < starScore; i++)
        {
            stars[index][i].enabled = true;

            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ShowBlinkers ()
    {
        yield return new WaitForSeconds(6f);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(pf_blinkers, Vector3.zero, Quaternion.identity, trans).GetComponent<ScoreboardStar>().Initialize(gm.pallet_1_brown.Tank[winnerIndex], winnerIndex);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ShowStats(int index, int attack, int acc, int kills, int score)
    {
        Attack[index].text = "ATKS " + attack;
        yield return new WaitForSeconds(1f);
        Acc[index].text = "ACC " + acc + "%";
        yield return new WaitForSeconds(1f);
        Kills[index].text = "KILLS " + kills;

        StartCoroutine(ShowStarScore(index, score));
        //returnToMainText.SetActive(true);
    }
}

//IEnumerator DoBlink ()
//{
//    //BLINK TITLE
//    for (int i = 0; i < blinkTimes; i++)
//    {
//        if (i % 2 == 0) //even number
//        {
//            centerText.color = primaryColor;
//            shadow.effectColor = bgColor;
//        }
//        else
//        {
//            centerText.color = Color.white;
//            shadow.effectColor = primaryColor;
//        }

//        yield return new WaitForSeconds(blinkInterval);
//    }

//    centerText.color = primaryColor;
//    shadow.effectColor = bgColor;
//}