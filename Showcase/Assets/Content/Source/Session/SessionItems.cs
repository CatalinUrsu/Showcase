using System;
using Source.MVP;
using System.Collections.Generic;

namespace Source.Session
{
[Serializable]
public class SessionItems
{
    public Dictionary<string, ItemModelWeapon> Weapons;
    public Dictionary<string, ItemModelShip> Ships;

    public SessionItems()
    {
        Weapons = new Dictionary<string, ItemModelWeapon>();
        Ships = new Dictionary<string, ItemModelShip>();
    }

    public SessionItems(SessionItems sessionItems)
    {
        Weapons = sessionItems.Weapons;
        Ships = sessionItems.Ships;
    }
}
}