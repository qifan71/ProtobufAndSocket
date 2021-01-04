using UnityEngine;
using System.Collections;
using System;
using cs;
using Net;
using ProtoBuf;
using System.IO;

public class TestProtoNet : MonoBehaviour {

	// Use this for initialization
	void Start () {


        CSLoginInfo mLoginInfo = new CSLoginInfo();
        mLoginInfo.UserName = "linshuhe";
        mLoginInfo.Password = "123456";
        CSLoginReq mReq = new CSLoginReq();
        mReq.LoginInfo = mLoginInfo;

        byte[] data = CreateData((int)EnmCmdID.CS_LOGIN_REQ, mReq);
        ClientSocket mSocket = new ClientSocket();
        mSocket.ConnectServer("127.0.0.1", 8088);
        mSocket.SendMessage(data);
    }

    private byte[] CreateData(int typeId,IExtensible pbuf)
    {
        byte[] pbdata = PackCodec.Serialize(pbuf);
        ByteBuffer buff = new ByteBuffer();
        buff.WriteInt(typeId);
        buff.WriteBytes(pbdata);
        return WriteMessage(buff.ToBytes());
    }

    /// <summary>
    /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private static byte[] WriteMessage(byte[] message)
    {
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            ushort msglen = (ushort)message.Length;
            writer.Write(msglen);
            writer.Write(message);
            writer.Flush();
            return ms.ToArray();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
