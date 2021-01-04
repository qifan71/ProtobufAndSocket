using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
namespace ConsoleApplication1
{
    [ProtoContract]
    class TestData
    {
        [ProtoMember(1)]
        public int Id;

        [ProtoMember(2)]
        public string Acc;

        [ProtoMember(99)]
        public string Pwd;
    }
}
