using Library.Data.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Common
{
    public interface IMemberService
    {
        List<Member> GetAll(int pageSize = 1000, int page = 0);

        Member GetById(int id);
        


        void Insert(Member Member);

        void Update(Member Member);

        void Delete(Member Member);

    }
}
