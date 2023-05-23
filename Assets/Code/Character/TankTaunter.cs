using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TankTaunter : MonoBehaviour
{
    //Component
    public Animator anim;
    public Color desertColor;
    public Color NightColor;

    //UI elements reference
    public Text[] letters;
    public RectTransform uiTextsGroup;

    //Offsets
    public Vector3 constantOffset;
    public Vector3[] subGroupOffset; //Offset pos.x based on the length of the string being displayed.
    
    static string[] NormalTaunt =
    {
        "GG", "GL", "glhf", "Boss", "pOoR", "Winning",
        "Smoked", "Dunked", "Chunked", "200 IQ", "Played", "300 IQ",
        "Bow", "Ha!", "HAHA!", "boo", "donKey",  "Fight me",
        "Thug life", "Westcoast", "Slurp", "Ferret", "Burp",
        "Chewed", "Shoo", "Shush", "Trash", "Lost?", "Funky", "Poppin",
        "Fool", "Foolish", "Plump", "Gulp", "...", "Tired?", "Dyng?",
        "Back off", "I win", "UR bad",  "So bad", "Peace", "Shush", "Nope!",
        "Played!", "Son!", "Doggydog", "Chill", "Relax", "Yolo","bihhh",  "Hehe", "hw u feel", "Killn it",
        "Noob", "Nubs", "Epic", "Fail","Merked","Style","Flex", "Boom", "kaboom", "kekeke",
        "Boosted", "Smh", "Derp", "Dab", "Worth?", "ffs", "Nice", "TSM!", "Faker", "Uzi", 
        "Eaay", "1v9", "Bling", "Bomb", "Yeet!", "Yaaas", "Game", "???", "> >", "+ +","= =", "> <","- -;","^_<",
        "Shawty", "Wig split", "Clapped", "No scope", "Sniped",  "nope nope", "dream on", "!", "!!", "#_#", "@.@", 
        "hOW!?", "Watch it", "Pull back", "go go go", "GOAL", "Laugh", "Bad man", "Good boy",
        "Too slow", "Ducks", "Feeling", "WoW", "LoL", "whoa..", "Smack", "WhoA!",
        "Talk much", "Im best", "Jokes", "Later", "rly?", "loool", "TroLL", "Try hard", "Try more",
        ":)", "^ ^","hmm","zzz","yeah","ok","maybe", "true", "false", "hit me", "u mad",
        "u mad bra", "u okay?", "mad much", "shut down", "Squeek", "Evil", "Help", "Save me",
        "Pfff","tsk tsk","Not true","grrl","brrr", "beef", "kanye",
        "LuL", "(TT)", ":-D", ":O", ":'(", ";)", ":<", ":>", "XD", ":p", ">:p", ">:(", ">:)", ">:o", "Xo", "Eat it",
        ":d", ">-<", "(>o_o)>", "<(o_o<)", "T_T", "bagged", "no no no", "u b mad",
        "(-_^)", "o(^_-)O", "(*^o^*)", "(^_^)/", "(^o^)/", "?", "GGEZ", "pEaCe oUT" , "Rekt", "RekT", "Woof", "Bark", "Chirp" , "meow"
    }; 

    //Cache animation states
    int animState_wave;
    int animState_shake;
    int animState_shrink; //expand 1 at a time then shrink
    int animState_hide;

    //Script state
    bool taunting = false;
    Transform parent;
    Vector3 tgt_uiTextGroupOffset;

    public void Initialize(Transform parentTrans)
    {
        animState_wave      = Animator.StringToHash("wave");
        animState_shake     = Animator.StringToHash("shake");
        animState_shrink  = Animator.StringToHash("shrink");
        animState_hide      = Animator.StringToHash("hide");
        ClearTexts();
        parent = parentTrans;

        if (GM.gameMode == GameMode.PVP_Desert)
        {
            SetColor(desertColor);
        }
        else if (GM.gameMode == GameMode.PVP_Night )
        {
            SetColor(NightColor);
        }
        
    }

    void SetColor (Color c)
    {
        for (int i = 0; i < letters.Length; i++)
        {
            //Debug.Log("i " + i);
            //Debug.Log("letters[i] " + letters[i]);
            //Debug.Log("cs[i] " + cs[i]);
            letters[i].color = c;
        }
    }

    private void Update()
    {
        transform.position = parent.position + constantOffset;
        if (taunting)
        {
            uiTextsGroup.position = parent.position + tgt_uiTextGroupOffset;
        }
    }

    public void Taunt()
    {
        if (!taunting)
            StartCoroutine(DoTaunt());
    }

    public void StopTaunt_PublicHook ()
    {
        if (taunting)
        {
            ClearTexts();
        }
        else
        {
            StopTaunt();
        }
    }

    void StopTaunt ()
    {
        //Debug.Log("stop taunt");
        anim.Play(animState_hide, 0, 0);
        ClearTexts();
        taunting = false;
    }

    IEnumerator DoTaunt()
    {
        taunting = true;
        //Debug.Log("==============taunt");

        //Assign letters
        char[] cs = GetRandomTaunt().ToCharArray();

        //Debug.Log("cs " + cs.Length + ". cs: " + cs + ".letteers: " + letters.Length);
        tgt_uiTextGroupOffset = subGroupOffset[cs.Length];
        uiTextsGroup.position = parent.position + tgt_uiTextGroupOffset;
        //Debug.Log("tgt_uiTextGroupOffset" + tgt_uiTextGroupOffset);

        for (int i = 0; i < cs.Length; i++)
        {
            //Debug.Log("i " + i);
            //Debug.Log("letters[i] " + letters[i]);
            //Debug.Log("cs[i] " + cs[i]);
            letters[i].text = cs[i].ToString();
        }

        //Play animation
        anim.Play(GetRandomAnimation(), 0, 0);

        yield return new WaitForSeconds(1.5f);
        StopTaunt();
    }

    int GetRandomAnimation ()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                //Debug.Log("wave");
                return animState_wave;
            case 1:
                return animState_shrink;
            default:
                //Debug.Log("shake");
                return animState_shake;
        }
    }

    string GetRandomTaunt ()
    {
        return NormalTaunt[Random.Range(0, NormalTaunt.Length)];
    }

    void ClearTexts ()
    {
        foreach (var letter in letters)
        {
            letter.text = "";
        }
    }
}