using System;
using System.Threading.Tasks;

namespace HoleyForkingShirt
{
    public interface IPayment
    {
        string Run(string cardtype);
    }
}