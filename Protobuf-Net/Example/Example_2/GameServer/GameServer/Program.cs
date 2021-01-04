using AhpilyServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using ProtoBuf;
using ProtoBuf.Meta;

namespace GameServer
{
    class Program
    {

        [ProtoContract]
        public class AccDto
        {
            [ProtoMember(1)]
            public int Id;

            [ProtoMember(2)]
            public string Acc;

            [ProtoMember(99)]
            public string Pwd;
        }


        static void Main(string[] args)
        {

            AccDto dto = new AccDto();
            dto.Id = 1;
            dto.Acc = "qwertt";
            dto.Pwd = "12345";

            byte[] buffer = PBSerialize(dto);

            for (int i = 0; i < buffer.Length; i++)
            {
                Console.Write(buffer[i]);
            }
            Console.WriteLine();
            Console.WriteLine("==========================");

            AccDto dto2 = PBDSerialize<AccDto>(buffer);

            Console.WriteLine("dto2 id:{0}  acc:{1} pwd:{2}", dto2.Id, dto2.Acc, dto2.Pwd);

            Console.WriteLine();
            Console.WriteLine("==========================");

            AccDto dto3 = (AccDto)PBDSerialize(buffer, typeof(AccDto));

            Console.WriteLine("dto2 id:{0}  acc:{1} pwd:{2}", dto3.Id, dto3.Acc, dto3.Pwd);


            ServerPeer server = new ServerPeer();
            //指定所关联的应用
            server.SetApplication(new NetMsgCenter());
            server.Start(6666, 10);

            //序列化 和 反序列化 方法的赋值
            //EncodeTool.decodeObjDelegate = PBDSerialize;

            EncodeTool.encodeObjDelegate = PBSerialize;

            Console.ReadKey();
        }


        public static object DecodeObj(byte[] valueBytes)
        {
            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object value = bf.Deserialize(ms);
                return value;
            }
        }


        /// <summary>
        /// 通过 protobuf 进行序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] PBSerialize(object value)
        {
            byte[] buffer = null;

            using (MemoryStream ms = new MemoryStream())
            {
                if (value != null)
                {
                    RuntimeTypeModel.Default.Serialize(ms, value);
                }

                ms.Position = 0;
                int length = (int)ms.Length;
                buffer = new byte[length];
                ms.Read(buffer, 0, length);
            }

            return buffer;
        }

        /// <summary>
        /// 通过 protobuf 进行反序列化 泛型形式
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <returns></returns>
        public static T PBDSerialize<T>(byte[] valueBytes)
        {
            object value = null;

            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                value = Serializer.Deserialize<T>(ms);
            }

            return (T)value;
        }

        /// <summary>
        /// 通过 protobuf 进行反序列化 system.type
        /// </summary>
        /// <param name="valueBytes"></param>
        /// <returns></returns>
        public static object PBDSerialize(byte[] valueBytes, Type type)
        {
            object value = null;

            using (MemoryStream ms = new MemoryStream(valueBytes))
            {
                value = RuntimeTypeModel.Default.Deserialize(ms, null, type);
            }

            return value;
        }

    }
}
