using DataAccess.Models;
using DataAccess.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.banner
{
    public class BannerRepository :BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(PROJECT_PRN212Context context) : base(context) {
        
        }
    }
}
