using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Online_Shop___BackEnd.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetDatasFromSetting()
        {
            return await _context.Settings.ToDictionaryAsync(m => m.Key, m => m.Value);

        }
    }
}
