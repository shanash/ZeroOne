using System;

#if !(NET5_0 || NET6_0 || NET7_0 || NET8_0 )
namespace ProtocolShared.Proto
{
    internal class RequiredAttribute : Attribute
    {
    }
}
#endif
