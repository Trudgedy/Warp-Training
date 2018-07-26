using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Library.Data;
using Library.Data.Models.Common;
using Library.Services.Cache;

namespace Library.Service.Common
{

    public class MemberService : IMemberService
    {
        #region Constants
        protected const String KEY_PATTERN = "WarpTraining.Member";
        protected const String KEY_GET_ALL = "WarpTraining.Member.GetAll()";
        protected const String KEY_GET_BY_ID = "WarpTraining.Member.GetById({0})";
        #endregion

        #region Fields
        protected IDataRepository<Member> _memberRepo;
        protected ICacheManager _cacheManager;
        #endregion

        #region Constructor
        public MemberService(IDataRepository<Member> memberRepo, ICacheManager cacheManager)
        {
            _memberRepo = memberRepo;
            _cacheManager = cacheManager;
        }
        #endregion

        public List<Member> GetAll(int page = 0, int pageSize = Int32.MaxValue)
        {
            return _cacheManager.Get<List<Data.Models.Common.Member>>(KEY_GET_ALL, 10, () =>
            {
                return _memberRepo.Table.OrderBy(p => p.GroupId).Skip(page * pageSize).Take(pageSize).ToList();
            });
        
        }

        public Member GetById(int id)
        {
            string key = String.Format(KEY_GET_BY_ID, id);
            return _cacheManager.Get<Member>(key, 10, () =>
            {
                //Filter
                return _memberRepo.Table.FirstOrDefault(r => r.GroupId == id);
            });
        }

        public void Insert(Member member)
        {
            
            _memberRepo.Insert(member);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
        }

        public void Update(Member member)
        {
            
            _memberRepo.Update(member);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
        }

        public void Delete(Member member)
        {
            _memberRepo.Delete(member);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
        }
    }
}
