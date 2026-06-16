using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace JamFramework
{
    /*
     * 
     * A specialized UI component for creating mutually exclusive gameobject toggling
     * Select child gameobjects can be considered "pages" of the slideshow, and only one
     * page may be shown at a time. Buttons can call Next and Previous page functions
     * to swap what's viewable.
     * 
     */
    public class Slideshow : MonoBehaviour
    {
        public List<GameObject> pages;


        public void Start()
        {
            SetPage(0);
        }

        public void SetPage(int pageIndex)
        {
            if (pageIndex >= pages.Count || pageIndex < 0) return;
            for (int i = 0; i < pages.Count; i++)
            {
                pages[i].SetActive(i == pageIndex);
            }
        }

        public void SetPage(string pageName)
        {
            pageName = pageName.ToLower();
            if (!pages.Any(x => x.name.ToLower() == pageName.ToLower())) return;

            foreach (var page in pages)
            {
                page.gameObject.SetActive(page.name.ToLower() == pageName);
            }
        }
    }
}
