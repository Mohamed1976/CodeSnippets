using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary
{
    [DataContract(Namespace = "http://www.williamgryan.mobi/Book/70-487")]
    [Flags]
    public enum AnswerDetails
    {
        [EnumMember]
        A = 0x0,
        [EnumMember]
        B = 0x1,
        [EnumMember]
        C = 0x2,
        [EnumMember]
        D = 0x4,
        [EnumMember]
        All = 0x8
    }
}
