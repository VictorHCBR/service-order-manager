using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceOrders.Application.Abstractions;

public interface IPasswordHasher
{
    string Hash(string password);
    bool VerifyPassword(string password, string passwordHash);
}
