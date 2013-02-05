using System.Linq;

#region #### Globales Robots ####
public enum IDSensorsArduino : byte
{
    IR = 0x01,
    UltraSon = 0x02
};
#endregion

#region #### Enumeration Messages Headers ####
public enum PCtoEMBmessHeads : byte
{
    TURN = 0x51,
    MOVE = 0x52,
    CLOSE_CLAW = 0x61,
    OPEN_CLAW = 0x62,
    ASK_SENSOR = 0x31,
    ASK_PING = 0x21,
    RESP_CONN = 0x11
};
public enum EMBtoPCmessHeads : byte
{
    ASK_CONN = 0x12,
    RESP_PING = 0x22,
    RESP_SENSOR = 0x32,
    ACK = 0x41
};
#endregion

#region #### Global Objects ####
class MessageProtocol
{
    public byte headerMess;

    //public byte[] getBytes();// declaration de la fonction en abstract
    public virtual byte[] getBytes()
    {
        return new byte[] { this.headerMess };
    }

    /* Retourne un tableau de bytes a partir d'une structure sérialisable */
    /*public static byte[] getBytes(object strucutre)
    {
        // Create a memory stream, and serialize.
        using (MemoryStream stream = new MemoryStream())
        {
            // Create a binary formatter.
            IFormatter formatter = new BinaryFormatter();

            // Serialize.
            formatter.Serialize(stream, strucutre);

            // Now return the array.
            return stream.ToArray();
        }
    }*/
}

abstract class PCtoEMBmess : MessageProtocol
{
}
abstract class EMBtoPCmess : MessageProtocol
{
}
#endregion 

#region #### PCtoEMB Structures ####

/* Connection Managment */
class PCtoEMBMessageRespConn    : PCtoEMBmess
{
    public byte state = 0;
    public override byte[] getBytes()
    {
        byte[] data = { this.state };
        return base.getBytes().Concat(data).ToArray();
    }
}
class PCtoEMBMessagePing        : PCtoEMBmess
{
   
}

/* Deplacement Managment */
class PCtoEMBMessageTurn        : PCtoEMBmess
{
    public byte direction;
    public byte angle;
    public override byte[] getBytes()
    {
        byte[] data = { this.direction, this.angle };
        return base.getBytes().Concat(data).ToArray();
    }
}
class PCtoEMBMessageMove        : PCtoEMBmess
{
    public byte sens;
    public byte speed;
    public byte distance;
    public override byte[] getBytes()
    {
        byte[] data = { this.sens, this.speed, this.distance };
        return base.getBytes().Concat(data).ToArray();
    }
}

/* Claw Managment */
class PCtoEMBMessageOpenClaw    : PCtoEMBmess
 {
 }
class PCtoEMBMessageCloseClaw   : PCtoEMBmess
 {

 }

/* Sensor Managment */
class PCtoEMBMessageAskSensor   : PCtoEMBmess
{
    public byte idSensor = 0;
    public override byte[] getBytes()
    {
        byte[] data = { this.idSensor};
        return base.getBytes().Concat(data).ToArray();
    }
}

#endregion

#region #### EMBtoPC Structures ####

/* Connection Managment */
class EMBtoPCMessageAskConn     : EMBtoPCmess
{

}
class EMBtoPCMessageRespPing    : EMBtoPCmess
{
}

/* Sensor Managment */
class EMBtoPCMessageRespSensor  : EMBtoPCmess
{
    public byte idSensor = 0;
    public byte valueSensor = 0;
    public override byte[] getBytes()
    {
        byte[] data = { this.idSensor, this.valueSensor };
        return base.getBytes().Concat(data).ToArray();
    }
}

/* Global Ack */
class EMBtoPCMessageGlobalAck   : EMBtoPCmess
{
    public byte idCommand = 0;
    public byte valueAck = 0;
    public override byte[] getBytes()
    {
        byte[] data = { this.idCommand, this.valueAck };
        return base.getBytes().Concat(data).ToArray();
    }
}
#endregion