using System;
using System.Runtime.Serialization;

[Flags]
public enum OrderStatus
{
    [EnumMember(Value = "None")]
    None = 0,

    [EnumMember(Value = "Pending")]
    Pending = 1 << 0, // 1

    [EnumMember(Value = "PaymentReceived")]
    PaymentReceived = 1 << 1, // 2

    [EnumMember(Value = "Shipped")]
    Shipped = 1 << 2, // 4

    [EnumMember(Value = "Delivered")]
    Delivered = 1 << 3, // 8

    [EnumMember(Value = "Cancelled")]
    Cancelled = 1 << 4 // 16
}
