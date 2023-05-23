using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] GameObject sfx_shoot1;
    [SerializeField] GameObject sfx_shoot2;
    [SerializeField] GameObject sfx_shoot3;
    [SerializeField] GameObject sfx_shoot4;
    [SerializeField] GameObject sfx_shoot5;

    [SerializeField] GameObject sfx_explode1;
    [SerializeField] GameObject sfx_explode2;
    [SerializeField] GameObject sfx_explode3;
    [SerializeField] GameObject sfx_explode4;
    [SerializeField] GameObject sfx_explode5;

    [SerializeField] GameObject sfx_hit1;
    [SerializeField] GameObject sfx_hit2;
    [SerializeField] GameObject sfx_hit3;
    [SerializeField] GameObject sfx_hit4;
    [SerializeField] GameObject sfx_hit5;

    [SerializeField] GameObject sfx_win1;
    [SerializeField] GameObject sfx_win2;
    [SerializeField] GameObject sfx_win3;

    [SerializeField] GameObject sfx_ui_click_soft;
    [SerializeField] GameObject sfx_ui_click_verysoft;

    [SerializeField] GameObject sfx_ui_confirm;
    [SerializeField] GameObject sfx_ui_cancel;
    [SerializeField] GameObject sfx_ui_pause;

    Transform cam;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cam = Camera.main.transform;
    }

    //SHOOT
    public void Spawn_shoot1()
    {
        Instantiate(sfx_shoot1, cam.position, Quaternion.identity);
    }

    public void Spawn_shoot2()
    {
        Instantiate(sfx_shoot2, cam.position, Quaternion.identity);
    }

    public void Spawn_shoot3()
    {
        Instantiate(sfx_shoot3, cam.position, Quaternion.identity);
    }

    public void Spawn_shoot4()
    {
        Instantiate(sfx_shoot4, cam.position, Quaternion.identity);
    }

    public void Spawn_shoot5()
    {
        Instantiate(sfx_shoot5, cam.position, Quaternion.identity);
    }

    //EXPLODE
    public void Spawn_explode1()
    {
        Instantiate(sfx_explode1, cam.position, Quaternion.identity);
    }

    public void Spawn_explode2()
    {
        Instantiate(sfx_explode2, cam.position, Quaternion.identity);
    }

    public void Spawn_explode3()
    {
        Instantiate(sfx_explode3, cam.position, Quaternion.identity);
    }

    public void Spawn_explode4()
    {
        Instantiate(sfx_explode4, cam.position, Quaternion.identity);
    }

    public void Spawn_explode5()
    {
        Instantiate(sfx_explode5, cam.position, Quaternion.identity);
    }

    //UI BEEP
    public void Spawn_UI_click_Soft(bool ignorePause = false)
    {
        if (!ignorePause)
        {
            Instantiate(sfx_ui_click_soft, cam.position, Quaternion.identity);
        }
        else
        {
            Instantiate(sfx_ui_click_soft, cam.position, Quaternion.identity).GetComponent<AudioSource>().ignoreListenerPause = true;
        }
    }

    public void Spawn_UI_click_verysoft()
    {
        Instantiate(sfx_ui_click_verysoft, cam.position, Quaternion.identity);
    }

    //UI CONFIRM
    public void Spawn_UI_Confirm(bool ignorePause = false)
    {
        if (!ignorePause)
        {
            Instantiate(sfx_ui_confirm, cam.position, Quaternion.identity);
        }
        else
        {
            Instantiate(sfx_ui_confirm, cam.position, Quaternion.identity).GetComponent<AudioSource>().ignoreListenerPause = true;
        }
    }

    //UI CANCEL
    public void Spawn_UI_Cancel()
    {
        Instantiate(sfx_ui_cancel, cam.position, Quaternion.identity);
    }

    //UI HITS
    public void Spawn_Hits1()
    {
        Instantiate(sfx_hit1, cam.position, Quaternion.identity);
    }

    public void Spawn_Hits2()
    {
        Instantiate(sfx_hit2, cam.position, Quaternion.identity);
    }

    public void Spawn_Hits3()
    {
        Instantiate(sfx_hit3, cam.position, Quaternion.identity);
    }

    public void Spawn_Hits4()
    {
        Instantiate(sfx_hit4, cam.position, Quaternion.identity);
    }

    public void Spawn_Hits5()
    {
        Instantiate(sfx_hit5, cam.position, Quaternion.identity);
    }
}
