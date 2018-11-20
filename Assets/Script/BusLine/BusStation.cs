using UnityEngine;
using System.Collections;

public class BusStation
{
    private Grid sidewalkBusStation0;
    private Grid sidewalkBusStation1;
    private Grid drivewayBusStation;
    public BusStation(Grid sidewalkBusStation0, Grid sidewalkBusStation1,Grid drivewayBusStation)
    {
        this.sidewalkBusStation0 = sidewalkBusStation0;
        this.sidewalkBusStation1 = sidewalkBusStation1;
        this.drivewayBusStation = drivewayBusStation;
    }


}
