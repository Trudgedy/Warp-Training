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


    public class GroupService : IGroupService
    {
        #region Constants
        private const string KEY_PATTERN = "WarpTraining.Group";
        private const string KEY_GET_ALL = "WarpTraining.Group.GetAll()";
        private const string KEY_GET_BY_ID = "WarpTraining.Group.GetById({0})";
        
        #endregion

        #region Fields
        IDataRepository<Group> _groupRepo;
        IDataRepository<Member> _memberRepo;
        ICacheManager _cacheManager;
        #endregion

        #region Constructor
        public GroupService(IDataRepository<Group> groupRepo, ICacheManager cacheManager, IDataRepository<Member> memberRepo)
        {
            _groupRepo = groupRepo;
            _cacheManager = cacheManager;
            _memberRepo = memberRepo;
        }
        #endregion

        public List<Data.Models.Common.Group> GetAll(int pageSize = 1000, int page = 0)
        {
            return _cacheManager.Get<List<Data.Models.Common.Group>>(KEY_GET_ALL, 10, () =>
            {
                return _groupRepo.Table.OrderBy(g => g.Name).Skip(page * pageSize).Take(pageSize).ToList();
            });
        }

        public Group GetById(int id)
        {
            string key = String.Format(KEY_GET_BY_ID, id);

            return _cacheManager.Get<Data.Models.Common.Group>(key, 10, () =>
            {
                return _groupRepo.Table.FirstOrDefault(r => r.GroupId == id);
            });
        }

        public Data.Models.Common.Group FindById(int id)
        {

            String key = String.Format(KEY_GET_BY_ID, id);
            return _cacheManager.Get<Data.Models.Common.Group>(key, 10, () =>
            {
                return _groupRepo.GetById(id);
            });

        }

        public void Insert(Group group)
        {
            _groupRepo.Insert(group);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
        }

        public void Update(Group group)
        {

            _groupRepo.Update(group);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
        }

        public void Delete(Group group)
        {
            _groupRepo.Delete(group);
            _cacheManager.RemoveByPattern(KEY_PATTERN);

        }

        public void Insert(Member member, string overload)
        {
            _memberRepo.Insert(member);
            _cacheManager.RemoveByPattern(KEY_PATTERN);
        }

    }
}
