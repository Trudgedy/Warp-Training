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
        protected const String KEY_GET_PEOPLE_BY_ID = "WarpTraining.Member.GetPeople({0})";
        #endregion

        #region Fields
        protected IDataRepository<Member> _memberRepo;
        protected IDataRepository<Person> _personRepo;
        protected ICacheManager _cacheManager;
        #endregion

        #region Constructor
        public MemberService(IDataRepository<Member> memberRepo, IDataRepository<Person> personRepo, ICacheManager cacheManager)
        {
            _memberRepo = memberRepo;
            _personRepo = personRepo;
            _cacheManager = cacheManager;
        }
        #endregion

        public List<Member> GetAll(int pageSize = 1000, int page = 0)
        {
            return _cacheManager.Get<List<Data.Models.Common.Member>>(KEY_GET_ALL, 10, () =>
            {
                return _memberRepo.Table.OrderBy(p => p.GroupId).Skip(page * pageSize).Take(pageSize).ToList();
            });
        
        }

        public List<Person> GetPeople(int GroupId, int pageSize = 1000, int page = 0)
        {
            var key = String.Format(KEY_GET_PEOPLE_BY_ID, GroupId);

            return _cacheManager.Get<List<Data.Models.Common.Person>>(key, 10, () =>
            {
                var result = from p in _personRepo.Table
                             join m in _memberRepo.Table on p.PersonId equals m.PersonId
                             where m.GroupId == GroupId
                             select p;

                
                

                return result.OrderBy(p => p.Name).Skip(page * pageSize).Take(pageSize).ToList();
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
            _cacheManager.RemoveByPattern("WarpTraining.Person");
        }

        public void Update(Member member)
        {
            
            _memberRepo.Update(member);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
            _cacheManager.RemoveByPattern("WarpTraining.Person");
        }

        public void Delete(Member member)
        {
            _memberRepo.Delete(member);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
            _cacheManager.RemoveByPattern("WarpTraining.Person");
        }
    }
}
