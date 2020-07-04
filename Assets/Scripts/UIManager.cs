using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> unlockedSprites;
    [SerializeField]
    private List<Transform> boxes;
    [SerializeField]
    private Button unlockButton;

    private float timeBetween = 0.5f;
    private int selectedIx;
    private int lastNumber;

    //Kutular arasında bu sayıda gidip gelsin.
    [SerializeField]
    private int randomizingCount = 12;

    [SerializeField]
    private Color[] colors ;
      
    public void UnlockSingleBox()
    {
        //Butona basıldığında, işlem bitene kadar buton deaktif edilsin.
        unlockButton.interactable = false;
        StartCoroutine(PickBox());     
    }   

    public IEnumerator PickBox()
    {
        if (boxes.Count>1)
            //Eğer listede 2 den az eleman varsa, random kutu seçme işlemi gerçekleşmesin.
        {
            for (int a = 0; a < randomizingCount; a++)
            {
                int boxIx = GetUniqueRandom(0, boxes.Count); 

                for (int i = 0; i < boxes.Count; i++)
                {
                    if (i == boxIx)
                    {
                        boxes[i].GetComponent<Image>().color = colors[0];
                    }
                    else
                    {
                        boxes[i].GetComponent<Image>().color = colors[1];
                    }
                }
                selectedIx = boxIx;
                timeBetween -= timeBetween / (randomizingCount + 2);

                yield return new WaitForSeconds(timeBetween);
            }
        }
        else
        {
            boxes[0].GetComponent<Image>().color = colors[0];
            selectedIx = 0;
        }
        
        timeBetween = 0.5f;

        //Kutuyu unlock etmeden önce bir süre daha beklesin.
        yield return new WaitForSeconds(0.7f);
        boxes[selectedIx].GetComponent<Image>().enabled = false;
        boxes[selectedIx].GetChild(1).GetComponent<Image>().sprite = unlockedSprites[selectedIx];

        //Açılan kutular ve spritelar listelerden silinsin.
        boxes.Remove(boxes[selectedIx]);
        unlockedSprites.Remove(unlockedSprites[selectedIx]);
        if (boxes.Count!=0)
            unlockButton.interactable = true;                  

    }

    public int GetUniqueRandom(int min, int max)
    {
        int rand = Random.Range(min, max);
        while (rand == lastNumber)
            rand = Random.Range(min, max);
        lastNumber = rand;
        return rand;
    }
}
