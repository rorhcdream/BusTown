﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{


    private LinkedList<SidewalkNode> goToList;
    private Area dest;

    public void SetGoToList(LinkedList<SidewalkNode> gotoList)
    {
        this.goToList = gotoList;
    }
}
