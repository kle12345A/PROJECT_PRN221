using DataAccess.Models;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.banner
{
    public class BannerService : BaseService<Banner>, IBannerService
    {
        public BannerService(IUnitOfWork unitOfWork):base(unitOfWork) 
        {
        }
    }
}
