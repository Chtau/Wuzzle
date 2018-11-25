using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPickup
{
    Player.TargetTriggerType TargetTrigger { get; }
    bool Interact();
}
