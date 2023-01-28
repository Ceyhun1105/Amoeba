using Amoeba.DBContextFIles;
using Amoeba.Models;

namespace Amoeba.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public List<Setting> GetSettings()
        {
            return _context.Settings.ToList();
        }

    }
}
