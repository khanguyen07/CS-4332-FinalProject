using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState  //An Interface defines some functions that everything that impliments that interface needs to have
                         //   there are no actual behaviors in an interface, just some empty functions.  
{

    //prepares the state to be executed.  everything in an Interface is public by default so you do not need to write public anywhere
    void Enter(Enemy parent);

    void Update();

    void Exit();

}
