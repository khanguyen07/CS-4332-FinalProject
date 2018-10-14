using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


//IUsable Interface is used for all of the Click-to-use items in the Game (potions, scrolls, etc)
public interface IUseable
{

    Sprite MyIcon
    {
        get;
    }

    void Use();
}
