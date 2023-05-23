using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListWrapper
{
    public List<Animator> tiles;
}

public class CaveStoryFader : MonoBehaviour, ICaveStory
{
    public List<ListWrapper> tiles;
    int animState_tileClose;
    int animState_tileOpen;

    #region MonoBehavior
    void Start()
    {
        animState_tileClose = Animator.StringToHash("TileClose");
        animState_tileOpen = Animator.StringToHash("TileOpen");

        OpenTiles();
    }

    private void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenTiles();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseTiles();
        }
    }
    #endregion

    #region Public
    public void CloseTiles ()
    {
        StartCoroutine(DoCloseTiles());
    }

    IEnumerator DoCloseTiles()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = 0; j < tiles[i].tiles.Count; j++)
            {
                tiles[i].tiles[j].Play(animState_tileClose);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void OpenTiles ()
    {
        StartCoroutine(DoOpenTiles());
    }

    IEnumerator DoOpenTiles()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j = 0; j < tiles[i].tiles.Count; j++)
            {
                tiles[i].tiles[j].Play(animState_tileOpen);
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
    #endregion


}
