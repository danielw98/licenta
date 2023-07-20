using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Enemy : Pathfinding
{
    public void HandlePathChanged()
    {
        isNewPathNeeded = true;
    }

    private void Update()
    {
        
    }
}